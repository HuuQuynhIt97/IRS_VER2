using Microsoft.AspNetCore.Http;
using IRS.Models.Interface;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IRS.DTO
{
    public class OCDto 
    {
        [Key]
        public int ID { get; set; }
        
        public string Type { get; set; }
        public string NO { get; set; }

        public string Name { get; set; }
     
        public string Principal { get; set; }
        public int Length { get; set; }
        public int Level { get; set; }
        public int Width { get; set; }
        public int GGP { get; set; }
        public int GP { get; set; }
        public int PMPF { get; set; }
        public int Semen { get; set; }
        public int Nursery { get; set; }
        public int Grower { get; set; }
        public string Comment { get; set; }
        public int? ParentID { get; set; }
        public string Longtitude { get; set; }
        public string Latitude { get; set; }

        public string TemptureTopLimit { get; set; }
        public string TemptureLowLimit { get; set; }
        public bool? Status { get; set; }

        public int CreateBy { get; set; }
        public int? UpdateBy { get; set; }
        public int? DeleteBy { get; set; }
        public string PhotoPath { get; set; }

        public DateTime CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public DateTime? DeleteDate { get; set; }

        public List<IFormFile> File { get; set; }
    }
}
