
using Core.Entities.Entities;
using Core.Interfaces.Repositories;
using Infrastructure.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services.Repositories
{
    public class BookingRepository : IBookingRepository
    {

        private readonly TripWhizDbContext _context;
        private readonly IUserRepository userRepository;

        public BookingRepository(TripWhizDbContext context, IUserRepository userRepository)
        {
            _context = context;
            this.userRepository = userRepository;
        }
        public async Task<Booking> UpdateBookingForUserAsync(Guid bookingId, Booking booking)
        {
            Booking existingBooking = await GetBookingForUserAsync(bookingId);

            _context.Bookings.Update(booking);
            await _context.SaveChangesAsync();
            return booking;
        }

        public async Task<Booking> GetBookingForUserAsync(Guid bookingId)
        {
            return await _context.Bookings.Include(x => x.BookingRooms).Include(x => x.BookingSeats).FirstOrDefaultAsync(x => x.Id == bookingId);
        }

        public async Task<ICollection<Booking>> GetBookingsForUserByEmailAsync(string email)
        {
            TripWhizUser user = await userRepository.GetUserByEmailAsync(email);
            return await _context.Bookings.Where(x => x.UserId == user.Id).ToListAsync();
        }
        public async Task<Booking> CreateBookingForUserAsync(Booking booking)
        {
            _context.Bookings.Add(booking);
            await _context.SaveChangesAsync();
            return booking;
        }

        public async Task CreateBookingRoomAsync(BookingRoom room)
        {
            _context.BookingRooms.Add(room);
            await _context.SaveChangesAsync();
        }

        public async Task CreateBookingSeatAsync(BookingSeat seat)
        {
            _context.BookingSeats.Add(seat);
            await _context.SaveChangesAsync();
        }
    }
}
