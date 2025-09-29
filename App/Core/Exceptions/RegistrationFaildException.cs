
using System.Net;

namespace Core.Exceptions
{
    public class RegistrationFaildException : Exception
    {
        public HttpStatusCode StatusCode = HttpStatusCode.BadRequest;
        public RegistrationFaildException(string message) : base(message) { }
    }
}
