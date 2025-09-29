
using Core.Entities.Base;
using Core.Enums;

namespace Core.Entities.Entities
{
    public class Place : BaseEntity
    {
        public string City { get; set; }
        public string Country { get; set; }
        public virtual ICollection<Accomodation>? Accomodations { get; set; }
    }
}
