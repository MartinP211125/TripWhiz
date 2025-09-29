
using Core.Enums;

namespace Core.DTOs.Request
{
    public class OfferDto
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public float Price { get; set; }
        public DateTime ToDate { get; set; }
        public DateTime FromDate { get; set; }
        public AccommodationDto Accommodation { get; set; }
        public OfferType OfferType { get; set; }
    }
}
