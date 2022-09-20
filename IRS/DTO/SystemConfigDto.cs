using System;

namespace IRS.DTO
{
    public partial class SystemConfigDto
    {
        public decimal Id { get; set; }
        public string Type { get; set; }
        public string Link { get; set; }
        public string Value { get; set; }
        public string No { get; set; }
        public decimal Sort { get; set; }
        public decimal WebBuildingId { get; set; }
        public string Comment { get; set; }
        public decimal? AccountId { get; set; }
        public decimal? Status { get; set; }
        public decimal? CreateBy { get; set; }
        public decimal? UpdateBy { get; set; }
        public decimal? DeleteBy { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public DateTime? DeleteDate { get; set; }

    }
}
