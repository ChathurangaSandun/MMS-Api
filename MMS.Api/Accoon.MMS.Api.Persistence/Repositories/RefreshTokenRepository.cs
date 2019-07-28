using Accoon.MMS.Api.Application.Interfaces.Repositories;
using Accoon.MMS.Api.Domain.Entities;
using Accoon.MMS.Api.Persistence.DatabaseContext;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Accoon.MMS.Api.Persistence.Repositories
{
    public class RefreshTokenRepository : IRefreshTokenRepository
    {
        private readonly DefaultDatabaseContext defaultDatabaseContext;

        public RefreshTokenRepository(DefaultDatabaseContext defaultDatabaseContext)
        {
            this.defaultDatabaseContext = defaultDatabaseContext;
        }

        public async Task<RefreshToken> AddRefreshTokenAsync(RefreshToken refreshToken)
        { 
            await this.defaultDatabaseContext.AddAsync(refreshToken);
            await this.defaultDatabaseContext.SaveChangesAsync();
            return refreshToken;
        }
    }
}
