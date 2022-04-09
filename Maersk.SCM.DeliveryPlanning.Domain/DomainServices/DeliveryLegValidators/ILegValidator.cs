using Maersk.SCM.DeliveryPlanning.Domain.Aggregates.DeliveryPlanAggregate;
using Maersk.SCM.DeliveryPlanning.Domain.Common;
using Maersk.SCM.Framework.Core.Common;

namespace Maersk.SCM.DeliveryPlanning.Domain.DomainServices.DeliveryLegValidators
{
    public interface ILegValidator
    {
        LegValidation Validate(DeliveryPlan transportDeliveryPlan, DeliveryPlanLeg leg);
    }

    public class LegValidation
    {
        public static LegValidation ValidLeg()
        {
            return new LegValidation();
        }

        public static LegValidation InvalidLeg(string errorMessage)
        {
            return new LegValidation(errorMessage);
        }

        public string ErrorMessage { get; private set; }

        public bool IsValid => string.IsNullOrEmpty(ErrorMessage);

        private LegValidation()
        {
        }

        private LegValidation(string errorMessage)
        {
            ErrorMessage = errorMessage.VerifyOrThrowException(nameof(errorMessage));
        }
    }

}
