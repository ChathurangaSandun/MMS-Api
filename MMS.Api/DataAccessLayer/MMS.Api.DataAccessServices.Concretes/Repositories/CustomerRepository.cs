using MMS.Api.Common.Concretes;
using MMS.Api.DataAccessServices.Entities;
using MMS.Api.DataAccessServices.Entities.Entities;
using MMS.Api.DataAccessServices.Interfaces.Repositories;
using System;

namespace MMS.Api.DataAccessServices.Concretes.Repositories
{
    public class CustomerRepository : RepositoryBase<MMSDbContext, CustomerEntity, Guid>, ICustomerRepository
    {
        public CustomerRepository(MMSDbContext dbContext) : base(dbContext)
        {
        }
    }
}
