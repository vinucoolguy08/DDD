using Maersk.SCM.DeliveryPlanning.Domain.Aggregates.DeliveryPlanAggregate;

namespace Maersk.SCM.DeliveryPlanning.Domain.DomainServices.DeliveryLegValidators
{
    public class SameStartAndEndLocationValidator : ILegValidator
    {
        public LegValidation Validate(DeliveryPlan transportDeliveryPlan, DeliveryPlanLeg leg)
        {
            if (leg.StartLocation == leg.EndLocation)
            {
                return LegValidation.InvalidLeg("Start location cannot be the same as end location.");
            }

            return LegValidation.ValidLeg();
        }
    }
}
