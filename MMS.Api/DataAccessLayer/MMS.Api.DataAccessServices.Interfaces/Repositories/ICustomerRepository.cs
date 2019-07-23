
using MMS.Api.Common.Interfaces;
using MMS.Api.DataAccessServices.Entities;
using MMS.Api.DataAccessServices.Entities.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace MMS.Api.DataAccessServices.Interfaces.Repositories
{
    public interface ICustomerRepository : IRepository<MMSDbContext, CustomerEntity, Guid>
    {
    }
}
