using Core.Entities.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
namespace Infrastructure.Data.Context
{
    public class TripWhizDbContext : IdentityDbContext<TripWhizUser>
    {
        public TripWhizDbContext(DbContextOptions<TripWhizDbContext> options)
        : base(options)
        {
        }

        public DbSet<Offer> Offers { get; set; }
        public DbSet<AccomodationAvailability> AccomodationAvailabilities { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<Place> Places { get; set; }
        public DbSet<Transportation> Transportations { get; set; }
        public DbSet<TripWhizUser> TripWizUsers { get; set; }
        public DbSet<UserActivity> UserActivities { get; set; }
        public DbSet<Accomodation> Accomodations { get; set; }
        public DbSet<TransportationAvailability> TransportationsAvailabilities { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<Seat> Seats { get; set; }
        public DbSet<BookingRoom> BookingRooms { get; set; }
        public DbSet<BookingSeat> BookingSeats { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
