using VacationRental.Services.Models;

namespace VacationRental.Services.Services.Base
{
    public interface IBookingService
    {
        BookingViewModel GetBooking(int id);

        ResourceIdViewModel Book(BookingBindingModel model);

        bool CanReduceUnits(int id, int units, int oldUnits);

        void ChangePreparationDays(int id, int preparationTimeInDays);
    }
}
