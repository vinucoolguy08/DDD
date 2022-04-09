using MediatR;
using System;

namespace Maersk.SCM.Framework.Core.Common
{
    public interface IDomainEvent : INotification
    {
    }
}
