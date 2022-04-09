using Maersk.SCM.DeliveryPlanning.Domain.Aggregates.DeliveryPlanAggregate;

namespace Maersk.SCM.DeliveryPlanning.Domain.DomainServices.DeliveryLegValidators
{
    public class StartAndEndLocationsAreValidValidator : ILegValidator
    {
        public LegValidation Validate(DeliveryPlan transportDeliveryPlan, DeliveryPlanLeg leg)
        {
            if (string.IsNullOrEmpty(leg.StartLocation.FullCountryName) || string.IsNullOrEmpty(leg.StartLocation.FullSiteName))
            {
                return LegValidation.InvalidLeg("Start Location is invalid");
            }

            if (string.IsNullOrEmpty(leg.EndLocation.FullCountryName) || string.IsNullOrEmpty(leg.EndLocation.FullSiteName))
            {
                return LegValidation.InvalidLeg("End Location is invalid");
            }

            return LegValidation.ValidLeg();
        }
    }
}
