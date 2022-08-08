using System;
using System.Collections.Generic;

#nullable disable

namespace IRS.Models
{
    public partial class Customer
    {
        public decimal Id { get; set; }
        public string CustomerNo { get; set; }
        public string CustomerName { get; set; }
        public string CustomerSex { get; set; }
        public DateTime? CustomerBirthday { get; set; }
        public string CustomerNickname { get; set; }
        public string CustomerTel { get; set; }
        public string CustomerMobile { get; set; }
        public string CustomerAddress { get; set; }
        public string CustomerIdcard { get; set; }
        public string CustomerEmail { get; set; }
        public string ContactName { get; set; }
        public string ContactTel { get; set; }
        public string ContactMobile { get; set; }
        public string ContactEmail { get; set; }
        public string Comment { get; set; }
        public string CancelFlag { get; set; }
        public DateTime? CreateDate { get; set; }
        public decimal? CreateBy { get; set; }
        public DateTime? UpdateDate { get; set; }
        public decimal? UpdateBy { get; set; }
        public decimal? Status { get; set; }
        public string Guid { get; set; }
        public string FarmGuid { get; set; }
        public string HallGuid { get; set; }
    }
}
