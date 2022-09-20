using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IRS.DTO
{
    public class RoleDto
    {
        public int ID { get; set; }
        public string Code { get; set; }
        public string VN { get; set; }
        public string EN { get; set; }
        public string CN { get; set; }
        public string TW { get; set; }
    }
    public class ScreenFunctionAndActionRequest
    {
        public List<int> RoleIDs { get; set; }
        public string  Lang{ get; set; }
    }
}
