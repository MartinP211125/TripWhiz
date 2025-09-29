
using System.Net;

namespace Core.Exceptions
{
    public class TransportationNullException : Exception
    {
        public HttpStatusCode StatusCode = HttpStatusCode.NotFound;
        public TransportationNullException(string message) : base(message) { }
    }
}
