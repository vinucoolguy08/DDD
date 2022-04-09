using System.Collections.Generic;

namespace Maersk.SCM.DeliveryPlanning.Domain.DomainServices.DeliveryLegValidators
{
    public class DefaultLegValidatorConfiguration : ILegValidatorConfiguration
    {
        private List<ILegValidator> _legValidators;

        public DefaultLegValidatorConfiguration()
        {
            _legValidators = new List<ILegValidator>();
            _legValidators.Add(new StartAndEndLocationsAreValidValidator());
            _legValidators.Add(new SameStartAndEndLocationValidator());
            _legValidators.Add(new DuplicateLegValidator());
            _legValidators.Add(new LegDropOffDateBeforePickUpDateValidator());
        }

        public IReadOnlyCollection<ILegValidator> Validators => _legValidators.AsReadOnly();
    }
}
