using System.ComponentModel.DataAnnotations;

namespace test.Models
{
    public class Status
    {
        [Key]
        public int status_id { get; set; }
        public string status_phase { get; set; }
        public string status_desc { get; set; }
    }
}
