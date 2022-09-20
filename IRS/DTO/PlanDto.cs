using System;
namespace IRS.DTO
{

    public class PlanDto
    {

        public int ID { get; set; }
   
        public string Topic { get; set; }
        public bool Status { get; set; }
        public int AccountID { get; set; }
        public int CreatedBy { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime CreatedTime { get; set; }
        public DateTime? ModifiedTime { get; set; }
       
    }
}
