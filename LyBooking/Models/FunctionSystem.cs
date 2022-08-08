using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IRS.Models
{
    [Table("FunctionSystem")]
    public class FunctionSystem
    {

        public int ID { get; set; }
        [MaxLength(100)]
        public string Code { get; set; }
        [MaxLength(200)]
        public string VN { get; set; }
        [MaxLength(200)]
        public string EN { get; set; }
        [MaxLength(200)]
        public string CN { get; set; }
        [MaxLength(200)]
        public string TW { get; set; }
        public int Level { get; set; }
        [MaxLength(200)]
        public string Url { get; set; }
        public int Sequence { get; set; }
        [MaxLength(100)]
        public string Icon { get; set; }

        public int? ParentID { get; set; }
        [ForeignKey("ParentID")]
        public FunctionSystem Function { get; set; }
        public int? ModuleID { get; set; }
        public Module Module { get; set; }
    }
}
