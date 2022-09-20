using System;
using System.Collections.Generic;

#nullable disable

namespace IRS.Models
{
    public partial class SysLogGp
    {
        public decimal Id { get; set; }
        public string UpperGuid { get; set; }
        public string Lat { get; set; }
        public string Lng { get; set; }
        public string Type { get; set; }
        public DateTime? CreateDate { get; set; }
        public string Ip { get; set; }
        public string Wip { get; set; }
        public string Action { get; set; }
        public string CallBy { get; set; }
    }
}
