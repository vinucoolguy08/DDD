using Maersk.SCM.DeliveryPlanning.Domain.Aggregates.DeliveryOrderAggregate;
using Maersk.SCM.DeliveryPlanning.Infrastructure.Repositories;
using Maersk.SCM.Framework.Core.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace Maersk.SCM.DeliveryPlanning.Infrastructure
{
    public class DeliveryPlanningDbContext : DbContext, IUnitOfWork
    { 
        public DeliveryPlanningDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<DeliveryPlanEventSourcedItems> DeliveryPlanEvents { get; set; }

        public DbSet<DeliveryOrderReference> DeliveryOrderReferences { get; set; }

        public DbSet<DeliveryOrderEventSourceItems> DeliveryOrderEvents { get; set; }

        public async Task<bool> SaveEntitiesAsync(CancellationToken cancellationToken = default)
        {
            await SaveChangesAsync();

            return true;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DeliveryPlanEventSourcedItems>()
               .HasKey(table => new
               {
                   table.Id,
                   table.Version
               });

            modelBuilder.Entity<DeliveryOrderEventSourceItems>()
               .HasKey(table => new
               {
                   table.Id,
                   table.Version
               });

            base.OnModelCreating(modelBuilder);
        }
    }
}
