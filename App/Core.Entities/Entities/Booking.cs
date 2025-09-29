
using Core.Entities.Base;
using Core.Enums;

namespace Core.Entities.Entities
{
    public class Booking: BaseEntity
    {
        public string UserId { get; set; }
        public virtual TripWhizUser User { get; set; }
        public Guid OfferId { get; set; }
        public virtual Offer Offer { get; set; }
        public Status Status { get; set; }
        public int NumberOfPeople { get; set; }
        public float TotalPrice { get; set; }
        public virtual ICollection<BookingRoom> BookingRooms { get; set; }
        public virtual ICollection<BookingSeat> BookingSeats { get; set; }
    }
}
