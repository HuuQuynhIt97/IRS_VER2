using IRS.Models.Abstracts;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace IRS.Models
{
    [Table("Actions")]
    public class Action
    {
        public int ID { get; set; }
        public string Code { get; set; }
        public string VN { get; set; }
        [MaxLength(200)]
        public string EN { get; set; }
        [MaxLength(200)]
        public string CN { get; set; }
        [MaxLength(200)]
        public string TW { get; set; }
        public int Sequence { get; set; }
    }
}
