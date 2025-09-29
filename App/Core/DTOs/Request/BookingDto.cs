
using Core.Enums;

namespace Core.DTOs.Request
{
    public class BookingDto
    {
        public int NumberOfPeople { get; set; }
        public Guid OfferId { get; set; }
        public TransportationType TransportationType { get; set; }
    }
}
