using System;
using System.Linq;
using AppMGL.DAL.Helper;

namespace AppMGL.Manager.Infrastructure.Results
{
    public class PagedResult<T>
    {
        public int PageIndex { get; set; }

        public int PageSize { get; set; }

        public int PageCount { get; private set; }

        public long RecordCount { get; set; }

        public string Sort { get; set; }
        
        public string Filter { get; set; }
        
        public IQueryable<T> Items { get; set; }

        public PagedResult(IQueryable<T> items, ListParams listParams, long recordCount)
        {
            Items = items;
            PageIndex = listParams.PageIndex;
            PageSize = listParams.PageSize;
            PageCount = recordCount > 0 ? (int)Math.Ceiling(recordCount / (double)PageSize) : 0;
            RecordCount = recordCount;
            Sort = listParams.Sort;
            Filter = listParams.Filter;
        }
    }
}