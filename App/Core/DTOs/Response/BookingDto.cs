
using Core.Enums;

namespace Core.DTOs.Response
{
    public class BookingDto
    {
        public Guid Id { get; set; }
        public string UserId { get; set; }
        public Guid OfferId { get; set; }
        public Status Status { get; set; }
        public int NumberOfPeople { get; set; }
        public float TotalPrice { get; set; }
    }
}
