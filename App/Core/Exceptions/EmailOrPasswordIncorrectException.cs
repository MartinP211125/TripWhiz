
using System.Net;

namespace Core.Exceptions
{
    public class EmailOrPasswordIncorrectException : Exception
    {
        public HttpStatusCode StatusCode = HttpStatusCode.BadRequest;
        public EmailOrPasswordIncorrectException(string message) : base(message) { }
    }
}
