using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using AspNetMvcActiveDirectoryOwin.Core.Data;
using AspNetMvcActiveDirectoryOwin.Core.Domain;
using AspNetMvcActiveDirectoryOwin.Data;

namespace AspNetMvcActiveDirectoryOwin.Services.Logging
{
    public class LogService : ILogService
    {
        private readonly IRepository<Log> _repository;

        public LogService(IRepository<Log> repository)
        {
            _repository = repository;
        }

        public async Task<Log> GetLogById(int id)
        {
            var query = _repository.Entities
                .Where(x => x.Id == id);

            return await query.FirstOrDefaultAsync();
        }

        public async Task<IPagedList<Log>> GetLogs(LogPagedDataRequest request)
        {
            var query = _repository.Entities.AsQueryable();

            if (request.FromDate.HasValue)
                query = query.Where(x => x.Date >= request.FromDate.Value);

            if (request.ToDate.HasValue)
                query = query.Where(x => x.Date <= request.ToDate.Value);

            if (!string.IsNullOrWhiteSpace(request.Username))
                query = query.Where(x => x.Username.Contains(request.Username));

            if (!string.IsNullOrWhiteSpace(request.Thread))
                query = query.Where(x => x.Thread.Contains(request.Thread));

            if (!string.IsNullOrWhiteSpace(request.Level))
                query = query.Where(x => x.Level == request.Level);

            if (!string.IsNullOrWhiteSpace(request.Message))
                query = query.Where(x => x.Message.Contains(request.Message));

            if (!string.IsNullOrWhiteSpace(request.Exception))
                query = query.Where(x => x.Exception.Contains(request.Exception));

            string orderBy = request.SortField.ToString();
            if (QueryHelper.PropertyExists<Log>(orderBy))
                query = request.SortOrder == SortOrder.Ascending ? query.OrderByProperty(orderBy) : query.OrderByPropertyDescending(orderBy);
            else
                query = query.OrderByDescending(x => x.Date);

            var pagedList = new PagedList<Log>();
            await pagedList.CreateAsync(query, request.PageIndex, request.PageSize);
            return pagedList;
        }

        public async Task<IList<string>> GetUsernames()
        {
            var query = _repository.Entities
                .Select(t => t.Username)
                .OrderBy(t => t)
                .Distinct();

            return await query.ToListAsync();
        }
    }
}