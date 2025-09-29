
using Core.Entities.Base;

namespace Core.Entities.Entities
{
    public class Seat : BaseEntity
    {
        public Guid TransportationId { get; set; }
        public virtual Transportation Transportation { get; set; }
        public virtual ICollection<TransportationAvailability> TransportationAvailabilities { get; set; }
    }
}
