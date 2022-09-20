using System;
using System.Collections.Generic;

#nullable disable

namespace IRS.DTO
{
    public partial class WorkPlanNewDto
    {
        public int Id { get; set; }
        public string Guid { get; set; }
        public string Line { get; set; }
        public string Line2 { get; set; }
        public string Pono { get; set; }
        public string ModelName { get; set; }
        public string ModelNo { get; set; }
        public string ArticleNo { get; set; }
        public string Qty { get; set; }
        public string Treatment { get; set; }
        public string Stitching { get; set; }
        public string Stockfitting { get; set; }
        public int? ScheduleId { get; set; }
        public bool? Status { get; set; }
        public string ShoeGuid { get; set; }
        public DateTime? CreateDate { get; set; }
        public decimal? CreateBy { get; set; }
        public DateTime? UpdateDate { get; set; }
        public decimal? UpdateBy { get; set; }
        public DateTime? DeleteDate { get; set; }
        public decimal? DeleteBy { get; set; }
    }
}
