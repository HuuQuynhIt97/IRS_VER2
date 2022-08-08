using IRS.Models.Abstracts;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace IRS.Models
{
    public class ReportConfig
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
}
