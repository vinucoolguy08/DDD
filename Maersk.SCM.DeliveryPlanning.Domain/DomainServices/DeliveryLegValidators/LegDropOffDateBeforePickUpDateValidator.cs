using Maersk.SCM.DeliveryPlanning.Domain.Aggregates.DeliveryPlanAggregate;

namespace Maersk.SCM.DeliveryPlanning.Domain.DomainServices.DeliveryLegValidators
{
    public class LegDropOffDateBeforePickUpDateValidator : ILegValidator
    {
        public LegValidation Validate(DeliveryPlan transportDeliveryPlan, DeliveryPlanLeg leg)
        {
            if (leg.DropOffDate <= leg.PickUpDate)
            {
                return LegValidation.InvalidLeg("Drop off date/time cannot be before or the same as the pick up date/time.");
            }

            return LegValidation.ValidLeg();
        }
    }
}
