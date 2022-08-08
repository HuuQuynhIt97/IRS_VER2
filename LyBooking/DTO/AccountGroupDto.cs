using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IRS.DTO
{
    public class AccountGroupDto
    {
        public int ID { get; set; }
        public int ZoneID { get; set; }
        public int BuildingID { get; set; }
        public string GroupNO { get; set; }
        public string GroupName { get; set; }
        public bool? Status { get; set; }

        public int CreateBy { get; set; }
        public int? UpdateBy { get; set; }
        public int? DeleteBy { get; set; }

        public DateTime CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public DateTime? DeleteDate { get; set; }
    }
}
