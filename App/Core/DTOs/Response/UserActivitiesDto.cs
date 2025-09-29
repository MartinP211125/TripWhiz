
using Core.Enums;

namespace Core.DTOs.Response
{
    public class UserActivitiesDto
    {
        public Guid Id { get; set; }
        public string UserId { get; set; }
        public DateTime TimeStamp { get; set; }
        public Guid? TargetId { get; set; }
        public TargetType? TargetType { get; set; }
        public string? Query { get; set; }
        public ActivityType ActivityType { get; set; }
    }
}
