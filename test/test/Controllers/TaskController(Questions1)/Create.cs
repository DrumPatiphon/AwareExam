using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using test.Models;
using test.Constants;

namespace test.Controllers.TaskController
{
  [Route("api/task/[controller]")]
  [ApiController]
  public class Create : Controller
  {
    private readonly DataContext _context;
    public Create(DataContext context)
    {
      _context = context;
    }

    public class Request : Dbtask
    {
      public string? StatusPhase { get; set; }
      public new IEnumerable<TaskDetail> TaskDetail { get; set; }
    }

    public class TaskDetailDto : TaskDetail
    {
      public string RowState { get; set; }
    }

    [HttpPost]
    public async Task<ActionResult> AddDbtask(Request request)
    {
      DateTime currentDateUtc = DateTime.UtcNow;
      string formattedDate = currentDateUtc.ToString("yyyyMM");
      Random random = new Random();
      int runningDoc = random.Next(100, 1000);

      Dbtask dbTask = new Dbtask();
      dbTask = request;
      dbTask.task_no = "TN" + "-" + formattedDate + runningDoc;
      dbTask.status = request.StatusPhase == null? TestConstants.TaskStatus.Save : request.StatusPhase;
      dbTask.task_amt = request.task_amt;

      //for checking date type null
      dbTask.task_date = request.task_date == null ? null : DateTime.Parse(request.task_date.ToString()).ToUniversalTime();
      dbTask.start_work_date = request.start_work_date == null ? null : DateTime.Parse(request.start_work_date.ToString()).ToUniversalTime();
      dbTask.appointment_date = request.appointment_date == null ? null : DateTime.Parse(request.appointment_date.ToString()).ToUniversalTime();
      dbTask.create_date = currentDateUtc;
      dbTask.create_by = request.employee_id;
      dbTask.update_date = null;

      this._context.dbtask.Add(dbTask);
      await this._context.SaveChangesAsync();

      foreach (TaskDetail detail in request.TaskDetail)
      {
        detail.task_id = dbTask.task_id.Value;
        detail.create_date = currentDateUtc;
        detail.update_date= null;
        detail.create_by = request.employee_id;
        this._context.task_detail.Add(detail);
      }

      // Update spare qty
      foreach (TaskDetail detail in request.TaskDetail)
      {
        SparePart sparePart = await GetSparePart(detail.spare_id);
        if (sparePart != null)
        {
          sparePart.quantity -= detail.detail_qty;
          if (sparePart.quantity <= 0)
          {
            Exception exception = new BadHttpRequestException("จำนวนอะไหล่มีค่าน้อยกว่าที่กำหนด กรุณาตรวจสอบ");
          }
          this._context.Set<SparePart>().Attach(sparePart);
          _context.Entry(sparePart).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
        }
      }

      await this._context.SaveChangesAsync();
      return Ok(dbTask.task_id);
    }

    [ApiExplorerSettings(IgnoreApi = true)]
    public async Task<SparePart> GetSparePart(int spareId)
    {
      SparePart sparePart = await _context.Set<SparePart>()
        .Where(o => o.spare_id == spareId).SingleOrDefaultAsync();
      return sparePart;
    }
  }
}
