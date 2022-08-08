using System;
using System.Collections.Generic;

#nullable disable

namespace IRS.Models
{
    public partial class SysConfig
    {
        public decimal Id { get; set; }
        public string ConfigType { get; set; }
        public decimal? ConfigNo { get; set; }
        public string ConfigValue { get; set; }
        public string Comment { get; set; }
        public string Status { get; set; }
        public DateTime? CreateDate { get; set; }
        public decimal? CreateBy { get; set; }
        public DateTime? UpdateDate { get; set; }
        public decimal? UpdateBy { get; set; }
        public int? Sort { get; set; }
        public string FarmGuid { get; set; }


    }
}
