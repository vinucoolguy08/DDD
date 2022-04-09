using Maersk.SCM.DeliveryPlanning.Domain.Aggregates.DeliveryPlanAggregate;
using Maersk.SCM.DeliveryPlanning.Domain.DomainServices;
using Maersk.SCM.DeliveryPlanning.Domain.DomainServices.DeliveryLegValidators;
using Maersk.SCM.DeliveryPlanning.Infrastructure;
using Maersk.SCM.DeliveryPlanning.Infrastructure.DomainServices;
using Maersk.SCM.DeliveryPlanning.Infrastructure.Repositories;
using Maersk.SCM.Framework.Core.Common;
using Maersk.SCM.Framework.Core.Persistence;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Maersk.SCM.Framework.Core.Messaging.AzureServiceBus;
using System;
using Maersk.SCM.DeliveryPlanning.Application.IntegrationEvents;
using Maersk.SCM.DeliveryPlanning.Domain.Aggregates.DeliveryOrderAggregate;
using Maersk.SCM.DeliveryPlanning.Application.IntegrationEventHandlers;
using Maersk.SCM.DeliveryPlanning.Application.Queries;

namespace Maersk.SCM.DeliveryPlanning.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var assembly = AppDomain.CurrentDomain.Load("Maersk.SCM.DeliveryPlanning.Application");
            services.AddMediatR(assembly);
            services.AddControllers();
            services.AddDbContext<DeliveryPlanningDbContext>(item => item.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Maersk.SCM.DeliveryPlanning.API", Version = "v1" });
            });

            AddDomainContextServices(services);
        }

        private void AddDomainContextServices(IServiceCollection services)
        {
            services.Configure<ConnectionStrings>(Configuration.GetSection("ConnectionStrings"));
            services.AddServiceBusTopicMessageBrokerFactory(Configuration);
            services.AddSingletonIntegrationEventHandler<DeliveryPlanCreatedIntegrationEvent, DeliveryPlanCreatedIntegrationEventProjectionHandler>();
            services.Configure<ConnectionStringSettings>(Configuration.GetSection(ConnectionStringSettings.SectionName));
            services.AddSingleton<ILocationService, MdmApiLocationService>();
            services.AddSingleton<ILegValidatorConfiguration, DefaultLegValidatorConfiguration>();
            services.AddSingleton<IDeliveryPlanFactory, DeliveryPlanFactory>();
            services.AddSingleton<IDeliveryOrderFactory, DeliveryOrderFactory>();
            services.AddScoped<IDeliveryOrderNumberGenerator, DefaultDeliveryOrderNumberGenerator>();
            services.AddScoped<IEventSourcedRepository<IDeliveryPlan>, DeliveryPlanSqlRepository>();
            services.AddScoped<IEventSourcedRepository<IDeliveryOrder>, DeliveryOrderSqlRepository>();
            services.AddSingleton<IDeliveryPlanQueries, DeliveryPlanSqlQueries>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Maersk.SCM.DeliveryPlanning.API v1"));
            }

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthorization();

            app.UseIntegrationEventHandlerSubscriber<DeliveryPlanCreatedIntegrationEvent>();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
