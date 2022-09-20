using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IRS.DTO.auth
{
    public class UserForLoginDto
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
    public class UserForLoginRememberDto
    {
        public decimal ID { get; set; }
    }
}
