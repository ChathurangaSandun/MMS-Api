using Accoon.MMS.Api.Application.Interfaces.Repositories;
using Accoon.MMS.Api.Domain.Entities;
using Accoon.MMS.Api.Persistence.DatabaseContext;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Accoon.MMS.Api.Persistence.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly DefaultDatabaseContext defaultDatabaseContext;

        public UserRepository(DefaultDatabaseContext defaultDatabaseContext)
        {
            this.defaultDatabaseContext = defaultDatabaseContext;
        }

       

        public async Task<User> AddUserAsync(User user)
        {
            await this.defaultDatabaseContext.AddAsync(user);
            await this.defaultDatabaseContext.SaveChangesAsync();
            return user;
        }
    }
}
