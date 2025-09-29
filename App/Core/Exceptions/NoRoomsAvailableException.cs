
using System.Net;

namespace Core.Exceptions
{
    public class NoRoomsAvailableException : Exception
    {
        public HttpStatusCode StatusCode = HttpStatusCode.NotFound;
        public NoRoomsAvailableException(string message) : base(message) { }
    }
}
