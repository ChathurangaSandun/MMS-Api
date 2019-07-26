using Accoon.MMS.Api.Persistence.IdentityDatabaseContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace Accoon.MMS.Api.Persistence.Infastructure
{
    public class AppIdentityDbContextFactory : IDesignTimeDbContextFactory<AppIdentityDbContext>
    {

        public AppIdentityDbContextFactory()
        {
            //Debugger.Launch();
        }
        public AppIdentityDbContext CreateDbContext(string[] args)
        {
            var currentDirentory = Path.Combine(Directory.GetCurrentDirectory());
            Console.WriteLine(currentDirentory);
            var resolver = new IdentityDependencyResolver
            {
                CurrentDirectory = currentDirentory

            };

            return resolver.ServiceProvider.GetService(typeof(AppIdentityDbContext)) as AppIdentityDbContext;
        }
    }
}
