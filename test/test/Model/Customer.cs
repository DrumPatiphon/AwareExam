using System.ComponentModel.DataAnnotations;

namespace test.Models
{
  public class Customer
  {
    [Key]
    public int? customer_id { get; set; }
    public string? first_name { get; set; }
    public string? last_name { get; set; }
    public string? company_name { get; set; }
    public string? address { get; set; }
    public string? phone_number { get; set; }
  }
}
