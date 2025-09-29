
using Core.Entities.Base;

namespace Core.Entities.Entities
{
    public class Room : BaseEntity
    {
        public Guid AccomodationId { get; set; }
        public virtual Accomodation Accomodation { get; set; }
        public int Capacity { get; set; }
        public virtual ICollection<AccomodationAvailability> AccomodationAvailabilities { get; set; }
    }
}
