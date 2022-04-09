using Maersk.SCM.DeliveryPlanning.Domain.Aggregates.DeliveryOrderAggregate;
using Maersk.SCM.Framework.Core.Persistence;
using MediatR;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Maersk.SCM.DeliveryPlanning.Infrastructure.Repositories
{
    public class DeliveryOrderSqlRepository : EventSourcedRepositoryBase<IDeliveryOrder>
    {
        private DeliveryPlanningDbContext _context;

        public override IUnitOfWork UnitOfWork => _context;

        public DeliveryOrderSqlRepository(IMediator mediator, DeliveryPlanningDbContext context) : base(mediator)
        {
            _context = context;
        }

        protected override Task<IDeliveryOrder> LoadEventsAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        protected override async Task SaveEventsAsync(IDeliveryOrder entity)
        {
            var deliveryOrderEvents = entity.Events
                                        .Select(x => new DeliveryOrderEventSourceItems(entity.DeliveryOrderId, entity.Version, x, x.GetType().Name))
                                        .ToList();

            await _context.DeliveryOrderEvents.AddRangeAsync(deliveryOrderEvents);
            await _context.SaveChangesAsync();
        }
    }
}
