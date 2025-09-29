
using BookingDtoResponse = Core.DTOs.Response.BookingDto;
using BookingDtoRequest = Core.DTOs.Request.BookingDto;

namespace Service.Interfaces
{
    public interface IBookingService
    {
        Task<ICollection<BookingDtoResponse>> GetBookingsForUserByEmailAsync(string email);
        Task<BookingDtoResponse> GetBookingForUserAsync(Guid bookingId);
        Task<BookingDtoResponse> CreateBookingForUserAsync(BookingDtoRequest booking, string userId);
        Task<BookingDtoResponse> CancelBooking(Guid bookingId);
    }
}
