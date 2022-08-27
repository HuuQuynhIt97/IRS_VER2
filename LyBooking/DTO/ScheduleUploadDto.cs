using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;

#nullable disable

namespace IRS.DTO
{
    public partial class ScheduleUploadDto
    {
        public int ScheduleId { get; set; }
        public string Guid { get; set; }
        public string ModelName { get; set; }
        public string ModelNo { get; set; }
        public string ArticleNo { get; set; }
        public string Treatment { get; set; }
        public string TreatmentGuid { get; set; }
        public string Process { get; set; }
        public string ProcessGuid { get; set; }
        public int GlueID { get; set; }
        public string PartID { get; set; }
        public string Name { get; set; }
        public string Consumption { get; set; }
        public int TreatmentWayID { get; set; }
        public string TreatmentWayGuid { get; set; }
        public DateTime? CreateDate { get; set; }
        public decimal? Status { get; set; }
        public List<ScheduleDto> Schedule { get; set; }
        // public List<string> Schedule { get; set; }
    }
}
