using Maersk.SCM.DeliveryPlanning.Domain.Aggregates.DeliveryPlanAggregate;
using Maersk.SCM.DeliveryPlanning.Domain.Common;
using Maersk.SCM.DeliveryPlanning.Domain.DomainServices;
using Maersk.SCM.DeliveryPlanning.Domain.DomainServices.DeliveryLegValidators;
using Maersk.SCM.Framework.Core.Common;
using Maersk.SCM.Framework.Core.Persistence;
using MediatR;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Maersk.SCM.DeliveryPlanning.Application.Commands
{
    public class UpdateDeliveryPlanCommandHandler : IRequestHandler<UpdateDeliveryPlanCommand>
    {
        private readonly IEventSourcedRepository<IDeliveryPlan> _eventSourcedRepository;
        private readonly ILocationService _locationService;
        private readonly ILegValidatorConfiguration _legValidatorConfiguration;

        public UpdateDeliveryPlanCommandHandler(
            IEventSourcedRepository<IDeliveryPlan> eventSourcedRepository, 
            ILocationService locationService,
            ILegValidatorConfiguration legValidatorConfiguration)
        {
            _eventSourcedRepository = eventSourcedRepository;
            _locationService = locationService;
            _legValidatorConfiguration = legValidatorConfiguration;
        }
        public async Task<Unit> Handle(UpdateDeliveryPlanCommand request, CancellationToken cancellationToken)
        {
            var existingDeliveryPlan = await _eventSourcedRepository.LoadAsync(request.DeliveryPlanId);

            if (existingDeliveryPlan == null)
            {
                throw new Exception("Not Found Exception");
            }

            existingDeliveryPlan.UpdateVesselDetails(request.VesselName, request.VesselType);

            var allLegIds = request.Legs.Where(x => x.DeliveryPlanLegId.HasValue).Select(x => x.DeliveryPlanLegId.Value).ToList();
            existingDeliveryPlan.RemoveLegsNotInList(allLegIds);
            
            foreach(var leg in request.Legs)
            {
                var startLocation = await _locationService.GetFullLocation(leg.StartLocationCountryCode, leg.StartLocationSiteCode);
                var endLocation = await _locationService.GetFullLocation(leg.EndLocationCountryCode, leg.EndLocationSiteCode);

                if (!leg.DeliveryPlanLegId.HasValue)
                {
                    existingDeliveryPlan.AddLeg(
                        leg.ProviderCode, 
                        leg.PickUpDate, 
                        leg.DropOffDate, 
                        startLocation, 
                        endLocation);
                }
                else
                {
                    existingDeliveryPlan.UpdateLeg(
                        leg.DeliveryPlanLegId.Value, 
                        leg.PickUpDate, 
                        leg.DropOffDate, 
                        startLocation, 
                        endLocation, 
                        Enumeration.FromDisplayName<LegStatus>(leg.Status));
                }
            }

            await _eventSourcedRepository.SaveAsync(existingDeliveryPlan);

            return new Unit();
        }
    }
}
