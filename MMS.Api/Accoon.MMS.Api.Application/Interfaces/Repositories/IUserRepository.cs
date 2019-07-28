using Accoon.MMS.Api.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Accoon.MMS.Api.Application.Interfaces.Repositories
{
    public interface IUserRepository
    {
        Task<User> AddUserAsync(User user);
    }
}
