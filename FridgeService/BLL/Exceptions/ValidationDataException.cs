using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Exceptions
{
    public class ValidationDataException : Exception
    {
        private const string DefaultMessage = "Validation failed.";

        public ValidationDataException() : base(DefaultMessage) { }
        public ValidationDataException(string message) : base(message) { }
        public ValidationDataException(string message, Exception innerException) : base(message, innerException) { }
    }
}
