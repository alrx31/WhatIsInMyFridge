using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infastructure.Middlewares.Exceptions
{
    public class NotFoundException : Exception
    {
        private const string DefaultMessage = "The requested resource was not found.";

        public NotFoundException() : base(DefaultMessage) { }
        public NotFoundException(string message) : base(message) { }
        public NotFoundException(string message, Exception innerException) : base(message, innerException) { }
    }
}
