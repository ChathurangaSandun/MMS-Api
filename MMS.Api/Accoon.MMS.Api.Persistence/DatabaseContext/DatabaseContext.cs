using Accoon.MMS.Api.Application.Interfaces.Database;
using Accoon.MMS.Api.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

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
            modelBuilder.Entity<User>(ConfigureUser);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(DefaultDatabaseContext).Assembly);
        }

        public void ConfigureUser(EntityTypeBuilder<User> builder)
        {
            var navigation = builder.Metadata.FindNavigation(nameof(User.RefreshTokens));
            //EF access the RefreshTokens collection property through its backing field
            navigation.SetPropertyAccessMode(PropertyAccessMode.Field);

            builder.Ignore(b => b.Email);
            builder.Ignore(b => b.PasswordHash);
        }
    }
}

// Add migrations
// Add-Migration InitMigration -Project Accoon.MMS.Api.Persistence -StartupProject Accoon.MMS.Api.Presenter -Context DefaultDatabaseContext 
// update-database -Project Accoon.MMS.Api.Persistence -StartupProject Accoon.MMS.Api.Presenter -Context DefaultDatabaseContext 


//https://fullstackmark.com/post/19/jwt-authentication-flow-with-refresh-tokens-in-aspnet-core-web-api