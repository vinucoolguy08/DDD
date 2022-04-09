using Newtonsoft.Json;
using System;

namespace Maersk.SCM.Framework.Core.Common
{
    public interface IEvent
    {
        [JsonIgnore]
        public IDomainEvent AssociatedDomainEvent { get; }

        public Guid Id { get; }

        public int Version { get; set; }

        public string TraceId { get; }
    }
}
