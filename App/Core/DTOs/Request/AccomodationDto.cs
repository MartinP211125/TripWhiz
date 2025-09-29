
using Core.Enums;

namespace Core.DTOs.Request
{
    public class AccommodationDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public StayType StayType { get; set; }
        public string ImageUrl { get; set; }
        public int NumberOfRooms { get; set; }
        public PlaceDto Place { get; set; }
        public OfferType OfferType { get; set; }
    }
}
