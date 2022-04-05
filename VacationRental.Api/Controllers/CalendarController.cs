using Microsoft.AspNetCore.Mvc;
using System;
using VacationRental.Services.Models;
using VacationRental.Services.Services.Base;

namespace VacationRental.Api.Controllers
{
    [Route("api/v1/calendar")]
    [ApiController]
    public class CalendarController : ControllerBase
    {
        private readonly ICalendarService _calendarService;

        public CalendarController(ICalendarService service)
        {
            _calendarService = service;
        }

        [HttpGet]
        public CalendarViewModel Get(int rentalId, DateTime start, int nights)
        {
            return _calendarService.GetCalendar(rentalId, start, nights);
        }
    }
}
