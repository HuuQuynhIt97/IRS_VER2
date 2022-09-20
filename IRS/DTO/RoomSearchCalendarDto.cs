using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;

#nullable disable

namespace IRS.DTO
{
    public class RoomSearchCalendarDto
    {
        public decimal RoomID { get; set; }
        public decimal BookingID { get; set; }
        public string RoomGuid { get; set; }
        public string HallGuid { get; set; }
        public string RoomName { get; set; }
        public string RoomDescript { get; set; }
        public string HallTime { get; set; }
        public string RoomNo { get; set; }
        public bool IsBooking { get; set; }
    }

    public class RoomSearchByList
    {
        public decimal RoomID { get; set; }
        public decimal BookingID { get; set; }
        public string RoomGuid { get; set; }
        public string HallGuid { get; set; }
        public string RoomName { get; set; }
        public string HallName { get; set; }
        public string RoomDescript { get; set; }
        public string HallTime { get; set; }
        public string RoomNo { get; set; }
        public string HallStart { get; set; }
        public string HallEnd { get; set; }
        public string Time { get; set; }
        public bool IsBooking { get; set; }
    }
}
