using Maersk.SCM.DeliveryPlanning.Application.IntegrationEvents;
using Maersk.SCM.DeliveryPlanning.Domain.Aggregates.DeliveryPlanAggregate;
using Maersk.SCM.DeliveryPlanning.Domain.DomainServices;
using Maersk.SCM.DeliveryPlanning.Domain.DomainServices.DeliveryLegValidators;
using Maersk.SCM.Framework.Core.Messaging;
using Maersk.SCM.Framework.Core.Persistence;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Maersk.SCM.DeliveryPlanning.Application.Commands
{
    public class CreateNewDeliveryPlanCommandHandler : IRequestHandler<CreateNewDeliveryPlanCommand, Guid>
    {
        private readonly IMessageBroker _deliveryPlanMessageBroker;
        private readonly IDeliveryPlanFactory _deliveryPlanFactory;
        private readonly ILocationService _locationService;
        private readonly IEventSourcedRepository<IDeliveryPlan> _eventSourcedRepository;
        private readonly ILegValidatorConfiguration _legValidatorConfiguration;

        public CreateNewDeliveryPlanCommandHandler(
            IMessageBrokerFactory messageBrokerFactory,
            IDeliveryPlanFactory deliveryPlanFactory,
            ILocationService locationService,
            IEventSourcedRepository<IDeliveryPlan> eventSourcedRepository,
            ILegValidatorConfiguration legValidatorConfiguration)
        {
            _deliveryPlanMessageBroker = messageBrokerFactory.GetBrokerClient("DeliveryPlanCreated");
            _deliveryPlanFactory = deliveryPlanFactory;
            _locationService = locationService;
            _eventSourcedRepository = eventSourcedRepository;
            _legValidatorConfiguration = legValidatorConfiguration;
        }

        public async Task<Guid> Handle(CreateNewDeliveryPlanCommand request, CancellationToken cancellationToken)
        {
            var deliveryPlan = _deliveryPlanFactory.Create(
                request.CargoStuffingId, 
                request.ContainerType, 
                request.ContainerReference, 
                request.VesselName, 
                request.VesselType);

            foreach (var leg in request.Legs)
            {
                var startLocation = await _locationService.GetFullLocation(leg.StartLocationCountryCode, leg.StartLocationSiteCode);
                var endLocation = await _locationService.GetFullLocation(leg.EndLocationCountryCode, leg.EndLocationSiteCode);
                deliveryPlan.AddLeg(leg.ProviderCode, leg.PickUpDate, leg.DropOffDate, startLocation, endLocation);
            }

            await _eventSourcedRepository.SaveAsync(deliveryPlan);

            await _deliveryPlanMessageBroker.PublishAsync(DeliveryPlanCreatedIntegrationEvent.ToIntegrationEvent(deliveryPlan));

            return deliveryPlan.DeliveryPlanId;
        }
    }
}
