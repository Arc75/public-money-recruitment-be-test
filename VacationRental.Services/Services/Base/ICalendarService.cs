using System;
using VacationRental.Services.Models;

namespace VacationRental.Services.Services.Base
{
    public interface ICalendarService
    {
        CalendarViewModel GetCalendar(int rentalId, DateTime start, int nights);
    }
}
