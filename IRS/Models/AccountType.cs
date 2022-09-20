using System;
using System.Collections.Generic;

#nullable disable

namespace IRS.Models
{
    public partial class AccountType
    {
        public AccountType()
        {
            Accounts = new HashSet<Account>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }

        public virtual ICollection<Account> Accounts { get; set; }
    }
}
