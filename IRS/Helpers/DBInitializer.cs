using Microsoft.EntityFrameworkCore;
using IRS.Data;
using IRS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IRS.Helpers
{
    public static class DBInitializer
    {
        //private readonly DataContext _context;
        //public DBInitializer(DataContext context)
        //{
        //    _context = context;
        //}
        public static void Seed(PigFarmContext _context)
        {
         



            #region Loại Tài Khoản
            //if (!(_context.AccountTypes.Any()))
            //{
            //    _context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT dbo.AccountTypes ON");
            //    _context.AccountTypes.AddRange(new List<AccountType> {
            //        new AccountType(1, "System Management", "SYSTEM"),
            //        new AccountType(2, "Members", "MEMBER"),
            //    });
            //    _context.SaveChanges();
            //    _context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT dbo.AccountTypes OFF");
            //}

            #endregion

            #region Tài Khoản
            //if (!(_context.Accounts.Any()))
            //{
            //    var supper = _context.AccountTypes.FirstOrDefault(x => x.Code.Equals("SYSTEM"));
            //    var user = _context.AccountTypes.FirstOrDefault(x => x.Code.Equals("MEMBER"));
            //    var account1 = new Account { Username = "admin", Password = "1", AccountTypeID = supper.ID };
            //    var account2 = new Account { Username = "user", Password = "1", AccountTypeID = user.ID };
            //    _context.Accounts.AddRange(new List<Account> {account1,
            //       account2
            //    });
            //    _context.SaveChanges();
            //}

            #endregion

            #region Nhóm Tài Khoản
            //if (!(_context.OCs.Any()))
            //{
            //    _context.OCs.AddRange(new List<OC> {
            //        new OC { Name = "Farm"},
            //        new OC { Name = "Area" },
            //        new OC { Name = "Bam"},
            //        new OC { Name = "Room"},
            //        new OC { Name = "Pen"},
            //        new OC { Name = "Culling pen"}
            //});
            //    _context.SaveChanges();
            //}

            #endregion


        }
    }
}
