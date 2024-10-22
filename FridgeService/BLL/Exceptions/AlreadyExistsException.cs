using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Exceptions
{
    public class AlreadyExistsException : Exception
    {
        private const string DefaultMessage = "The resource already exists.";

        public AlreadyExistsException() : base(DefaultMessage) { }
        public AlreadyExistsException(string message) : base(message) { }
        public AlreadyExistsException(string message, Exception innerException) : base(message, innerException) { }
    }
}
