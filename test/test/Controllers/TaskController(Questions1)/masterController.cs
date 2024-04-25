using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using test.Models;

namespace test.Controllers.TaskController
{
  [Route("api/task/[controller]")]
  [ApiController]
  public class MasterDataController : ControllerBase
  {
    private readonly DataContext _context;

    public MasterDataController(DataContext context)
    {
      _context = context;
    }
    public class MasterData
    {
      public IEnumerable<dynamic> CustomerData { get; set; }
      public IEnumerable<dynamic> Status { get; set; }
      public IEnumerable<dynamic> TaskNo { get; set; }
      public IEnumerable<dynamic> Province { get; set; }
      public IEnumerable<dynamic> Employee { get; set; }
      public IEnumerable<dynamic> SpareData { get; set; }
    }

    [HttpGet]
    public async Task<MasterData> GetMasterData()
    {
      MasterData masterData = new MasterData();
      masterData.CustomerData = await GetCustomerData();
      masterData.Status = await GetStatus();
      masterData.TaskNo = await GetTaskNo();
      masterData.Province = await GetProvince();
      masterData.Employee = await GetEmployee();
      masterData.SpareData = await GetSpareData();


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

    private async Task<IEnumerable<dynamic>> GetStatus()
    {
      var status = await (from s in _context.Set<Status>()
                          orderby s.status_id
                          select new
                          {
                            Value = s.status_phase,
                            Text = s.status_desc
                          }).ToListAsync();
      return status;
    }

    private async Task<IEnumerable<dynamic>> GetTaskNo()
    {
      var dbtask = await (from t in _context.Set<Dbtask>()
                          select new
                          {
                            Value = t.task_no,
                            Text = t.task_no,
                            customerPhone = t.customer_phone,
                          }).ToListAsync();
      return dbtask;
    }

    private async Task<IEnumerable<dynamic>> GetProvince()
    {
      var province = await (from p in _context.Set<Province>()
                            select new
                            {
                              Value = p.province_id,
                              Text = p.province_name
                            }).ToListAsync();
      return province;
    }

    private async Task<IEnumerable<dynamic>> GetEmployee()
    {
      var employee = await (from e in _context.Set<Employee>()
                            select new
                            {
                              Value = e.employee_id,
                              Text = string.Concat(e.employee_id, " : ", e.empfirst_name, " ", e.emplast_name),
                            }).ToListAsync();
      return employee;
    }

    private async Task<IEnumerable<dynamic>> GetSpareData()
    {
      var employee = await (from s in _context.Set<SparePart>()
                            orderby s.spare_id
                            select new
                            {
                              Value = s.spare_id,
                              Text = string.Concat(s.spare_id, " : ", s.spare_name),
                              SpareName = s.spare_name,
                              SparePrice = s.spare_price,
                              SpareQty = s.quantity,
                            }).ToListAsync();
      return employee;
    }

  }
}
