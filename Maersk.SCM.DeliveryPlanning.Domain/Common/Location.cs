using Maersk.SCM.Framework.Core.Common;
using System.Collections.Generic;

namespace Maersk.SCM.DeliveryPlanning.Domain.Common
{
    public class Location : ValueObject
    {
        public string CountryCode { get; }

        public string SiteCode { get; }

        public string FullCountryName { get; }

        public string FullSiteName { get; }

        public Location(string countryCode, string siteCode, string fullCountryName, string fullSiteName)
        {
            CountryCode = countryCode.VerifyOrThrowException(nameof(countryCode));
            SiteCode = siteCode.VerifyOrThrowException(nameof(siteCode));
            FullCountryName = fullCountryName;
            FullSiteName = fullSiteName;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return CountryCode;
            yield return SiteCode;
            yield return FullCountryName;
            yield return FullSiteName;
        }
    }
}
