using System;
using System.Collections.Generic;
using System.Text;

namespace Accoon.MMS.Api.Persistence.Infastructure
{
    public class EnvironmentService : IEnvironmentService
    {
        public EnvironmentService()
        {
            EnvironmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")
                ?? "Production";
        }

        public string EnvironmentName { get; set; }
    }
}
