using Maersk.SCM.Framework.Core.Common;
using System.Collections.Generic;

namespace Maersk.SCM.DeliveryPlanning.Domain.Common
{
    public class Vessel : ValueObject
    {
        public string Name { get; private set; }

        public string Type { get; private set; }

        public Vessel(string name, string type)
        {
            Name = name;
            Type = type;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Name;
            yield return Type;
        }
    }
}
