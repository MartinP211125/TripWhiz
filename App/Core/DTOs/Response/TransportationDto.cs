
using Core.Enums;

namespace Core.DTOs.Response
{
    public class TransportationDto
    {
        public Guid Id { get; set; }
        public TransportationType TransportationType { get; set; }
        public float Price { get; set; }
        public int NumberOfSeats { get; set; }
        public Guid? OfferId { get; set; }
    }
}
