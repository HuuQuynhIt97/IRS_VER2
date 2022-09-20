using IRS.Models.Abstracts;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IRS.Models
{
    [Table("Roles")]
    public class Role
    {
        public int ID { get; set; }
        [MaxLength(50)]
        public string Code { get; set; }
        [MaxLength(200)]
        public string VN { get; set; }
        [MaxLength(200)]
        public string EN { get; set; }
        [MaxLength(200)]
        public string CN { get; set; }
        [MaxLength(200)]
        public string TW { get; set; }
    }
}
