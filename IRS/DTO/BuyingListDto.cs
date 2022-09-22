using System;
using System.Collections.Generic;

#nullable disable

namespace IRS.DTO
{
    public partial class BuyingListDto
    {
        public int Id { get; set; }
        public string Guid { get; set; }
        public string ST_Team { get; set; }
        public string SF_Team { get; set; }
        public string Pono { get; set; }
        public string Batch { get; set; }
        public string ModelName { get; set; }
        public string ModelNo { get; set; }
        public string ArticleNo { get; set; }
        public string Qty { get; set; }
        public string CutStartDate { get; set; }
        public string SFStartDate { get; set; }
        public int? ScheduleId { get; set; }
        public bool? Status { get; set; }
        public string ShoeGuid { get; set; }
    }

    public partial class InkColorListDto
    {
        public string Guid { get; set; }
        public string ColorGuid { get; set; }
        public string NameColor { get; set; }
        public string InkGuid { get; set; }
        public string NameInk { get; set; }
        public string Percentage { get; set; }
        public string Code { get; set; }
        public double Consumption { get; set; }
    }

    public partial class InkListDto
    {
        public string Guid { get; set; }
        public string NameInk { get; set; }
        public string Code { get; set; }
        public string Consumption { get; set; }
    }

    public partial class ChemicalColorListDto
    {
        public string Guid { get; set; }
        public string ColorGuid { get; set; }
        public string NameColor { get; set; }
        public string ChemicalGuid { get; set; }
        public string NameChemical { get; set; }
        public string Percentage { get; set; }
        public string Code { get; set; }
        public double Consumption { get; set; }
    }

    public partial class ChemicalListDto
    {
        public string Guid { get; set; }
        public string NameChemical { get; set; }
        public string Code { get; set; }
        public string Consumption { get; set; }
    }
    
}
