using System.ComponentModel.DataAnnotations;

namespace test.Models
{
    public class Dbtask
    {
        [Key]
        public int? task_id { get; set; }
        public string? task_no { get; set; }
        public string? license_desc { get; set; }
        public int? province_id { get; set; }
        public int? customer_id { get; set; }
        public string? customer_name { get; set; }
        public string? customer_lastname { get; set; }
        public string? customer_address { get; set; }
        public string? customer_phone { get; set; }
        public decimal? task_amt { get; set; }
        public string? remark { get; set; }
        public string? status { get; set; }
        public int? create_by { get; set; }
        public DateTime? create_date { get; set; }
        public DateTime? update_date { get; set; }
        public DateTime? start_work_date { get; set; }
        public DateTime? appointment_date { get; set; }
        public int? employee_id { get; set; }
        public DateTime? task_date { get; set; }
        //public IEnumerable<TaskDetail> TaskDetail { get; set; }
    }
}
