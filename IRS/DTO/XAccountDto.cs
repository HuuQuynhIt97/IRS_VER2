using Microsoft.AspNetCore.Http;
using IRS.Models.Interface;
using System;
using System.Collections.Generic;

#nullable disable

namespace IRS.DTO
{
    public partial class XAccountDto: IAuditEntity
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
        public string EmployeeGuid { get; set; }
        public string EmployeeNickName { get; set; }
        public string FarmName { get; set; }
        public string AccountGroupName { get; set; }
        public string AccountRole { get; set; }
        public string AccountType { get; set; }
        public string AccountGroup { get; set; }
        public decimal? AccountOrganization { get; set; }
        public decimal? AccountSite { get; set; }
        public decimal? ErrorLogin { get; set; }
        public string PhotoPath { get; set; }
        public string AccountDomicileAddress { get; set; }
        public List<IFormFile> File { get; set; }

    }
    public partial class StorePermissionDto
    {
        public string Guid { get; set; }
       
        public List<string> Permissions { get; set; }

    }

    public partial class ForgotUsernameDto
    {
        public string email { get; set; }
    }
    public partial class ResetPasswordDto
    {
        public string token { get; set; }
        public string NewPassword { get; set; }
    }
    public partial class StoreProfileDto
    {
        public string NickName { get; set; }
        public string AccountGuid { get; set; }
        public decimal AccountId { get; set; }
        public string Tel { get; set; }
        public string Mobile { get; set; }
        public string Address { get; set; }
        public string AddressDomicile { get; set; }
        public string Email { get; set; }
        public string Comment { get; set; }
        public string ContactName { get; set; }
        public string ContactTel { get; set; }
        public string PageSizeSetting { get; set; }
        public string PageSizeSettingValue { get; set; }
    }
}
