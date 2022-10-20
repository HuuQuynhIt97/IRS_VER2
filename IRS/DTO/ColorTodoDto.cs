using System;
using System.Collections.Generic;

#nullable disable

namespace IRS.DTO
{
    public partial class ColorTodoDto
    {
        public int Id { get; set; }
        public string Guid { get; set; }
        public string ColorWorkPlanGuid { get; set; }
        public DateTime? ExecuteDate { get; set; }
        public decimal? ExecuteAmount { get; set; }
        public string SchedulesGuid { get; set; }
        public int? IsFinished { get; set; }
        public DateTime? FinishedTime { get; set; }
        public int? IsPending { get; set; }
        public decimal? FinishedBy { get; set; }
        public string ColorName { get; set; }
    }
}
