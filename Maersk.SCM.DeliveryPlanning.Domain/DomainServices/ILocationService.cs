using Maersk.SCM.DeliveryPlanning.Domain.Common;
using System.Threading.Tasks;

namespace Maersk.SCM.DeliveryPlanning.Domain.DomainServices
{
    public interface ILocationService
    {
        Task<Location> GetFullLocation(string countryCode, string siteCode);
    }
}
