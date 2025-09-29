
using System.Net;

namespace Core.Exceptions
{
    public class PasswordDoNotMatchException : Exception
    {
        public HttpStatusCode StatusCode = HttpStatusCode.BadRequest;
        public PasswordDoNotMatchException(string message) : base(message) { }
    }
}
