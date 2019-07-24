using Accoon.MMS.Api.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Accoon.MMS.Api.Application.Interfaces.Database
{
    /// <summary>
    /// 
    /// </summary>
    public interface IDatabaseContext
    {
        // define dbsets in database
        DbSet<Customer> Customers { get; set; }

        // save database changes  
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
