using System;
namespace IRS.DTO
{

    public class BookingFilterDto
    {

        public string SiteGuid { get; set; }
        public string HallGuidFiter { get; set; }
        public string RoomGuidFiter { get; set; }
        public string FuneralDirectorFilter { get; set; }

    }
    public class BookingCheckFilterDto
    {

        public string SiteGuid { get; set; }
        public string SiteGuidFilter { get; set; }
        public string HallGuidFiter { get; set; }
        public string RoomGuidFiter { get; set; }
        public string FuneralDirectorFilter { get; set; }
        public DateTime? bookingDate { get; set; }

    }
}
