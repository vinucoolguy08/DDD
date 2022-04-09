using Maersk.SCM.DeliveryPlanning.Domain.Aggregates.DeliveryOrderAggregate;
using Maersk.SCM.DeliveryPlanning.Domain.DomainServices;
using System;
using System.Threading.Tasks;

namespace Maersk.SCM.DeliveryPlanning.Infrastructure.DomainServices
{
    public class DefaultDeliveryOrderNumberGenerator : IDeliveryOrderNumberGenerator
    {
        private const string TransportOrderTemplate = "MDO{YY}{NO}";
        private readonly DeliveryPlanningDbContext _context;

        public DefaultDeliveryOrderNumberGenerator(DeliveryPlanningDbContext context)
        {
            _context = context;
        }

        public async Task<string> GenerateOrderNumber(DeliveryOrderReference deliveryOrderReference)
        {
            var yearString = DateTime.UtcNow.Year.ToString();
            var tranportOrderWithYear = TransportOrderTemplate.Replace("{YY}", yearString[^2..]);

            deliveryOrderReference = _context.DeliveryOrderReferences.Add(deliveryOrderReference).Entity;
            await _context.SaveChangesAsync();

            return tranportOrderWithYear.Replace("{NO}", deliveryOrderReference.DeliveryOrderNumberId.ToString().PadLeft(6, '0'));
        }
    }
}
