using System;

namespace VacationRental.Services.Models
{
    public class RentalBindingModel
    {
        public int? Id { get; set; }

        public int Units { get; set; }

        public int PreparationTimeInDays { get; set; }
    }
}
