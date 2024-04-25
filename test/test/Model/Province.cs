using System.ComponentModel.DataAnnotations;

namespace test.Models
{
  public class Province
  {
    [Key]
    public int province_id { get; set; }
    public string province_name { get; set; }
  }
}
