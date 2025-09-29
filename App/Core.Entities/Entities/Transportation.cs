
using Core.Entities.Base;
using Core.Enums;

namespace Core.Entities.Entities
{
    public class Transportation : BaseEntity
    {
        public TransportationType TransportationType { get; set; }
        public float Price { get; set; }
        public virtual Offer? Offer { get; set; }
        public Guid? OfferId { get; set; }
        public int NumberOfSeats { get; set; }
        public virtual ICollection<Seat> Seats { get; set; }
    }
}
