
using System.Net;

namespace Core.Exceptions
{
    public class AccomodationAvailabilityNotFoundException : Exception
    {
        public HttpStatusCode StatusCode = HttpStatusCode.NotFound;
        public AccomodationAvailabilityNotFoundException(string message) : base(message) { }
    }
}
