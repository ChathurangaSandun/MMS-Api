using Accoon.MMS.Api.Domain.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Accoon.MMS.Api.Persistence.IdentityDatabaseContext
{
    public class AppIdentityDbContext : IdentityDbContext<AppUser>
    {
        public AppIdentityDbContext(DbContextOptions<AppIdentityDbContext> options) : base(options)
        {
        }
    }
}


// Add migrations

// Add-Migration InitMigration -Project Accoon.MMS.Api.Persistence -StartupProject Accoon.MMS.Api.Presenter -Context AppIdentityDbContext
// update-database -Project Accoon.MMS.Api.Persistence -StartupProject Accoon.MMS.Api.Presenter -Context AppIdentityDbContext
