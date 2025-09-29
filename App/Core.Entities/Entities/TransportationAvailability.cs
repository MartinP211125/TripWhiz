
using Core.Entities.Base;

namespace Core.Entities.Entities
{
    public class TransportationAvailability : BaseDateEntity
    {
        public Guid SeatId { get; set; }
        public Guid TransportationId { get; set; }
        public virtual Seat Seat { get; set; }
    }
}
