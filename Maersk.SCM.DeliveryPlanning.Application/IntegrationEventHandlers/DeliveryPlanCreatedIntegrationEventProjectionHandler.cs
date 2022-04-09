using Dapper;
using Maersk.SCM.DeliveryPlanning.Application.IntegrationEvents;
using Maersk.SCM.Framework.Core.Common;
using Maersk.SCM.Framework.Core.Messaging;
using Microsoft.Extensions.Options;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Maersk.SCM.DeliveryPlanning.Application.IntegrationEventHandlers
{
    public class DeliveryPlanCreatedIntegrationEventProjectionHandler : IIntegrationEventHandler<DeliveryPlanCreatedIntegrationEvent>
    {
        private ConnectionStrings _connectionStrings;

        public DeliveryPlanCreatedIntegrationEventProjectionHandler(IOptions<ConnectionStrings> options)
        {
            _connectionStrings = options.Value;
        }

        public async Task Handle(DeliveryPlanCreatedIntegrationEvent @event)
        {
            var containerInfo = GetContainerInfo();

            using (var connection = new SqlConnection(_connectionStrings.DefaultConnection))
            {
                connection.Open();

                using (var transaction = connection.BeginTransaction())
                {
                    await InsertDeliveryPlanView(@event, containerInfo, connection, transaction);
                    await InsertDeliveryPlan(@event, connection, transaction);
                    await InsertDeliveryPlanLegs(@event, connection, transaction);

                    transaction.Commit();
                }
            }
        }

        
        private static async Task InsertDeliveryPlanView(DeliveryPlanCreatedIntegrationEvent @event, dynamic containerInfo, SqlConnection connection, SqlTransaction transaction)
        {
            await connection.ExecuteAsync(@"
                    INSERT INTO     [dbo].[DeliveryPlanView]
                    (
                        [DeliveryPlanId],
	                    [ContainerReference],
	                    [ContainerName],
	                    [StatusId],
	                    [StatusDescription],
	                    [VesselName],
	                    [EstimatedTimeOfArrival]
                    )
                    VALUES
                    (
                        @DeliveryPlanId,
                        @ContainerReference,
                        @ContainerName,
                        @StatusId,
                        @StatusDescription,
                        @VesselName,
                        @EstimatedTimeOfArrival
                    )",
                                new
                                {
                                    @event.DeliveryPlanId,
                                    @event.Shipment.ContainerReference,
                                    ContainerName = containerInfo.ContainerName,
                                    StatusId = @event.Status.Id,
                                    StatusDescription = @event.Status.Name,
                                    VesselName = @event.Shipment.Vessel.Name,
                                    EstimatedTimeOfArrival = containerInfo.ETA
                                },
                                transaction: transaction,
                                commandType: CommandType.Text);
        }

        private static async Task InsertDeliveryPlan(DeliveryPlanCreatedIntegrationEvent @event, SqlConnection connection, SqlTransaction transaction)
        {
            await connection.ExecuteAsync(@"
                     INSERT INTO [dbo].[DeliveryPlan]
                    (
                            [DeliveryPlanId]
                           ,[CargoStuffingId]
                           ,[ContainerType]
                           ,[ContainerReference]
                           ,[VesselName]
                           ,[VesselType]
                           ,[Status]
                    )
                    VALUES
                    (
                            @DeliveryPlanId
                           ,@CargoStuffingId
                           ,@ContainerType
                           ,@ContainerReference
                           ,@VesselName
                           ,@VesselType
                           ,@Status
                    )",
                                new
                                {
                                    @event.DeliveryPlanId,
                                    @event.CargoStuffingId,
                                    @event.Shipment.ContainerType,
                                    @event.Shipment.ContainerReference,
                                    VesselName = @event.Shipment.Vessel.Name,
                                    VesselType = @event.Shipment.Vessel.Type,
                                    Status = @event.Status.Name
                                },
                                transaction: transaction,
                                commandType: CommandType.Text);
        }

        private static async Task InsertDeliveryPlanLegs(DeliveryPlanCreatedIntegrationEvent @event, SqlConnection connection, SqlTransaction transaction)
        {
            var deliveryLegs = @event.Legs.Select(x => new
            {
                DeliveryPlanLegId = x.LegId,
                DeliveryPlanId = @event.DeliveryPlanId,
                ProviderCode = x.ProviderCode,
                PickUpDate = x.PickUpDate,
                DropOffDate = x.DropOffDate,
                StartLocationCountryCode = x.StartLocation.CountryCode,
                StartLocationSiteCode = x.StartLocation.SiteCode,
                StartLocationFullCountryName = x.StartLocation.FullCountryName,
                StartLocationFullSiteName = x.StartLocation.FullSiteName,
                EndLocationCountryCode = x.EndLocation.CountryCode,
                EndLocationSiteCode = x.EndLocation.SiteCode,
                EndLocationFullCountryName = x.EndLocation.FullCountryName,
                EndLocationFullSiteName = x.EndLocation.FullSiteName,
                Status = x.Status.Name
            });

            await connection.ExecuteAsync(@"
                     INSERT INTO [dbo].[DeliveryPlanLeg]
                     (
                            [DeliveryPlanLegId]
                           ,[DeliveryPlanId]
                           ,[ProviderCode]
                           ,[PickUpDate]
                           ,[DropOffDate]
                           ,[StartLocationCountryCode]
                           ,[StartLocationSiteCode]
                           ,[StartLocationFullCountryName]
                           ,[StartLocationFullSiteName]
                           ,[EndLocationCountryCode]
                           ,[EndLocationSiteCode]
                           ,[EndLocationFullCountryName]
                           ,[EndLocationFullSiteName]
                           ,[Status]
                     )
                     VALUES
                     (
                           @DeliveryPlanLegId
                           ,@DeliveryPlanId
                           ,@ProviderCode
                           ,@PickUpDate
                           ,@DropOffDate
                           ,@StartLocationCountryCode
                           ,@StartLocationSiteCode
                           ,@StartLocationFullCountryName
                           ,@StartLocationFullSiteName
                           ,@EndLocationCountryCode
                           ,@EndLocationSiteCode
                           ,@EndLocationFullCountryName
                           ,@EndLocationFullSiteName
                           ,@Status
                     )",
                    deliveryLegs,
                    transaction: transaction,
                    commandType: CommandType.Text);
        }

        private dynamic GetContainerInfo()
        {
            // Call out to actual service to fetch additional container info if it's not available in the Event
            return new
            {
                ContainerName = "Some Container Name",
                ETA = DateTime.UtcNow.AddDays(-2)
            };
        }
    }
}
