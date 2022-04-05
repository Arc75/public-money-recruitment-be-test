using Microsoft.AspNetCore.Mvc;
using VacationRental.Services.Models;
using VacationRental.Services.Services.Base;

namespace VacationRental.Api.Controllers
{
    [Route("api/v1/rentals")]
    [ApiController]
    public class RentalsController : ControllerBase
    {
        private readonly IRentalService _rentalService;

        public RentalsController(IRentalService service)
        {
            _rentalService = service;
        }

        [HttpGet]
        [Route("{rentalId:int}")]
        public RentalViewModel Get(int rentalId)
        {
            return _rentalService.GetRental(rentalId);
        }

        [HttpPost]
        public ResourceIdViewModel Post(RentalBindingModel model)
        {
            return _rentalService.PostRental(model);
        }

        [HttpPut]
        public ResourceIdViewModel Put(RentalBindingModel model)
        {
            return _rentalService.PutRental(model);
        }
    }
}
