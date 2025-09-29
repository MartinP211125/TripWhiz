
using Core.Enums;

namespace Core.DTOs.Request
{
    public class TransportationDto
    {
        public TransportationType TransportationType { get; set; }
        public float Price { get; set; }
        public int NumberOfSeats { get; set; }
        public OfferDto Offer { get; set; }
    }
}
