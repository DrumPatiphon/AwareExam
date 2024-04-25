using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data.Entity.Infrastructure;
using System.Threading;
using test.Models;

namespace test.Controllers.CustomerController
{
  [Route("api/cus/[controller]")]
  [ApiController]
  public class EditCustomer : Controller
  {
    private readonly DataContext _context;
    public EditCustomer(DataContext context)
    {
      _context = context;
    }

    public class CustomerDto : Customer
    {
      public string Action { get; set; }
    }

    [HttpPut]
    public async Task<ActionResult> AttachSparePart(CustomerDto EditRequest)
    {
      Customer customer = new Customer();
      customer = EditRequest;
      if(EditRequest.Action == "Save")
      {
        this._context.Set<Customer>().Attach(customer);
        this._context.Entry(customer).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
      }
      else if(EditRequest.Action == "Delete")
      {
        this._context.Set<Customer>().Remove(customer);
        this._context.Entry(customer).State = Microsoft.EntityFrameworkCore.EntityState.Deleted;
      }
      await this._context.SaveChangesAsync();
      return Ok(customer.customer_id);
    }
  }
}
