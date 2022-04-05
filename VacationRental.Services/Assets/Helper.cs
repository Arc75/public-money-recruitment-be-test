using System;
using System.Collections.Generic;
using System.Linq;
using VacationRental.Services.Models;

namespace VacationRental.Services.Assets
{
    internal static class Helper
    {
        internal static bool IsUnitOccupied(BookingBindingModel newBooking, BookingViewModel exBooking, DateTime newEnd, DateTime exEnd)
        {
            return (exBooking.Start <= newBooking.Start && exEnd > newBooking.Start.Date) ||
                   (exBooking.Start < newEnd && exEnd >= newEnd) ||
                   (exBooking.Start > newBooking.Start && exEnd < newEnd);
        }

        internal static int GetUnit(IDictionary<int, BookingViewModel> bookings, RentalViewModel rental, BookingBindingModel newBooking)
        {
            var existingBookings = bookings.Values.Where(x => x.RentalId == newBooking.RentalId);

            var newEnd = newBooking.Start.AddDays(newBooking.Nights + rental.PreparationTimeInDays);

            var units = existingBookings.Where(x => IsUnitOccupied(newBooking, x, newEnd, x.Start.AddDays(newBooking.Nights + rental.PreparationTimeInDays))).Select(x => x.Unit);

            if (units.Count() >= rental.Units)
                throw new ApplicationException("Not available");

            for (var i = 1; i <= rental.Units; i++)
                if (!units.Contains(i))
                    return i;

            throw new ApplicationException("Not available");
        }
    }
}
