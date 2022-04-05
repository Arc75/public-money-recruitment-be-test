using VacationRental.Services.Models;

namespace VacationRental.Services.Services.Base
{
    public interface IRentalService
    {
        RentalViewModel GetRental(int id);

        ResourceIdViewModel PostRental(RentalBindingModel model);

        ResourceIdViewModel PutRental(RentalBindingModel model);
    }
}
