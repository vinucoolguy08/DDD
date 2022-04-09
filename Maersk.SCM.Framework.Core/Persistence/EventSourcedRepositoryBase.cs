using Maersk.SCM.Framework.Core.Common;
using MediatR;
using System;
using System.Threading.Tasks;

namespace Maersk.SCM.Framework.Core.Persistence
{
    public abstract class EventSourcedRepositoryBase<TEntity> : IEventSourcedRepository<TEntity> where TEntity : IEntity
    {
        private readonly IMediator _mediator;

        public abstract IUnitOfWork UnitOfWork { get; }

        protected abstract Task SaveEventsAsync(TEntity entity);

        protected abstract Task<TEntity> LoadEventsAsync(Guid id);

        public EventSourcedRepositoryBase(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<TEntity> LoadAsync(Guid id)
        {
            return await LoadEventsAsync(id);
        }

        public async Task SaveAsync(TEntity entity)
        {
            await SaveEventsAsync(entity);

            foreach (var domainEvent in entity.DomainEvents)
            {
                await _mediator.Publish(domainEvent);
            }

            entity.ClearDomainEvents();

            await UnitOfWork.SaveEntitiesAsync();
        }

        public void Dispose()
        {
            
        }
    }
}
