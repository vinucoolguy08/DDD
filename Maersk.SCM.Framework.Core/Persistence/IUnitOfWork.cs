using System;
using System.Threading;
using System.Threading.Tasks;

namespace Maersk.SCM.Framework.Core.Persistence
{
    public interface IUnitOfWork : IDisposable
    {
        Task<bool> SaveEntitiesAsync(CancellationToken cancellationToken = default);
    }
}
