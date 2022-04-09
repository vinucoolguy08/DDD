using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Maersk.SCM.DeliveryPlanning.Domain.Aggregates.DeliveryOrderAggregate
{
    public class DeliveryOrderReference
    {
        public static DeliveryOrderReference CreateNew(Guid deliveryOrderId)
        {
            return new DeliveryOrderReference(deliveryOrderId);
        }

        [Key]
        public Guid DeliveryOrderId { get; private set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int DeliveryOrderNumberId { get; private set; }

        private DeliveryOrderReference()
        {
        }

        public DeliveryOrderReference(Guid deliveryOrderId)
        {
            DeliveryOrderId = deliveryOrderId;
        }
    }
}
