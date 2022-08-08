using System;
using System.Collections.Generic;

#nullable disable

namespace IRS.Models
{
    public partial class AccountPermission
    {
        public decimal Id { get; set; }
        public string PermissionNo { get; set; }
        public string PermissionName { get; set; }
        public decimal? Sort { get; set; }
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
