
using Core.Entities.Base;
using Core.Enums;

namespace Core.Entities.Entities
{
    public class Accomodation : BaseEntity
    {
        public virtual ICollection<Offer>? Offers { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public StayType StayType { get; set; }
        public Guid PlaceId { get; set; }
        public virtual Place Place { get; set; }
        public int NumberOfRooms { get; set; }
        public virtual ICollection<Room> Rooms { get; set; }
        public OfferType OfferType { get; set; }
    }
}
