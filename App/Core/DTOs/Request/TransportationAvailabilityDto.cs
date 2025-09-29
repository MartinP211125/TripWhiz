
namespace Core.DTOs.Request
{
    public class TransportationAvailabilityDto
    {
        public Guid TransportationId { get; set; }
        public Guid SeatId { get; set; }
        public DateTime Departure { get; set; }
        public DateTime? Arrival { get; set; }
    }
}
