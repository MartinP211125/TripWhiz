
using System.Net;

namespace Core.Exceptions
{
    public class AccomodationNullException : Exception
    {
        public HttpStatusCode StatusCode = HttpStatusCode.NotFound;
        public AccomodationNullException(string message) : base(message) { }
    }
}
