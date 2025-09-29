
using System.Net;

namespace Core.Exceptions
{
    public class FailedGroqResponseException : Exception
    {
        public HttpStatusCode StatusCode = HttpStatusCode.NoContent;
        public FailedGroqResponseException(string message) : base(message) { }
    }
}
