using IRS.Models.Abstracts;
using System;
using System.ComponentModel.DataAnnotations;

namespace IRS.DTO
{
    public class EmployeeDto 
    {
        public decimal Id { get; set; }
        public string Name { get; set; }
        public string No { get; set; }
        public decimal? Sex { get; set; }
        public string NickName { get; set; }
        public string Tel { get; set; }
        public string Mobile { get; set; }
        public string Address { get; set; }
        public string AddressDomicile { get; set; }
        public string Idcard { get; set; }
        public string Email { get; set; }
        public string Comment { get; set; }
        public string Unit { get; set; }
        public string Dept { get; set; }
        public string Level { get; set; }
        public string ContactName { get; set; }
        public string ContactTel { get; set; }
        public string Guid { get; set; }
        public DateTime? BirthDay { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public decimal? Status { get; set; }
        public decimal? CreateBy { get; set; }
        public decimal? UpdateBy { get; set; }
        public decimal? DeleteBy { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public DateTime? DeleteDate { get; set; }
        public string SexName { get; set; }
        public string UnitName { get; set; }
        public string DeptName { get; set; }
        public string FarmGuid { get; set; }

    }
}
