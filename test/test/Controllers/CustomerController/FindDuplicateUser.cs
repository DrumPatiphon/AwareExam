using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using test.Models;

namespace test.Controllers.CustomerController
{
  [Route("api/cus/[controller]")]
  [ApiController]
  public class FindDuplicateUser : Controller
  {
    private readonly DataContext _context;
    public FindDuplicateUser(DataContext context)
    {
      _context = context;
    }

    public class RegisterRequest
    {
      public string PhoneNumber { get; set; }
      public string Name { get; set; }
    }

    [HttpGet]
    public async Task<Boolean> findDuplicateResult([FromQuery] RegisterRequest request)
    {
      var duplicate = await _context.Set<Customer>().AnyAsync(x => x.phone_number == request.PhoneNumber && x.first_name == request.Name);
      return duplicate;
    }
  }
}
