﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace IRS.Models
{
    public partial class Gluese
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int PartId { get; set; }
        public int ScheduleId { get; set; }
        public DateTime CreatedDate { get; set; }
        public int TreatmentWayId { get; set; }
        public string Consumption { get; set; }
        public int? Sequence { get; set; }
    }
}