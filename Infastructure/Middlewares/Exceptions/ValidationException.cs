using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infastructure.Middlewares.Exceptions
{
    public class ValidationException : Exception
    {
        private const string DefaultMessage = "Validation failed.";

        public ValidationException() : base(DefaultMessage) { }
        public ValidationException(string message) : base(message) { }
        public ValidationException(string message, Exception innerException) : base(message, innerException) { }
    }
}
