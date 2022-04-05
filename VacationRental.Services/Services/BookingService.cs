using System;
using System.Collections.Generic;
using System.Linq;
using VacationRental.Services.Assets;
using VacationRental.Services.Models;
using VacationRental.Services.Services.Base;

namespace VacationRental.Services
{
    public class BookingService : IBookingService
    {
        private readonly IDictionary<int, RentalViewModel> _rentals;
        private readonly IDictionary<int, BookingViewModel> _bookings;

        public BookingService(IDictionary<int, RentalViewModel> rentals,
            IDictionary<int, BookingViewModel> bookings)
        {
            _rentals = rentals;
            _bookings = bookings;
        }

        public ResourceIdViewModel Book(BookingBindingModel model)
        {
            if (model.Nights <= 0)
                throw new ApplicationException("Nights must be positive");

            if (!_rentals.ContainsKey(model.RentalId))
                throw new ApplicationException("Rental not found");

            var key = new ResourceIdViewModel { Id = _bookings.Keys.Count + 1 };

            _bookings.Add(key.Id, new BookingViewModel
            {
                Id = key.Id,
                Nights = model.Nights,
                RentalId = model.RentalId,
                Start = model.Start.Date,
                Unit = Helper.GetUnit(_bookings, _rentals[model.RentalId], model)
            });

            return key;
        }

        public BookingViewModel GetBooking(int id)
        {
            if (!_bookings.ContainsKey(id))
                throw new ApplicationException("Booking not found");

            return _bookings[id];
        }

        public bool CanReduceUnits(int rentalId, int units, int oldUnits)
        {
            var bookedUnits = _bookings.Values.Where(x => x.RentalId == rentalId).Select(x => x.Unit);

            for (var i = oldUnits; i >= units; i--)
            {
                if (bookedUnits.Contains(i))
                    return false;
            }

            return true;
        }

        public void ChangePreparationDays(int rentalId, int preparationTimeInDays)
        {
            var existingBookings = _bookings.Values.Where(x => x.RentalId == rentalId);

            foreach (var unitGroup in existingBookings.OrderBy(x => x.Start).GroupBy(x => x.Unit))
            {
                var list = unitGroup.ToList();

                for (var i = 0; i < list.Count - 1; i++)
                {
                    var currentBooking = list[i];
                    var nextBooking = list[i + 1];

                    if (currentBooking.Start.Date.AddDays(currentBooking.Nights + preparationTimeInDays) > nextBooking.Start)
                        throw new ApplicationException("Changing preparation time is prevented by existing bookings");
                }
            }
        }
    }
}
