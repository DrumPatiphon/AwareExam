using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using test.Models;

namespace test.Controllers.CustomerController
{
  [Route("api/cus/[controller]")]
  [ApiController]
  public class CustomerList : Controller
  {
    private readonly DataContext _context;
    public CustomerList(DataContext context)
    {
      _context = context;
    }

    public class Request
    {
      public int? CustomerId { get; set; }
      public string? PhoneNumber { get; set; }
    }

    [HttpGet]
    public async Task<IEnumerable<dynamic>> GetSearchList([FromQuery] Request request)
    {
      var listResult = await (from c in _context.Set<Customer>()
                              select new
                              {
                                customerId = c.customer_id,
                                customerName = string.Concat( c.first_name, " ", c.last_name),
                                phoneNumber = c.phone_number,
                              }).OrderBy(o => o.customerId).ToListAsync();

      if (request.CustomerId != null)
      {
        listResult = listResult.Where(o => o.customerId == request.CustomerId).ToList();
      }

      if (request.PhoneNumber != null)
      {
        listResult = listResult.Where(o => o.phoneNumber == request.PhoneNumber).ToList();
      }

      return listResult;
    }
  }
}
