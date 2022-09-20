using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;

#nullable disable

namespace IRS.DTO
{
    public partial class SiteDto
    {
        public decimal Id { get; set; }
        public string Type { get; set; }
        public string SiteNo { get; set; }
        public string SiteName { get; set; }
        public string SiteLocation { get; set; }
        public string SitePhoto { get; set; }
        public string Comment { get; set; }
        public DateTime? CreateDate { get; set; }
        public decimal? CreateBy { get; set; }
        public DateTime? UpdateDate { get; set; }
        public decimal? UpdateBy { get; set; }
        public DateTime? DeleteDate { get; set; }
        public decimal? DeleteBy { get; set; }
        public decimal? Status { get; set; }
        public string Guid { get; set; }
        public List<IFormFile> File { get; set; }
    }
}
