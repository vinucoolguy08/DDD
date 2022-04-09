using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace Maersk.SCM.DeliveryPlanning.Application.Commands
{
    public class CreateNewDeliveryPlanCommand : IRequest<Guid>
    {
        [Range(1, long.MaxValue)]
        public long CargoStuffingId { get; set; }

        [Required]
        public string ContainerType { get; set; }

        [Required]
        public string ContainerReference { get; set; }

        [Required]
        public string VesselName { get; set; }

        [Required]
        public string VesselType { get; set; }

        public ServiceModeEnum ServiceMode { get; set; }

        public List<CreateDeliveryLegModel> Legs { get; set; }
    }

    public class CreateDeliveryLegModel
    {
        [Required]
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
    }

    public enum ServiceModeEnum
    {
        [EnumMember(Value = "Road")]
        Road = 10,
        [EnumMember(Value = "Rail")]
        Rail = 20,
        [EnumMember(Value = "Barge")]
        Barge = 30,
        [EnumMember(Value = "Multi")]
        Multi = 40,
    }
}
