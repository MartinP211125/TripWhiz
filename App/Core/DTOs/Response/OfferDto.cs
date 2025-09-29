
namespace Core.DTOs.Response
{
    public class OfferDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public float Price { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public Guid? CreatedBy { get; set; }
        public Guid AccomodationId { get; set; }
    }
}
