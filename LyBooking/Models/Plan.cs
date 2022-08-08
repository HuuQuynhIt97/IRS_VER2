using IRS.Models.Abstracts;
using IRS.Models.Interface;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IRS.Models
{

    [Table("Plans")]
    public class Plan : AuditEntity
    {
        [Key]
        public int ID { get; set; }
     
      
      
    }
}
