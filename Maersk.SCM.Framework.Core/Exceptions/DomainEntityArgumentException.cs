using System;

namespace Maersk.SCM.Framework.Core.Exceptions
{
    public class DomainEntityArgumentException : Exception
    {
        public DomainEntityArgumentException(string message) : base(message)
        {
        }
    }
}
