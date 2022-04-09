using Maersk.SCM.Framework.Core.Common;
using System;
using System.Threading.Tasks;

namespace Maersk.SCM.Framework.Core.Persistence
{
    public interface IEventSourcedRepository<TEntity> where TEntity : IEntity
    {
        public IUnitOfWork UnitOfWork { get; }

        Task<TEntity> LoadAsync(Guid id);

        Task SaveAsync(TEntity entity);
    }
}
