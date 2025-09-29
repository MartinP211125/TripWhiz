
using Core.Entities.Entities;

namespace Core.Interfaces.Repositories
{
    public interface IBookingRepository
    {
        Task<ICollection<Booking>> GetBookingsForUserByEmailAsync(string email);
        Task<Booking> GetBookingForUserAsync(Guid bookingId);
        Task<Booking> CreateBookingForUserAsync(Booking booking);
        Task<Booking> UpdateBookingForUserAsync(Guid bookingId, Booking booking);
        Task CreateBookingRoomAsync (BookingRoom room);
        Task CreateBookingSeatAsync (BookingSeat seat); 
    }
}
