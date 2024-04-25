using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net.NetworkInformation;
using test.Models;

namespace test.Controllers.TaskController
{
  [Route("api/task/[controller]")]
  [ApiController]
  public class Detail : Controller
  {
    private readonly DataContext _context;
    public Detail(DataContext context)
    {
      _context = context;
    }

    public class Request
    {
      public int task_id { get; set; }
    }

    public class DbTaskDto
    {
      public TaskDto DbTask { get; set; }
      public IEnumerable<TaskDetail> TaskDetail { get; set; }
    }

    public class TaskDto : Dbtask
    {
      public string customer_company { get; set; }
    }

    [HttpGet("{taskId}")]
    public async Task<ActionResult<DbTaskDto>> GetDbTaskById(int taskId)
    {

      var dbTask = await GetTaskById(taskId);

      if (dbTask == null)
      {
        return NotFound();
      }

      var taskDetail = await GetTaskDetailById(taskId);

      var dbTaskDto = new DbTaskDto
      {
        DbTask = dbTask,
        TaskDetail = taskDetail
      };

      return dbTaskDto;
    }

    private async Task<TaskDto> GetTaskById(int TaskId)
    {
      //var dbTask = await _context.Set<Dbtask>()
      //    .Where(o => o.task_id == TaskId)
      //    .SingleOrDefaultAsync();

      var dbTask = await (from dt in _context.Set<Dbtask>()
                          join c in _context.Set<Customer>() on dt.customer_id equals c.customer_id into cg
                          from lc in cg.DefaultIfEmpty()
                          where dt.task_id == TaskId
                          select new TaskDto
                          {
                            task_id = dt.task_id,
                            task_no = dt.task_no,
                            task_date = dt.task_date,
                            task_amt = dt.task_amt,
                            customer_id = lc.customer_id,
                            customer_name = dt.customer_name,
                            customer_lastname = dt.customer_lastname,
                            customer_phone = dt.customer_phone,
                            customer_company = lc.company_name,
                            customer_address = dt.customer_address,
                            employee_id = dt.employee_id,
                            license_desc = dt.license_desc,
                            remark = dt.remark,
                            status = dt.status,
                            province_id = dt.province_id,
                            start_work_date = dt.start_work_date,
                            appointment_date = dt.appointment_date,
                            create_date = dt.create_date,
                            update_date = dt.update_date,
                            create_by = dt.create_by,
                          }).SingleOrDefaultAsync();
      return dbTask;
    }

    private async Task<IEnumerable<TaskDetail>> GetTaskDetailById(int TaskId)
    {
      var taskDetails = await _context.Set<TaskDetail>()
          .Where(o => o.task_id == TaskId)
          .ToListAsync();

      return taskDetails;
    }


  }
}
