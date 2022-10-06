using System;
using System.Collections.Generic;

#nullable disable

namespace IRS.DTO
{
    public partial class ColorWorkPlanDto
    {
        public int Id { get; set; }
        public string Guid { get; set; }
        public string ShoeGuid { get; set; }
        public string ShoeName { get; set; }
        public DateTime? CreateDate { get; set; }
        public string CreateBy { get; set; }
        public DateTime? ExecuteDate { get; set; }
        public string Execute { get; set; }
    }
}
