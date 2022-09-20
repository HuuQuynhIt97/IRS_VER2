using System;
using System.Collections.Generic;

#nullable disable

namespace IRS.Models
{
    public partial class SystemLogUser
    {
        public int Id { get; set; }
        public string Page { get; set; }
        public string Type { get; set; }
        public string Key { get; set; }
        public string Comment { get; set; }
        public string Tw { get; set; }
        public string En { get; set; }
        public string Cn { get; set; }
        public string Vn { get; set; }
    }
}
