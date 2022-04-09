using Dapper;
using Maersk.SCM.Framework.Core.Common;
using Microsoft.Extensions.Options;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace Maersk.SCM.DeliveryPlanning.Application.Queries
{
    public class DeliveryPlanSqlQueries : IDeliveryPlanQueries
    {
        private ConnectionStrings _connectionStrings;

        public DeliveryPlanSqlQueries(IOptions<ConnectionStrings> options)
        {
            _connectionStrings = options.Value;
        }

        public async Task<dynamic> GetDeliveryPlanAsync(Guid id)
        {
            using (var connection = new SqlConnection(_connectionStrings.DefaultConnection))
            {
                var queryResults = await connection.QueryMultipleAsync(
                    @"SELECT    [DeliveryPlanId]
                                ,[CargoStuffingId]
                                ,[ContainerType]
                                ,[ContainerReference]
                                ,[VesselName]
                                ,[VesselType]
                                ,[Status]
                      FROM      [dbo].[DeliveryPlan]
                      WHERE     [DeliveryPlanId] = @DeliveryPlanId

                      SELECT     [DeliveryPlanLegId]
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
                      FROM      [dbo].[DeliveryPlanLeg]
                      WHERE     [DeliveryPlanId] = @DeliveryPlanId
                    ",  
                    new
                    {
                        DeliveryPlanId = id
                    },
                    commandType: CommandType.Text);

                var deliveryPlan = await queryResults.ReadFirstAsync<dynamic>();
                var deliveryPlanLegs = await queryResults.ReadAsync<dynamic>();
                deliveryPlan.Legs = deliveryPlanLegs;

                return deliveryPlan;
            }
        }

        public async Task<dynamic> GetDeliveryPlansSummaryAsync(DeliveryPlanFilter filter)
        {
            using (var connection = new SqlConnection(_connectionStrings.DefaultConnection))
            {
                return await connection.QueryAsync<dynamic>(
                    @"SELECT    [DeliveryPlanId],
	                            [ContainerReference],
	                            [ContainerName],
	                            [StatusId],
	                            [StatusDescription],
	                            [VesselName],
	                            [EstimatedTimeOfArrival]
                      FROM      [dbo].[DeliveryPlanView]
                      WHERE     [EstimatedTimeOfArrival] BETWEEN @ETAFrom AND @ETATo",
                    filter,
                    commandType: CommandType.Text);
            }
        }
    }
}
