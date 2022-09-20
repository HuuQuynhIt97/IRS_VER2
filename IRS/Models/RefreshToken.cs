using System;
using System.Collections.Generic;

#nullable disable

namespace IRS.Models
{
    public partial class RefreshToken
    {
        public string Token { get; set; }
        public string JwtId { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime ExpiryDate { get; set; }
        public bool Used { get; set; }
        public bool Invalidated { get; set; }
        public int AccountId { get; set; }

       // public virtual Account Account { get; set; }
    }
}
