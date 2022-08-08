using System;
using System.Collections.Generic;

#nullable disable

namespace IRS.Models
{
    public partial class XAccountGroup
    {
        public decimal Id { get; set; }
        public string FarmGuid { get; set; }
        public string GroupNo { get; set; }
        public string GroupName { get; set; }
        public string CancelFlag { get; set; }
        public DateTime? CreateDate { get; set; }
        public decimal? CreateBy { get; set; }
        public DateTime? UpdateDate { get; set; }
        public decimal? UpdateBy { get; set; }
        public decimal? Status { get; set; }
        public string Guid { get; set; }
    }
}
