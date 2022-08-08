using System;
using System.Collections.Generic;

#nullable disable

namespace IRS.Models
{
    public partial class AccountRole
    {
        public int Id { get; set; }
        public string AccountGuid { get; set; }
        public int? AccountId { get; set; }
        public string CodeNo { get; set; }

        public virtual Account Account { get; set; }
    }
}
