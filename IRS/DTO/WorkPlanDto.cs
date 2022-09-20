using System;
using System.Collections.Generic;

#nullable disable

namespace IRS.DTO
{
    public partial class WorkPlanDto
    {
        public int Id { get; set; }
        public string Line { get; set; }
        public string Pono { get; set; }
        public string ModelName { get; set; }
        public string ModelNo { get; set; }
        public string ArticleNo { get; set; }
        public string Qty { get; set; }
        public string Treatment { get; set; }
        public string Stitching { get; set; }
        public string Stockfitting { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime CreatedTime { get; set; }
        public int? ScheduleId { get; set; }
        public bool? Status { get; set; }
    }
}
