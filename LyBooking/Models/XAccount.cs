using IRS.Models.Interface;
using System;
using System.Collections.Generic;

#nullable disable

namespace IRS.Models
{
    public partial class XAccount: IAuditEntity
    {
        public decimal AccountId { get; set; }
        public decimal? ClinicId { get; set; }
        public string Uid { get; set; }
        public string Upwd { get; set; }
        public string AccountNo { get; set; }
        public string AccountName { get; set; }
        public string AccountSex { get; set; }
        public DateTime? AccountBirthday { get; set; }
        public string AccountNickname { get; set; }
        public string AccountTel { get; set; }
        public string AccountMobile { get; set; }
        public string AccountAddress { get; set; }
        public string AccountIdcard { get; set; }
        public string AccountEmail { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public DateTime? Lastlogin { get; set; }
        public DateTime? Lastuse { get; set; }
        public string Comment { get; set; }
        public string Status { get; set; }
        public string Token { get; set; }
        public string CancelFlag { get; set; }
        public decimal? CreateBy { get; set; }
        public DateTime? CreateDate { get; set; }
        public decimal? UpdateBy { get; set; }
        public DateTime? UpdateDate { get; set; }
        public string PAdmin { get; set; }
        public string PAccount { get; set; }
        public string PPatient { get; set; }
        public string PRequisitionConfirm { get; set; }
        public string PPhotoComment { get; set; }
        public DateTime? LastLoginDate { get; set; }
        public string PClinic { get; set; }
        public string PCodeType { get; set; }
        public string PEnquiry { get; set; }
        public string PEnquiryResult { get; set; }
        public string Guid { get; set; }
        public decimal? RoleId { get; set; }
        public decimal? TypeId { get; set; }
        public string FarmGuid { get; set; }
        public string AccountRole { get; set; }
        public string AccountType { get; set; }
        public string AccountGroup { get; set; }
        public decimal? AccountOrganization { get; set; }
        public decimal? AccountSite { get; set; }
        public decimal? ErrorLogin { get; set; }
        public string PhotoPath { get; set; }
        public string AccountDomicileAddress { get; set; }
        public string EmployeeGuid { get; set; }
        public string PageSizeSetting { get; set; }
        
    }
}
