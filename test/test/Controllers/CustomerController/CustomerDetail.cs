using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net.NetworkInformation;
using test.Models;

namespace test.Controllers.CustomerController
{
  [Route("api/cus/[controller]")]
  [ApiController]
  public class CustomerDetail : Controller
  {
    private readonly DataContext _context;
    public CustomerDetail(DataContext context)
    {
      _context = context;
    }

    public class Request
    {
      public int customer_id { get; set; }
    }

    [HttpGet("{customerId}")]
    public async Task<ActionResult<Customer>> GetDbTaskById(int customerId)
    {

      var customer = await GetCusById(customerId);

      if (customer == null)
      {
        return NotFound();
      }

      return customer;
    }

    private async Task<Customer> GetCusById(int customerId)
    {

      var customer = await (from c in _context.Set<Customer>()
                          where  c.customer_id == customerId
                             select c).SingleOrDefaultAsync();
      return customer;
    }
  }
}
