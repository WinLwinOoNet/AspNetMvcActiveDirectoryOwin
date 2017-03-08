using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetMvcActiveDirectoryOwin.Core.Data
{
    [Serializable]
    public class PagedList<T> : List<T>, IPagedList<T> 
    {
        public PagedList()
        {
            AddRange(new List<T>());
            TotalCount = 0;
            TotalPages = 0;
            PageSize = 0;
            PageIndex = 0;
        }

        public PagedList(IQueryable<T> source, int pageIndex, int pageSize)
        {
            if (pageSize <= 0)
                pageSize = 1;

            int total = source.Count();
            TotalCount = total;
            TotalPages = total / pageSize;

            if (total % pageSize > 0)
                TotalPages++;

            PageSize = pageSize;
            PageIndex = pageIndex;
            AddRange(source.Skip(pageIndex * pageSize).Take(pageSize).ToList());
        }

        public PagedList(IEnumerable<T> source, int pageIndex, int pageSize, int totalCount)
        {
            TotalCount = totalCount;
            TotalPages = TotalCount / pageSize;

            if (TotalCount % pageSize > 0)
                TotalPages++;

            PageSize = pageSize;
            PageIndex = pageIndex;
            AddRange(source);
        }

        public async Task CreateAsync(IQueryable<T> source, int pageIndex, int pageSize)
        {
            if (pageSize <= 0)
                pageSize = 1;

            int total = await source.CountAsync();
            TotalCount = total;
            TotalPages = total / pageSize;

            if (total % pageSize > 0)
                TotalPages++;

            PageSize = pageSize;
            PageIndex = pageIndex;
            AddRange(await source.Skip(pageIndex * pageSize).Take(pageSize).ToListAsync());
        }

        public int PageIndex { get; private set; }

        public int PageSize { get; private set; }

        public int TotalCount { get; private set; }

        public int TotalPages { get; private set; }

        public bool HasPreviousPage => PageIndex > 0;

        public bool HasNextPage => PageIndex + 1 < TotalPages;
    }
}
