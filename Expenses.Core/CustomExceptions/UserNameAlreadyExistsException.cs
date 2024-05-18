using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Expenses.Core.CustomExceptions
{
    public class UserNameAlreadyExistsException : Exception
    {
        public UserNameAlreadyExistsException()
        {
        }

        public UserNameAlreadyExistsException(string? message) : base(message)
        {
        }

        public UserNameAlreadyExistsException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected UserNameAlreadyExistsException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
