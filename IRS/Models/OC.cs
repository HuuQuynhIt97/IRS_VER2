using System;
using System.Collections.Generic;

#nullable disable

namespace IRS.Models
{
    public partial class Oc
    {
        public Oc()
        {
            Accounts = new HashSet<Account>();
        }

        public int Id { get; set; }
        public string Type { get; set; }
        public string Name { get; set; }
        public string Principal { get; set; }
        public int Length { get; set; }
        public int Width { get; set; }
        public int Ggp { get; set; }
        public int Gp { get; set; }
        public int Pmpf { get; set; }
        public int Semen { get; set; }
        public int Nursery { get; set; }
        public int Grower { get; set; }
        public string Comment { get; set; }
        public int? ParentId { get; set; }
        public bool? Status { get; set; }
        public int CreateBy { get; set; }
        public int? UpdateBy { get; set; }
        public int? DeleteBy { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public DateTime? DeleteDate { get; set; }
        public string No { get; set; }
        public int Level { get; set; }

        public virtual ICollection<Account> Accounts { get; set; }
    }
}
