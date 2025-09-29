
using Core.Entities.Base;
using Core.Enums;

namespace Core.Entities.Entities
{
    public class UserActivity : BaseEntity
    {
        public string UserId { get; set; }
        public virtual TripWhizUser User { get; set; }
        public DateTime TimeStamp { get; set; }
        public Guid? TargetId { get; set; }
        public string? Query { get; set; }
        public ActivityType ActivityType { get; set; }
        public TargetType? TargetType { get; set; }
    }
}
