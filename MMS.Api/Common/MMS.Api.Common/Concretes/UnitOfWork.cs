using Microsoft.EntityFrameworkCore;
using MMS.Api.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace MMS.Api.Common.Concretes
{
    public class UnitOfWork<TDbContext> : IUnitOfWork<TDbContext> where TDbContext : DbContext
    {
        private readonly TDbContext dbContext;
        public UnitOfWork(TDbContext context)
        {
            this.dbContext = context;
        }

        public int Commit()
        {
            this.dbContext.SaveChanges();
            return 1;
        }

        public void Dispose()
        {
            this.dbContext.Dispose();
        }
    }
}
