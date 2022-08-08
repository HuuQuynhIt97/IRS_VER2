
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace IRS.DTO
{
    public class ReportConfigDto
    {

        public int Id { get; set; }
        public string Slpage { get; set; }
        public string Sltype { get; set; }
        public string Slkey { get; set; }
        public string Comment { get; set; }
        public string Sltw { get; set; }
        public string Slen { get; set; }
        public string Slcn { get; set; }
        public string Slvn { get; set; }
        public string SystemMenuGuid { get; set; }
        public decimal? Sequence { get; set; }
        public decimal? ColumnWidth { get; set; }
        public string TextAlign { get; set; }


    }
    public class ExcelExportPieChartDto
    {

        public string PrintBy { get; set; }
        public string FunctionName { get; set; }
        public List<string> ImageBase64 { get; set; }

    }
    public class ExcelExportChartDto
    {

        public string PrintBy { get; set; }
        public string FunctionName { get; set; }
        public string ImageBase64 { get; set; }

    }
    public class ExcelExportFileDto
    {

        public string PrintBy { get; set; }
        public string FunctionName { get; set; }
        public IFormFile File { get; set; }

    }
}
