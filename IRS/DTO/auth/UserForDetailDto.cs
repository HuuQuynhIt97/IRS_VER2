using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IRS.DTO.auth
{
    public class UserForDetailDto
    {
        public object ID { get; set; }
        public string Guid { get; set; }
        public string Username { get; set; }
        public string AccountName { get; set; }
        public string NickName { get; set; }
        public string Email { get; set; }
        public string GroupCode { get; set; }
        public object GroupID { get; set; }
        public object PageSizeSetting { get; set; }
        public object PageSizeSettingValue { get; set; }
        public object PageSizeSettingList { get; set; }
    }
}
