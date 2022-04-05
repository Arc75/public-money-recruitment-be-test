using System;
using System.Collections.Generic;

namespace VacationRental.Services.Models
{
    public class CalendarDateViewModel
    {
        public DateTime Date { get; set; }

        public List<CalendarBookingViewModel> Bookings { get; set; }

        public List<UnitViewModel> PreparationTimes { get; set; }
    }
}
