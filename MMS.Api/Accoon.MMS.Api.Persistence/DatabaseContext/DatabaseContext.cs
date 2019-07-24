using Accoon.MMS.Api.Application.Interfaces.Database;
using Accoon.MMS.Api.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Accoon.MMS.Api.Persistence.DatabaseContext
{
    public class DefaultDatabaseContext : DbContext, IDatabaseContext
    {
        // constructor
        public DefaultDatabaseContext(DbContextOptions<DefaultDatabaseContext> options) : base(options)
        {

        }

        // database entities
        public DbSet<Customer> Customers { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        public DbSet<User> Users { get; set; }


        // register entity configurations
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(DefaultDatabaseContext).Assembly);
        }
    }
}

// Add migrations
// Add-Migration InitMigration -Project Accoon.MMS.Api.Persistence -StartupProject Accoon.MMS.Api.Presenter -Context DefaultDatabaseContext
// update-database -Project Accoon.MMS.Api.Persistence -StartupProject Accoon.MMS.Api.Presenter