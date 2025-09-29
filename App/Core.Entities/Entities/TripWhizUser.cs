

using Microsoft.AspNetCore.Identity;

namespace Core.Entities.Entities
{
    public class TripWhizUser : IdentityUser
    {
        public virtual ICollection<UserActivity> Activities { get; set; }
        public virtual ICollection<Booking> Bookings { get; set; }
    }
}
