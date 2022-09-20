using System;
using System.Collections.Generic;

#nullable disable

namespace IRS.DTO
{
    public partial class WorkOrderDto
    {
        public decimal Id { get; set; }
        public string HallGuid { get; set; }
        public string WorkOrderNo { get; set; }
        public string WorkOrderName { get; set; }
        public string Comment { get; set; }
        public DateTime? CreateDate { get; set; }
        public decimal? CreateBy { get; set; }
        public DateTime? UpdateDate { get; set; }
        public decimal? UpdateBy { get; set; }
        public DateTime? DeleteDate { get; set; }
        public decimal? DeleteBy { get; set; }
        public decimal? Status { get; set; }
        public string Guid { get; set; }
    }
}
