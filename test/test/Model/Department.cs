using System.ComponentModel.DataAnnotations;

namespace test.Models
{
    public class Department
    {
        [Key]
        public int department_id { get; set; }
        public string? department_name { get; set; }
    }
}
