using Maersk.SCM.DeliveryPlanning.Domain.Common;
using Maersk.SCM.DeliveryPlanning.Domain.DomainServices;
using System.Threading.Tasks;

namespace Maersk.SCM.DeliveryPlanning.Infrastructure.DomainServices
{
    public class MdmApiLocationService : ILocationService
    {
        public Task<Location> GetFullLocation(string countryCode, string siteCode)
        {
            return Task.FromResult(new Location(countryCode, siteCode, $"{countryCode} Full Name", $"{siteCode} Full Name"));
        }
    }
}
