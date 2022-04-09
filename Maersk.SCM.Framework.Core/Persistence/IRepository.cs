using Maersk.SCM.Framework.Core.Common;

namespace Maersk.SCM.Framework.Core.Persistence
{
    public interface IRepository<T> where T : IAggregateRoot
    {
        IUnitOfWork UnitOfWork { get; }
    }
}
