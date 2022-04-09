using Maersk.SCM.DeliveryPlanning.Domain.Aggregates.DeliveryPlanAggregate;
using System.Linq;

namespace Maersk.SCM.DeliveryPlanning.Domain.DomainServices.DeliveryLegValidators
{
    public class DuplicateLegValidator : ILegValidator
    {
        public LegValidation Validate(DeliveryPlan transportDeliveryPlan, DeliveryPlanLeg leg)
        {

            var isExistingLegByStartLocation = transportDeliveryPlan.Legs.Where(x => x != leg)
                .Any(x => x.StartLocation.SiteCode == leg.StartLocation.SiteCode
                     && x.StartLocation.CountryCode == leg.StartLocation.CountryCode);
            
            if (isExistingLegByStartLocation)
            {
                return LegValidation.InvalidLeg("A duplciate Leg has been added with the same start location.");
            }

            return LegValidation.ValidLeg();
        }
    }
}
