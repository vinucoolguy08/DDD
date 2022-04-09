using Maersk.SCM.DeliveryPlanning.Domain.Aggregates.DeliveryOrderAggregate;
using System.Threading.Tasks;

namespace Maersk.SCM.DeliveryPlanning.Domain.DomainServices
{
    public interface IDeliveryOrderNumberGenerator
    {
        Task<string> GenerateOrderNumber(DeliveryOrderReference deliveryOrderReference);
    }
}
