using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Exceptions
{
    public class ForbiddenException : Exception
    {
        private const string DefaultMessage = "Forbidden access.";

        public ForbiddenException() : base(DefaultMessage) { }
        public ForbiddenException(string message) : base(message) { }
        public ForbiddenException(string message, Exception innerException) : base(message, innerException) { }
    }
}
