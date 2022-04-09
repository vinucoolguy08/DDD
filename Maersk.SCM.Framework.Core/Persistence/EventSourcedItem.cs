using Newtonsoft.Json;
using System;
using System.Diagnostics;

namespace Maersk.SCM.Framework.Core.Persistence
{
    public record EventSourcedItem
    {
        public Guid Id { get; private set; }

        public int Version { get; private set; }

        public string Data { get; private set; }

        public string EventName { get; private set; }

        public DateTime CreatedDate { get; private set; }

        public string TraceId { get; private set; }

        protected EventSourcedItem()
        {
        }

        protected EventSourcedItem(Guid id, int version, object data, string eventName)
        {
            Id = id;
            Version = version;
            Data = JsonConvert.SerializeObject(data, new JsonSerializerSettings()
            {
                TypeNameHandling = TypeNameHandling.All
            });
            EventName = eventName;
            CreatedDate = DateTime.UtcNow;
            TraceId = Activity.Current.TraceId.ToString();
        }
    }
}
