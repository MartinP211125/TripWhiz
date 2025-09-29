
namespace Core.Entities.Base
{
    public class BaseDateEntity : BaseEntity
    {
        public DateTime FromDate { get; set; }
        public DateTime? ToDate { get; set; }
    }
}
