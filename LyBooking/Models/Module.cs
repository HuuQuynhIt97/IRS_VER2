using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IRS.Models
{
    public class Module
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
        [MaxLength(200)]
        public string Url { get; set; }
        [MaxLength(200)]
        public string Icon { get; set; }
        public int Sequence { get; set; }
        public DateTime CreatedTime { get; set; }
        public ICollection<FunctionSystem> Functions { get; set; }
    }
}
