using System;

namespace Maersk.SCM.Framework.Core.Exceptions
{
    public class DomainValidationException : Exception
    {
        public DomainValidationException(string message) : base(message)
        {

        }
    }
}
