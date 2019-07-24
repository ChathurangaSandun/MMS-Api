using System;
using System.Collections.Generic;
using System.Text;

namespace Accoon.MMS.Api.Persistence.Infastructure
{
    public interface IEnvironmentService
    {
        string EnvironmentName { get; set; }
    }
}
