
using System.Net;

namespace Core.Exceptions
{
    public class NoSeatsAvailableExceptions : Exception
    {
        public HttpStatusCode StatusCode = HttpStatusCode.NotFound;
        public NoSeatsAvailableExceptions(string message) : base(message) { }
    }
}
