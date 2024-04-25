using System.ComponentModel.DataAnnotations;

namespace test.Models
{
    public class SpareType
    {
        [Key]
        public int sparetype_id { get; set; }
        public string? sparetype_name { get; set; }
    }
}
