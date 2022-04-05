using System;
using System.Collections.Generic;
using System.Linq;
using VacationRental.Services.Models;
using VacationRental.Services.Services.Base;

namespace VacationRental.Services
{
    public class CalendarService : ICalendarService
    {
        private readonly IDictionary<int, RentalViewModel> _rentals;
        private readonly IDictionary<int, BookingViewModel> _bookings;

        public CalendarService(IDictionary<int, RentalViewModel> rentals,
            IDictionary<int, BookingViewModel> bookings)
        {
            _rentals = rentals;
            _bookings = bookings;
        }

        public CalendarViewModel GetCalendar(int rentalId, DateTime start, int nights)
        {
            if (nights < 0)
                throw new ApplicationException("Nights must be positive");

            if (!_rentals.ContainsKey(rentalId))
                throw new ApplicationException("Rental not found");

            var result = new CalendarViewModel
            {
                RentalId = rentalId,
                Dates = new List<CalendarDateViewModel>()
            };

            var bookings = _bookings.Values.Where(x=> x.Id == rentalId);
            for (var i = 0; i < nights; i++)
            {
                var date = new CalendarDateViewModel
                {
                    Date = start.Date.AddDays(i),
                    Bookings = new List<CalendarBookingViewModel>(),
                    PreparationTimes = new List<UnitViewModel>()
                };

                foreach (var booking in bookings)
                {
                    var end = booking.Start.AddDays(booking.Nights);

                    if (booking.Start <= date.Date && end > date.Date)
                    {
                        date.Bookings.Add(new CalendarBookingViewModel { Id = booking.Id, Unit = booking.Unit });
                    }

                    if (end <= date.Date && end.AddDays(_rentals[rentalId].PreparationTimeInDays) > date.Date)
                    {
                        date.PreparationTimes.Add(new UnitViewModel { Unit = booking.Unit });
                    }
                }

                result.Dates.Add(date);
            }

            return result;
        }
    }
}
