using System;

namespace Maersk.SCM.Framework.Core.Messaging
{
    public record IntegrationEvent
    {
        public Guid Id { get; set; }

        public DateTime CreationDate { get; set; }

        public string CreatedBy { get; set; }
    }
}
