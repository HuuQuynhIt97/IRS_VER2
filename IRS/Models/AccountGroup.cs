using System;
using System.Collections.Generic;

#nullable disable

namespace IRS.Models
{
    public partial class AccountGroup
    {
        public decimal Id { get; set; }
        public decimal? ZoneId { get; set; }
        public decimal? BuildingId { get; set; }
        public string GroupNo { get; set; }
        public string GroupName { get; set; }
        public DateTime? CreateDate { get; set; }
        public decimal? CreateBy { get; set; }
        public DateTime? UpdateDate { get; set; }
        public decimal? UpdateBy { get; set; }
        public DateTime? DeleteDate { get; set; }
        public decimal? DeleteBy { get; set; }
        public decimal? Status { get; set; }
        public string Guid { get; set; }
    }
}
