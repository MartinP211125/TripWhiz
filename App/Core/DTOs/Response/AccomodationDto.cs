
using Core.Enums;

namespace Core.DTOs.Response
{
    public class AccomodationDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public StayType StayType { get; set; }
        public string ImageUrl { get; set; }
        public int NumberOfRooms { get; set; }
        public Guid PlaceId { get; set; }
    }
}
