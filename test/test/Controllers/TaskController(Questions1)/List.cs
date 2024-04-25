using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using test.Models;

namespace test.Controllers.TaskController
{
    [Route("api/task/[controller]")]
    [ApiController]
    public class List : Controller
    {
        private readonly DataContext _context;
        public List(DataContext context)
        {
            _context = context;
        }

        public class Request
        {
            public int? CustomerId { get; set; }
            public string? DocStatus { get; set; }
            public string? SdocNo { get; set; }
            public string? EdocNo { get; set; }
            public string? LicenseName { get; set; }
            public string? PhoneNo { get; set; }
        }

        [HttpGet]
        public async Task<IEnumerable<dynamic>> GetSearchList([FromQuery] Request request)
        {
            var listResult = await (from dt in _context.Set<Dbtask>()
                                    join s in _context.Set<Status>() on dt.status equals s.status_phase
                                    select new
                                    {
                                        taskId = dt.task_id,
                                        taskNo = dt.task_no,
                                        taskDate = dt.task_date,
                                        cusId = dt.customer_id,
                                        cusName = string.Concat(dt.customer_name, " ", dt.customer_lastname),
                                        taskAmt = dt.task_amt,
                                        status = dt.status,
                                        statusDesc = s.status_desc,
                                        licenseName = dt.license_desc,
                                        phoneNo = dt.customer_phone,
                                    }).OrderByDescending(o => o.taskId).ToListAsync();

            if (request.CustomerId != null)
            {
                listResult = listResult.Where(o => o.cusId == request.CustomerId).ToList();
            }

            if (request.DocStatus != null)
            {
                listResult = listResult.Where(o => o.status == request.DocStatus).ToList();
            }

            if (request.LicenseName != null)
            {
                listResult = listResult.Where(o => o.licenseName == request.LicenseName).ToList();
            }

            if (request.SdocNo != null)
            {
                listResult = listResult.Where(o => string.Compare(o.taskNo, request.SdocNo) >= 0).ToList();
            }

            if (request.EdocNo != null)
            {
                listResult = listResult.Where(o => string.Compare(o.taskNo, request.SdocNo) <= 0).ToList();
            }

            if (request.PhoneNo != null)
            {
                listResult = listResult.Where(o => o.phoneNo == request.PhoneNo).ToList();
            }


            return listResult;
        }
    }
}
