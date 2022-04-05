using System;
using System.Collections.Generic;
using VacationRental.Services.Models;
using VacationRental.Services.Services.Base;

namespace VacationRental.Services
{
    public class RentalService : IRentalService
    {
        private readonly IDictionary<int, RentalViewModel> _rentals;
        private readonly IBookingService _bookingService;

        public RentalService(IDictionary<int, RentalViewModel> rentals, IBookingService service)
        {
            _rentals = rentals;
            _bookingService = service;
        }

        public RentalViewModel GetRental(int id)
        {
            if (!_rentals.ContainsKey(id))
                throw new ApplicationException("Rental not found");

            return _rentals[id];
        }

        public ResourceIdViewModel PostRental(RentalBindingModel model)
        {
            var key = new ResourceIdViewModel { Id = _rentals.Keys.Count + 1 };

            _rentals.Add(key.Id, new RentalViewModel
            {
                Id = key.Id,
                Units = model.Units,
                PreparationTimeInDays = model.PreparationTimeInDays
            });

            return key;
        }

        public ResourceIdViewModel PutRental(RentalBindingModel model)
        {
            if (!model.Id.HasValue || !_rentals.ContainsKey(model.Id.Value))
                return PostRental(model);

            var existingRental = _rentals[model.Id.Value];

            if (model.Units < existingRental.Units && !_bookingService.CanReduceUnits(existingRental.Id, model.Units, existingRental.Units))
                throw new ApplicationException("Existing bookings prevents changes");

            if (model.PreparationTimeInDays > existingRental.PreparationTimeInDays)
                _bookingService.ChangePreparationDays(existingRental.Id, model.PreparationTimeInDays);

            existingRental.PreparationTimeInDays = model.PreparationTimeInDays;
            existingRental.Units = model.Units;

            return new ResourceIdViewModel { Id = existingRental.Id };
        }
    }
}
