using System;
using System.Collections.Generic;

#nullable disable

namespace IRS.Models
{
    public partial class XAccountSetting
    {
        public decimal Id { get; set; }
        public string AccountGuid { get; set; }
        public string Page { get; set; }
        public string ControllerName { get; set; }
        public string ControllerValue { get; set; }
    }
}
