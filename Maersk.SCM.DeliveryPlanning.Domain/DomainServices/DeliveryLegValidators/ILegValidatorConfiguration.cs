using System.Collections.Generic;

namespace Maersk.SCM.DeliveryPlanning.Domain.DomainServices.DeliveryLegValidators
{
    public interface ILegValidatorConfiguration
    {
        IReadOnlyCollection<ILegValidator> Validators { get; }
    }
}
