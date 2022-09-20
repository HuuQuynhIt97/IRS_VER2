using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IRS.Models.Interface
{
   public interface IAuditEntity
    {

        public decimal? CreateBy { get; set; }
        public DateTime? CreateDate { get; set; }
        public decimal? UpdateBy { get; set; }
        public DateTime? UpdateDate { get; set; }


    }
}
