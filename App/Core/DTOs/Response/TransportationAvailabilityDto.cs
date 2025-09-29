
namespace Core.DTOs.Response
{
    public class TransportationAvailabilityDto
    {
        public Guid Id { get; set; }
        public Guid TransportationId { get; set; }
        public Guid SeatId { get; set; }
        public DateTime Departure { get; set; }
        public DateTime? Arrival { get; set; }
    }
}
