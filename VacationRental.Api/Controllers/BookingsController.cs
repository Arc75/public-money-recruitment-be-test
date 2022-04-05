using Microsoft.AspNetCore.Mvc;
using VacationRental.Services.Models;
using VacationRental.Services.Services.Base;

namespace VacationRental.Api.Controllers
{
    [Route("api/v1/bookings")]
    [ApiController]
    public class BookingsController : ControllerBase
    {
        private readonly IBookingService _bookingService;

        public BookingsController(IBookingService service)
        {
            _bookingService = service;
        }

        [HttpGet]
        [Route("{bookingId:int}")]
        public BookingViewModel Get(int bookingId)
        {
            return _bookingService.GetBooking(bookingId);
        }

        [HttpPost]
        public ResourceIdViewModel Post(BookingBindingModel model)
        {
            return _bookingService.Book(model);
        }
    }
}
