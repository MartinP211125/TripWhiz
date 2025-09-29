
using Core.Enums;

namespace Core.DTOs.Request
{
    public class CustomeActivityDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string StayType { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public DateTime TimeStamp { get; set; }
        public ActivityType ActivityType { get; set; }
        public string? Query { get; set; }
    }
}
