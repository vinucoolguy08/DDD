using MediatR;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Maersk.SCM.DeliveryPlanning.Application.Commands
{
    public class UpdateDeliveryPlanCommand : IRequest
    {
        [JsonIgnore]
        public Guid DeliveryPlanId { get; set; }

        [Required]
        public string VesselName { get; set; }

        [Required]
        public string VesselType { get; set; }

        public List<UpdateDeliveryLegModel> Legs { get; set; }
    }

    public class UpdateDeliveryLegModel
    {
        public Guid? DeliveryPlanLegId { get; set; }

        public string ProviderCode { get; set; }

        public DateTime PickUpDate { get; set; }

        public DateTime DropOffDate { get; set; }

        [Required]
        public string StartLocationCountryCode { get; set; }

        [Required]
        public string StartLocationSiteCode { get; set; }

        [Required]
        public string EndLocationCountryCode { get; set; }

        [Required]
        public string EndLocationSiteCode { get; set; }

        public string Status { get; set; }
    }
}
