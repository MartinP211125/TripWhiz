
using Core.Entities.Base;

namespace Core.Entities.Entities
{
    public class BookingRoom : BaseEntity
    {
        public Guid BookingId { get; set; }
        public virtual Booking Booking { get; set; }
        public Guid AccomodationAvailabilityId { get; set; }
    }
}
