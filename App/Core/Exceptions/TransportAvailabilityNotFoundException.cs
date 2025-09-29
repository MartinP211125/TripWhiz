
using System.Net;

namespace Core.Exceptions
{
    public class TransportAvailabilityNotFoundException : Exception
    {
        public HttpStatusCode StatusCode = HttpStatusCode.NotFound;
        public TransportAvailabilityNotFoundException(string message) : base(message) { }
    }
}
