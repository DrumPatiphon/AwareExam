using System.ComponentModel.DataAnnotations;

namespace test.Models
{
    public class Employee
    {
        [Key]
        public int? employee_id { get; set; }
        public string? empfirst_name { get; set; }
        public string? emplast_name { get; set; }
        public int? department_id { get; set; }
        public string? empaddress { get; set; }
        public string? empphone_number { get; set; }
        public int? salary { get; set; }
        public bool? work_status { get; set; }
        public string? password { get; set; }
    }
}
