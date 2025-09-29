
using Core.Entities.Base;

namespace Core.Entities.Entities
{
    public class AccomodationAvailability : BaseDateEntity
    {
        public Guid RoomId { get; set; }
        public Guid AccomodationId { get; set; }
        public virtual Room Room { get; set; }
    }
}
