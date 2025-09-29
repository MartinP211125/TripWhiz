
using System.Net;

namespace Core.Exceptions
{
    public class OfferNullException : Exception
    {
        public HttpStatusCode StatusCode = HttpStatusCode.NotFound;
        public OfferNullException(string message) : base(message) { }
    }
}
