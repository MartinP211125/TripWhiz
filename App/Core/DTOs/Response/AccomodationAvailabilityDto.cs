
namespace Core.DTOs.Response
{
    public class AccomodationAvailabilityDto
    {
        public Guid Id { get; set; }
        public Guid RoomId { get; set; }
        public Guid AccomodationId { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime? ToDate { get; set; }
    }
}
