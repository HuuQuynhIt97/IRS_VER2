﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace IRS.Models
{
    public partial class PeriodDispatch
    {
        public int Id { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public bool IsDelete { get; set; }
        public int CreatedBy { get; set; }
        public int DeletedBy { get; set; }
        public int UpdatedBy { get; set; }
        public DateTime CreatedTime { get; set; }
        public DateTime? DeletedTime { get; set; }
        public DateTime? UpdatedTime { get; set; }
        public int PeriodMixingId { get; set; }
    }
}