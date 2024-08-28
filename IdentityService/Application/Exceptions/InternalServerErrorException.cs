using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Exceptions
{
    public class InternalServerErrorException : Exception
    {
        private const string DefaultMessage = "Internal server error.";

        public InternalServerErrorException() : base(DefaultMessage) { }
        public InternalServerErrorException(string message) : base(message) { }
        public InternalServerErrorException(string message, Exception innerException) : base(message, innerException) { }
    }
}
