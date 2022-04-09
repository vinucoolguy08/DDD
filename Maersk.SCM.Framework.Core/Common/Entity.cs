using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace Maersk.SCM.Framework.Core.Common
{
    public abstract class Entity : IEntity
    {
        private List<IDomainEvent> _domainEvents;
        private List<IEvent> _events;
        private List<IEvent> _eventLogs;

        [JsonIgnore]
        public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents?.AsReadOnly();

        [JsonIgnore]
        public IReadOnlyCollection<IEvent> Events => _events?.AsReadOnly();

        [JsonIgnore]
        public IReadOnlyCollection<IEvent> EventLogs => _eventLogs?.AsReadOnly();

        public Entity()
        {
            _domainEvents = new List<IDomainEvent>();
            _events = new List<IEvent>();
            _eventLogs = new List<IEvent>();
        }

        public void AddEvent(IEvent @event)
        {
            @event.Version = GetNextVersion();

            if (@event.AssociatedDomainEvent != null)
            {
                if (!_domainEvents.Any(x => x.GetType() == @event.AssociatedDomainEvent.GetType()))
                {
                    _domainEvents.Add(@event.AssociatedDomainEvent);
                }
            }

            _events.Add(@event);
        }

        public void RemoveEvent(IEvent @event)
        {
            _events?.Remove(@event);
        }

        public void ClearEvents()
        {
            _events?.Clear();
        }

        public void ClearDomainEvents()
        {
            _domainEvents?.Clear();
        }

        public void AddEventLog(IEvent @event)
        {
            _eventLogs.Add(@event);
        }

        public void AddEventLogs(IEnumerable<IEvent> eventLogs)
        {
            _eventLogs.AddRange(eventLogs);
        }

        public bool IsEntityBeingUpdated()
        {
            return _eventLogs.Any();
        }

        protected void Apply(IEvent @event)
        {
            AddEventLog(@event);
            Mutate(@event);
        }

        protected void Mutate(IEvent @event)
        {
            ((dynamic)this).When((dynamic)@event);
        }

        private int GetNextVersion()
        {
            var version = 0;

            if (_eventLogs.Count > 0)
            {
                var existingEvent = _eventLogs.OrderBy(x => x.Version).Last();
                if (existingEvent != null)
                {
                    version = existingEvent.Version;
                }
            }

            if (_events.Count > 0)
            {
                var newEvent = _events.OrderBy(x => x.Version).Last();
                if (newEvent != null && version < newEvent.Version)
                {
                    version = newEvent.Version;
                }
            }

            return version + 1;
        }
    }
}
