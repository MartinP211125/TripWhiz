using Core.DTOs.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Interfaces;
using System.Security.Claims;

namespace TripApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class BookingController : ControllerBase
    {
        private readonly IBookingService bookingService;
        private readonly IUserService userService;

        public BookingController(IBookingService bookingService, IUserService userService)
        {
            this.bookingService = bookingService;
            this.userService = userService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateBooking([FromBody] BookingDto bookingDto)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId)) return Unauthorized();

            var result = await bookingService.CreateBookingForUserAsync(bookingDto, userId);
            return Ok(result);
        }

        [HttpGet("{bookingId}")]
        public async Task<IActionResult> GetBooking(Guid bookingId)
        {
            var booking = await bookingService.GetBookingForUserAsync(bookingId);
            return Ok(booking);
        }
        
        [HttpGet("my-bookings")]
        public async Task<IActionResult> GetUserBookings()
        {
            var email = User.FindFirst(ClaimTypes.Name)?.Value;

            if (string.IsNullOrEmpty(email)) return Unauthorized();

            var bookings = await bookingService.GetBookingsForUserByEmailAsync(email);
            return Ok(bookings);
        }

        [HttpPut("{bookingId}/cancel")]
        public async Task<IActionResult> CancelBooking(Guid bookingId)
        {
            var result = await bookingService.CancelBooking(bookingId);
            return Ok(result);
        }
    }
}
