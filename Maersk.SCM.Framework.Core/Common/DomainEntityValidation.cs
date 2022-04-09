namespace Maersk.SCM.Framework.Core.Common
{
    public class DomainEntityValidation
    {
        public static DomainEntityValidation Valid()
        {
            return new DomainEntityValidation(null);
        }

        public static DomainEntityValidation Invalid(string message)
        {
            return new DomainEntityValidation(message);
        }

        private DomainEntityValidation(string message)
        {
            Message = message;
        }

        public bool IsValid => Message?.Length == 0;

        public string Message { get; }
    }
}
