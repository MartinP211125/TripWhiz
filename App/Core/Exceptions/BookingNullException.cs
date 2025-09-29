
using System.Net;

namespace Core.Exceptions
{
    public class BookingNullException : Exception
    {
        public HttpStatusCode StatusCode = HttpStatusCode.NotFound;
        public BookingNullException(string message) : base(message) { }
    }
}
