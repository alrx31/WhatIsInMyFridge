using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.Middlewares.Exceptions
{
    public class BadRequestException : Exception
    {
        private const string DefaultMessage = "Bad request.";

        public BadRequestException() : base(DefaultMessage) { }
        public BadRequestException(string message) : base(message) { }
        public BadRequestException(string message, Exception innerException) : base(message, innerException) { }
    }
}
