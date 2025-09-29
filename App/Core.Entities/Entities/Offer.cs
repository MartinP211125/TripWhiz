
using Core.Entities.Base;
using Core.Enums;

namespace Core.Entities.Entities
{
    public class Offer : BaseDateEntity
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public virtual Accomodation Accomodation {  get; set; }
        public Guid AccomodationId { get; set; }
        public string ImageUrl { get; set; }
        public Guid? CreatedBy { get; set; }
        public float Price { get; set; }
        public virtual ICollection<Transportation> Transportation { get; set; }
        public virtual ICollection<Booking> Bookings { get; set; }
        public OfferType OfferType { get; set; }
    }
}
