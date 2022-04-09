using Maersk.SCM.DeliveryPlanning.Domain.Common;
using Maersk.SCM.DeliveryPlanning.Domain.DomainServices.DeliveryLegValidators;
using Maersk.SCM.DeliveryPlanning.Domain.Events;
using Maersk.SCM.Framework.Core.Common;
using Maersk.SCM.Framework.Core.Exceptions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Maersk.SCM.DeliveryPlanning.Domain.Aggregates.DeliveryPlanAggregate
{
    public class DeliveryPlan : Entity, IDeliveryPlan, IAggregateRoot
    {
        [JsonProperty(PropertyName = "Legs")]
        private List<DeliveryPlanLeg> _legs;
        private readonly ILegValidatorConfiguration _legValidatorConfiguration;

        public Guid DeliveryPlanId { get; private set; }

        public long CargoStuffingId { get; private set; }

        public Shipment Shipment { get; private set; }

        public BookingStatus Status { get; private set; }

        [JsonIgnore]
        public IReadOnlyCollection<DeliveryPlanLeg> Legs => _legs.AsReadOnly();

        // Required for Entity Framework
        private DeliveryPlan()
        {
            _legs = new List<DeliveryPlanLeg>();
        }

        public DeliveryPlan(Guid servicePlanId, long cargoStuffingId, Shipment shipment, BookingStatus status, ILegValidatorConfiguration legValidatorConfiguration)
            : this()
        {
            _legValidatorConfiguration = legValidatorConfiguration;
            DeliveryPlanId = servicePlanId.VerifyOrThrowException(nameof(servicePlanId));
            CargoStuffingId = cargoStuffingId.VerifyOrThrowException(nameof(cargoStuffingId));
            Shipment = shipment;
            Status = status;
            AddEvent(new DeliveryPlanCreatedEvent(this, DeliveryPlanId));
        }

        public DeliveryPlan(IEnumerable<IEvent> @events, ILegValidatorConfiguration legValidatorConfiguration) : this()
        {
            _legValidatorConfiguration = legValidatorConfiguration;

            AddEventLogs(events);

            foreach (var @event in events)
            {
                Mutate(@event);
            }
        }

        public void AddLeg(string providerCode, DateTime pickUpDate, DateTime dropOffDate, Location startLocation, Location endLocation)
        {
            var legToAdd = new DeliveryPlanLeg(providerCode, pickUpDate, dropOffDate, startLocation, endLocation, LegStatus.Created);
            _legs.Add(legToAdd);

            SortLegsByPickUpDate();
            ValidateSortedLegs();

            if (IsEntityBeingUpdated())
            {
                AddEvent(new DeliveryPlanLegAddedEvent(this, legToAdd, DeliveryPlanId));
            }
        }

        public void UpdateLeg(Guid deliveryPlanLegId, DateTime pickUpDate, DateTime dropOffDate, Location startLocation, Location endLocation, LegStatus legStatus)
        {
            var legToUpdate = _legs.Find(x => x.DeliveryLegId == deliveryPlanLegId);

            legToUpdate.UpdateDetails(pickUpDate, dropOffDate, startLocation, endLocation);
            
            if (legToUpdate.HasDetailsChanged)
            {
                SortLegsByPickUpDate();
                ValidateSortedLegs();

                AddEvent(new DeliveryPlanLegUpdatedEvent(this, legToUpdate, DeliveryPlanId));
            }

            UpdateLegStatus(legToUpdate, legStatus);
        }

        public void UpdateVesselDetails(string name, string type)
        {
            var newVessel = new Vessel(name, type);
            if (!Shipment.Vessel.Equals(newVessel))
            {
                Shipment.UpdateVessel(newVessel);
                AddEvent(new DeliveryPlanVesselDetailsUpdatedEvent(this, Shipment.Vessel, DeliveryPlanId));
            }
        }

        private void UpdateLegStatus(DeliveryPlanLeg legToUpdate, LegStatus newStatus)
        {
            if (legToUpdate.Status == LegStatus.Cancelled || legToUpdate.Status == LegStatus.Rejected)
            {
                throw new DomainValidationException("Cannot change Leg status when it is cancelled or rejected.");
            }

            // Add other Status change validation logic here

            var oldStatus = legToUpdate.Status;

            legToUpdate.UpdateStatus(newStatus);

            if (legToUpdate.HasStatusChanged)
            {
                AddEvent(new DeliveryPlanLegStatusChangedEvent(this, legToUpdate.DeliveryLegId, oldStatus, newStatus, DeliveryPlanId));
            }
        }

        public void RemoveLegsNotInList(IEnumerable<Guid> deliveryPlanLegIds)
        {
            if (deliveryPlanLegIds.ToList().Count > 0)
            {
                var legIdsToRemove = _legs.Where(x => !deliveryPlanLegIds.Contains(x.DeliveryLegId)).Select(x => x.DeliveryLegId).ToList();

                foreach (var legId in legIdsToRemove)
                {
                    RemoveLeg(legId);
                }
            }
        }

        public void RemoveLeg(Guid deliveryPlanlegId)
        {
            var legToRemove = _legs.Find(x => x.DeliveryLegId == deliveryPlanlegId);

            if (legToRemove.Status == LegStatus.Accepted)
            {
                throw new DomainValidationException("An accepted Leg cannot be removed");
            }

            _legs.Remove(legToRemove);

            SortLegsByPickUpDate();

            AddEvent(new DeliveryPlanLegRemovedEvent(this, legToRemove, DeliveryPlanId));
        }

        public IEnumerable<ProviderLegGroup> GetLegsGroupedByProviderCode()
        {
            return _legs.GroupBy(
                x => x.ProviderCode,
                x => x,
                (key, leg) => new ProviderLegGroup(
                    key, 
                    leg.Select(y => new ProviderLeg()
                    {
                        PickUpDate = y.PickUpDate,
                        DropOffDate = y.DropOffDate,
                        StartLocation = y.StartLocation,
                        EndLocation = y.EndLocation
                    }).ToList()));
        }

        private void SortLegsByPickUpDate()
        {
            if (_legs.Any())
            {
                _legs = _legs.OrderBy(x => x.PickUpDate).ToList();

                var legCount = _legs.Count;

                for (int i = 0; i < legCount; i++)
                {
                    _legs[i].SetSequence(i + 1);
                }
            }
        }

        private void ValidateSortedLegs()
        {
            foreach (var leg in _legs)
            {
                foreach (var validator in _legValidatorConfiguration.Validators)
                {
                    var legValidation = validator.Validate(this, leg);

                    if (!legValidation.IsValid)
                    {
                        throw new DomainValidationException(legValidation.ErrorMessage);
                    }
                }
            }
        }

        public void When(DeliveryPlanCreatedEvent @event)
        {
            DeliveryPlanId = @event.DeliveryPlan.DeliveryPlanId;
            CargoStuffingId = @event.DeliveryPlan.CargoStuffingId;
            Shipment = (Shipment)@event.DeliveryPlan.Shipment.GetCopy();
            Status = @event.DeliveryPlan.Status;

            foreach (var leg in @event.DeliveryPlan.Legs)
            {
                _legs.Add(new DeliveryPlanLeg(
                    leg.DeliveryLegId,
                    leg.ProviderCode,
                    leg.PickUpDate,
                    leg.DropOffDate,
                    (Location)leg.StartLocation.GetCopy(),
                    (Location)leg.EndLocation.GetCopy(),
                    leg.Status));
            }
        }

        public void When(DeliveryPlanLegAddedEvent @event)
        {
            _legs.Add(new DeliveryPlanLeg(
                    @event.DeliveryPlanLeg.DeliveryLegId,
                    @event.DeliveryPlanLeg.ProviderCode,
                    @event.DeliveryPlanLeg.PickUpDate,
                    @event.DeliveryPlanLeg.DropOffDate,
                    (Location)@event.DeliveryPlanLeg.StartLocation.GetCopy(),
                    (Location)@event.DeliveryPlanLeg.EndLocation.GetCopy(),
                    @event.DeliveryPlanLeg.Status));
        }

        public void When(DeliveryPlanLegUpdatedEvent @event)
        {
            var leg = _legs.Find(x => x.DeliveryLegId == @event.DeliveryPlanLeg.DeliveryLegId);

            leg.UpdateDetails(
                @event.DeliveryPlanLeg.PickUpDate,
                @event.DeliveryPlanLeg.DropOffDate,
                (Location)@event.DeliveryPlanLeg.StartLocation.GetCopy(),
                (Location)@event.DeliveryPlanLeg.EndLocation.GetCopy());
        }

        public void When(DeliveryPlanVesselDetailsUpdatedEvent @event)
        {
            Shipment.UpdateVessel((Vessel)@event.Vessel.GetCopy());
        }

        public void When(DeliveryPlanLegRemovedEvent @event)
        {
            var leg = _legs.Find(x => x.DeliveryLegId == @event.DeliveryPlanLeg.DeliveryLegId);

            _legs.Remove(leg);
        }

        public void When(DeliveryPlanLegStatusChangedEvent @event)
        {
            var leg = _legs.Find(x => x.DeliveryLegId == @event.DeliveryPlanLegId);

            leg.UpdateStatus(@event.NewStatus);
        }
    }
}
