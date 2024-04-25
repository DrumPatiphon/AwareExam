using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using test.Models;

namespace test.Controllers.CustomerController
{
  [Route("api/cus/[controller]")]
  [ApiController]
  public class CreateCustomer : Controller
  {
    private readonly DataContext _context;
    public CreateCustomer(DataContext context)
    {
      _context = context;
    }

    [HttpPost]
    public async Task<ActionResult> AddSparePart(Customer request)
    {
      Customer customer = await _context.Set<Customer>().OrderByDescending(o => o.customer_id).FirstOrDefaultAsync();
      request.customer_id = customer.customer_id + 1;
      this._context.Set<Customer>().Add(request);
      await this._context.SaveChangesAsync();
      return Ok(request.customer_id);
    }
  }
}
