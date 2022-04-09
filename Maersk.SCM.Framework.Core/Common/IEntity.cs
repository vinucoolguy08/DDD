using System.Collections.Generic;

namespace Maersk.SCM.Framework.Core.Common
{
    public interface IEntity
    {
        IReadOnlyCollection<IDomainEvent> DomainEvents { get; }

        IReadOnlyCollection<IEvent> Events { get; }

        IReadOnlyCollection<IEvent> EventLogs { get; }

        void AddEvent(IEvent @event);

        void RemoveEvent(IEvent @event);

        void ClearEvents();

        void ClearDomainEvents();

        void AddEventLog(IEvent @event);

        void AddEventLogs(IEnumerable<IEvent> eventLogs);

        bool IsEntityBeingUpdated();
    }
}
