using Microsoft.EntityFrameworkCore;
using MMS.Api.DataAccessServices.Entities.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace MMS.Api.DataAccessServices.Entities
{
    public class MMSDbContext:  DbContext
    {
        public MMSDbContext(DbContextOptions<MMSDbContext> options)
          : base(options)
        { }

        
        public DbSet<CustomerEntity> Customers { get; set; }
    }
}

// Add-Migration NewMigration -Project Accoon.Api.DataServices.Entities -StartupProject Accoon.Api
// update-database -verbose
