using Maersk.SCM.DeliveryPlanning.Domain.Aggregates.DeliveryPlanAggregate;
using Maersk.SCM.DeliveryPlanning.Domain.DomainServices;
using Maersk.SCM.Framework.Core.Common;
using Maersk.SCM.Framework.Core.Persistence;
using MediatR;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Maersk.SCM.DeliveryPlanning.Infrastructure.Repositories
{
    public class DeliveryPlanSqlRepository : EventSourcedRepositoryBase<IDeliveryPlan>
    {
        private DeliveryPlanningDbContext _context;
        private readonly IDeliveryPlanFactory _deliveryPlanFactory;
        private const string DeliveryPlanEventsNameSpace = "Maersk.SCM.DeliveryPlanning.Domain.Events.{0},  Maersk.SCM.DeliveryPlanning.Domain, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null";

        public override IUnitOfWork UnitOfWork => _context;

        public DeliveryPlanSqlRepository(
            IMediator mediator, 
            DeliveryPlanningDbContext context,
            IDeliveryPlanFactory deliveryPlanFactory) : base(mediator)
        {
            _context = context;
            _deliveryPlanFactory = deliveryPlanFactory;
        }

        protected async override Task<IDeliveryPlan> LoadEventsAsync(Guid id)
        {
            var eventStoreEvents = await _context.DeliveryPlanEvents.Where(x => x.Id == id).ToListAsync();

            var events = eventStoreEvents.Select(x =>
            {
                var type = Type.GetType(string.Format(DeliveryPlanEventsNameSpace, x.EventName));
                var @event = JsonConvert.DeserializeObject(x.Data, type, new JsonSerializerSettings()
                {
                    ContractResolver = new PrivateResolver(),
                    TypeNameHandling = TypeNameHandling.All
                });

                return (IEvent)@event;
            });

            return _deliveryPlanFactory.Create(events);
        }

        protected override async Task SaveEventsAsync(IDeliveryPlan entity)
        {
            var events = entity.Events
                                        .Select(x => new DeliveryPlanEventSourcedItems(entity.DeliveryPlanId, x.Version, x, x.GetType().Name))
                                        .ToList();

            await _context.DeliveryPlanEvents.AddRangeAsync(events);
        }
    }
}
