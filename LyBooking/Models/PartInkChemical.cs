﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace IRS.Models
{
    public partial class PartInkChemical
    {
        public int Id { get; set; }
        public int PartId { get; set; }
        public int InkId { get; set; }
        public int ChemicalId { get; set; }
        public double Percentage { get; set; }
        public DateTime CreatedDate { get; set; }
        public int GlueId { get; set; }
        public double? Consumption { get; set; }
    }
}