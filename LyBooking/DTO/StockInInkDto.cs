﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace IRS.DTO
{
    public partial class StockInInkDto
    {
        public decimal Id { get; set; }
        public string InkGuid { get; set; }
        public string InkName { get; set; }
        public string Guid { get; set; }
        public DateTime? CreateDate { get; set; }
        public decimal? CreateBy { get; set; }
        public DateTime? UpdateDate { get; set; }
        public decimal? UpdateBy { get; set; }
        public DateTime? DeleteDate { get; set; }
        public decimal? DeleteBy { get; set; }
        public decimal? Status { get; set; }
        public DateTime? ExecuteDate { get; set; }
        public DateTime? ApproveDate { get; set; }
        public decimal? ApproveBy { get; set; }
        public double? RealAmount { get; set; }
        public double? RemainingAmount { get; set; }

        public bool ApproveStatus { get; set; }
        public string ApproveByName { get; set; }
    }
}