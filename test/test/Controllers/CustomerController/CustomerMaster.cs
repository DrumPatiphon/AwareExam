using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using test.Models;

namespace test.Controllers.CustomerController
{
  [Route("api/cus/[controller]")]
  [ApiController]
  public class CustomerMaster : ControllerBase
  {
    private readonly DataContext _context;

    public CustomerMaster(DataContext context)
    {
      _context = context;
    }
    public class CustomerMasterData
    {
      public IEnumerable<dynamic> CustomerData { get; set; }
    }

    [HttpGet]
    public async Task<CustomerMasterData> GetMasterData()
    {
      CustomerMasterData masterData = new CustomerMasterData();
      masterData.CustomerData = await GetCustomerData();
 
      return masterData;
    }

    private async Task<IEnumerable<dynamic>> GetCustomerData()
    {
      var customers = await (from c in _context.Set<Customer>()
                             select new
                             {
                               Value = c.customer_id,
                               Text = string.Concat(c.customer_id, " : ", c.first_name, " ", c.last_name),
                               FirstName = c.first_name,
                               LastName = c.last_name,
                               CompanyName = c.company_name,
                               Address = c.address,
                               PhoneNumber = c.phone_number
                             }).ToListAsync();

      return customers;
    }

  }
}
