using Accoon.MMS.Api.Persistence.DatabaseContext;
using Microsoft.EntityFrameworkCore.Design;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace Accoon.MMS.Api.Persistence.Infastructure
{
    public class DatabaseContextFactory : IDesignTimeDbContextFactory<DefaultDatabaseContext>
    {
        public DatabaseContextFactory()
        {
            Debugger.Launch();
        }

        public DefaultDatabaseContext CreateDbContext(string[] args)
        {
            var currentDirentory = Path.Combine(Directory.GetCurrentDirectory());
            Console.WriteLine(currentDirentory);
            var resolver = new DependencyResolver
            {
                CurrentDirectory = currentDirentory

            };

            return resolver.ServiceProvider.GetService(typeof(DefaultDatabaseContext)) as DefaultDatabaseContext;
        }
    }
}
