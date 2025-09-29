
using System.Net;

namespace Core.Exceptions
{
    public class UserAlreadyExistsException : Exception
    {
        public HttpStatusCode StatusCode = HttpStatusCode.BadRequest;
        public UserAlreadyExistsException(string message) : base(message) { }
    }
}
