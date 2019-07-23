using MMS.Api.Common.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace MMS.Api.Common.Pagination
{
    public class PaginationModel<TEntity, TPrimaryKey> where TEntity: Entity<TPrimaryKey> 
    {
        public int PageIndex { get; private set; }

        public int PageSize { get; private set; }

        public long Count { get; private set; }

        public IEnumerable<TEntity> Data { get;  set; }

        public PaginationModel(int pageIndex, int pageSize, long count, IEnumerable<TEntity> data)
        {
            this.PageIndex = pageIndex;
            this.PageSize = pageSize;
            this.Count = count;
            this.Data = data;
        }
    }
}
