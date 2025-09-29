
namespace Core.DTOs.Request
{
    public class AccomodationAvailabilityDto
    {
        public Guid RoomId { get; set; }
        public Guid AccomodationId { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
    }
}
