using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using System.Data.Entity.Infrastructure;
using System.Threading;
using test.Constants;
using test.Models;

namespace test.Controllers.TaskController
{
    [Route("api/task/[controller]")]
    [ApiController]
    public class Edit : Controller
    {
        private readonly DataContext _context;
        public Edit(DataContext context)
        {
            _context = context;
        }

        public class EditRequest : Dbtask
        {
            public string Action { get; set; }
            public string? StatusPhase { get; set; }
            public new IEnumerable<TaskDetailDto> TaskDetail { get; set; }
        }

        public class TaskDetailDto : TaskDetail
        {
            public string RowState { get; set; }
        }

        [HttpPut]
        public async Task<ActionResult> AttachDbtask(EditRequest EditRequest)
        {
            DateTime currentDateUtc = DateTime.UtcNow;
            Dbtask dbTask = new Dbtask();
            dbTask = EditRequest;

            if (EditRequest.Action == TestConstants.Action.Save)
            {
                dbTask.status = EditRequest.StatusPhase == null ? TestConstants.TaskStatus.Save : EditRequest.StatusPhase;
            }
            else if (EditRequest.Action == TestConstants.Action.Cancel)
            {
                dbTask.status = TestConstants.TaskStatus.Cancel;
            }

            //for checking date type null
            dbTask.task_date = EditRequest.task_date == null ? null : GetDateInUTC(EditRequest.task_date);
            dbTask.start_work_date = EditRequest.start_work_date == null ? null : GetDateInUTC(EditRequest.start_work_date);
            dbTask.appointment_date = EditRequest.appointment_date == null ? null : GetDateInUTC(EditRequest.appointment_date);
            dbTask.create_date = EditRequest.create_date == null ? currentDateUtc : GetDateInUTC(EditRequest.create_date);
            dbTask.update_date = currentDateUtc;
            dbTask.create_by = EditRequest.employee_id;

            this._context.dbtask.Attach(dbTask);
            this._context.Entry(dbTask).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            await _context.SaveChangesAsync();

            List<TaskDetail> deletedRow = new List<TaskDetail>();
            if (EditRequest.TaskDetail.Any(o => o.RowState == TestConstants.RowState.Delete))
            {
                foreach (TaskDetail detail in EditRequest.TaskDetail.Where(o => o.RowState == TestConstants.RowState.Delete))
                {
                    deletedRow.Add(detail);
                    _context.Set<TaskDetail>().Remove(detail);
                    _context.Entry(detail).State = Microsoft.EntityFrameworkCore.EntityState.Deleted;
                }
                await _context.SaveChangesAsync();
                EditRequest.TaskDetail = EditRequest.TaskDetail.Where(o => o.RowState != TestConstants.RowState.Delete).ToList();
            };

            foreach (TaskDetailDto detail in EditRequest.TaskDetail)
            {
                detail.task_id = dbTask.task_id.Value;
                detail.create_date = detail.create_date == null ? null : GetDateInUTC(detail.create_date);
                detail.create_by = EditRequest.employee_id;

                if (detail.RowState == TestConstants.RowState.Edit)
                {
                    detail.update_date = currentDateUtc;
                    this._context.Set<TaskDetail>().Attach(detail);
                    _context.Entry(detail).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                }
                else if (detail.RowState == TestConstants.RowState.Add)
                {
                    detail.update_date = null;
                    this._context.task_detail.Add(detail);
                }
            }

            if (deletedRow.Count > 0)
            {
                foreach (TaskDetail detail in deletedRow)
                {
                    SparePart sparePart = await GetSparePart(detail.spare_id);
                    if (sparePart != null)
                    {
                        sparePart.quantity += detail.detail_qty;
                        this._context.Set<SparePart>().Attach(sparePart);
                        _context.Entry(sparePart).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                    }
                }
            }
            else if (EditRequest.Action == TestConstants.Action.Cancel)
            {
                foreach (TaskDetail detail in EditRequest.TaskDetail)
                {
                    SparePart sparePart = await GetSparePart(detail.spare_id);
                    if (sparePart != null)
                    {
                        sparePart.quantity += detail.detail_qty;
                        _context.Entry(sparePart).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                        this._context.Set<SparePart>().Attach(sparePart);
                    }
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

        [ApiExplorerSettings(IgnoreApi = true)]
        public DateTime? GetDateInUTC(DateTime? Date)
        {
            return DateTime.Parse(Date.ToString()).ToUniversalTime();
        }
    }
}
