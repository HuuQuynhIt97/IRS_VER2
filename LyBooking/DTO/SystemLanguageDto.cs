using IRS.Models.Abstracts;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace IRS.DTO
{
    public class SystemLanguageDto
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

        public DateTime? CreateDate { get; set; }
        public decimal? CreateBy { get; set; }
        public DateTime? UpdateDate { get; set; }
        public decimal? UpdateBy { get; set; }
        public decimal? Status { get; set; }
    }
}
