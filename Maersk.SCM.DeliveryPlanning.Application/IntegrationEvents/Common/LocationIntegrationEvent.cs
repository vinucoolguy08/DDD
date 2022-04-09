namespace Maersk.SCM.DeliveryPlanning.Application.IntegrationEvents.Common
{
    public class LocationIntegrationEvent
    {
        public string CountryCode { get; set; }

        public string SiteCode { get; set; }

        public string FullCountryName { get; set; }

        public string FullSiteName { get; set; }
    }
}
