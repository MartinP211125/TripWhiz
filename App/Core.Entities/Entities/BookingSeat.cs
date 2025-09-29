using Core.Entities.Base;

namespace Core.Entities.Entities
{
    public class BookingSeat : BaseEntity
    {
        public Guid BookingId { get; set; }
        public virtual Booking Booking { get; set; }
        public Guid TransportationAvailabilityId { get; set; }
    }
}
