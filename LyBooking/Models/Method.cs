using System;
using System.Collections.Generic;

#nullable disable

namespace IRS.Models
{
    public partial class Method
    {
        public Method()
        {
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public bool? Status { get; set; }
        public int CreateBy { get; set; }
        public int? UpdateBy { get; set; }
        public int? DeleteBy { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public DateTime? DeleteDate { get; set; }

    }
}
