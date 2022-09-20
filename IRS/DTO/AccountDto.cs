using Microsoft.AspNetCore.Http;
using IRS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IRS.DTO
{
    public class AccountDto
    {
        public int ID { get; set; }

        public string Name { get; set; }

        public string NO { get; set; }

        public string RFID { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }
        
        public string AccountRole { get; set; }

        //[Column(TypeName = "char(1)")]
        //public string AccountType { get; set; }

        //[Column(TypeName = "nvarchar(100)")]
        //public string AccountGroup { get; set; }

        public string LicensePath { get; set; }

        public string PhotoPath { get; set; }

        public string Comment { get; set; }

        public DateTime? StartDate { get; set; } // user account start date for ACTIVE 
        public DateTime? EndDate { get; set; } // user account end date for ACTIVE 
        public DateTime? LastLogin { get; set; } // record for user last login datetime
        public DateTime? LastUse { get; set; } // record for user last LOGOUT datetime

        public int PigAppLoginErrorTimes { get; set; }
        public int EquipmentAppLoginErrorTimes { get; set; }
        public int WebLoginErrorTimes { get; set; }

        public DateTime? PigAppLockedAt { get; set; }
        public DateTime? EquipmentAppLockedAt { get; set; }

        public int? AccountTypeID { get; set; }
        public int? AccountGroupID { get; set; }
        public int? OCID { get; set; }
        public List<int> OCIDList { get; set; }
        public List<int> list { get; set; }
        public string Farms { get; set; }
        public string NickName { get; set; }
        public string AccountGroupName { get; set; }

        public int? EmployeeID { get; set; }

        public List<IFormFile> File { get; set; }
        public string Guid { get; set; }
    }
    public class UploadAvatarRequest
    {
        public IFormFile File { get; set; }
        public decimal Key { get; set; }

    }
    public class ChangePasswordDto
    {
        public decimal ID { get; set; }
        public string Upwd { get; set; }
    }
    public class XChangePasswordDto
    {
        public decimal ID { get; set; }
        public string Upwd { get; set; }
        public string OldPassword { get; set; }
    }
    public class AccountLoginDto
    {
        public decimal ID { get; set; }

        public string Username { get; set; }
        public string Password { get; set; }

        public string FullName { get; set; }

        public string GroupCode { get; set; }

    }
}
