using System;
using System.Collections.Generic;

#nullable disable

namespace IRS.Models
{
    public partial class Account
    {
        public Account()
        {
            AccountPermissions = new HashSet<AccountPermission>();
            AccountRoles = new HashSet<AccountRole>();
            RefreshTokens = new HashSet<RefreshToken>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string No { get; set; }
        public string Rfid { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string AccountRole { get; set; }
        public string LicensePath { get; set; }
        public string PhotoPath { get; set; }
        public string Comment { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public DateTime? LastLogin { get; set; }
        public DateTime? LastUse { get; set; }
        public int PigAppLoginErrorTimes { get; set; }
        public int EquipmentAppLoginErrorTimes { get; set; }
        public int WebLoginErrorTimes { get; set; }
        public DateTime? PigAppLockedAt { get; set; }
        public DateTime? EquipmentAppLockedAt { get; set; }
        public int? AccountTypeId { get; set; }
        public int? AccountGroupId { get; set; }
        public int? Ocid { get; set; }
        public string Guid { get; set; }
        public bool? Status { get; set; }
        public int CreateBy { get; set; }
        public int? UpdateBy { get; set; }
        public int? DeleteBy { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public DateTime? DeleteDate { get; set; }
        public int? EmployeeId { get; set; }

        public virtual AccountType AccountType { get; set; }
        public virtual Employee Employee { get; set; }
        public virtual Oc Oc { get; set; }
        public virtual ICollection<AccountPermission> AccountPermissions { get; set; }
        public virtual ICollection<AccountRole> AccountRoles { get; set; }
        public virtual ICollection<RefreshToken> RefreshTokens { get; set; }
    }
}
