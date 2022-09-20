using System;
using System.Collections.Generic;

#nullable disable

namespace IRS.Models
{
    public partial class EmailPool
    {
        public decimal Id { get; set; }
        public string FromEmail { get; set; }
        public string FromName { get; set; }
        public string Subject { get; set; }
        public string Content { get; set; }
        public string Sendto { get; set; }
        public string Sendtocc { get; set; }
        public string Sendtobcc { get; set; }
        public DateTime? Senddate { get; set; }
        public string Status { get; set; }
        public DateTime? CreateDate { get; set; }
        public decimal? CreateBy { get; set; }
        public decimal? SendBy { get; set; }
        public DateTime? SendDate1 { get; set; }
    }
}
