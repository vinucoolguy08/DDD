using System;
using System.Threading.Tasks;

namespace Maersk.SCM.DeliveryPlanning.Application.Queries
{
    public interface IDeliveryPlanQueries
    {
        Task<dynamic> GetDeliveryPlanAsync(Guid id);

        Task<dynamic> GetDeliveryPlansSummaryAsync(DeliveryPlanFilter filter);
    }
}
