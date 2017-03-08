using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using AspNetMvcActiveDirectoryOwin.Core.Data;
using AspNetMvcActiveDirectoryOwin.Core.Domain;
using AspNetMvcActiveDirectoryOwin.Data;

namespace AspNetMvcActiveDirectoryOwin.Services.Logging
{
    public class TraceLogService : ITraceLogService
    {
        private readonly IRepository<TraceLog> _traceLogRepository;

        public TraceLogService(IRepository<TraceLog> traceLogRepository)
        {
            _traceLogRepository = traceLogRepository;
        }

        public async Task<TraceLog> GetTraceLogById(int id)
        {
            var query = _traceLogRepository.Entities
                .Where(x => x.Id == id);

            return await query.FirstOrDefaultAsync();
        }

        public async Task<IPagedList<TraceLog>> GetTraceLogs(TraceLogPagedDataRequest request)
        {
            var query = _traceLogRepository.Entities.AsQueryable();

            if (!string.IsNullOrWhiteSpace(request.Controller))
                query = query.Where(x => x.Controller.Contains(request.Controller));

            if (!string.IsNullOrWhiteSpace(request.Action))
                query = query.Where(x => x.Action == request.Action);

            if (!string.IsNullOrWhiteSpace(request.Message))
                query = query.Where(x => x.Message.Contains(request.Message));

            if (!string.IsNullOrWhiteSpace(request.PerformedBy))
                query = query.Where(x => x.PerformedBy == request.PerformedBy);

            if (request.FromDate.HasValue)
                query = query.Where(x => x.PerformedOn >= request.FromDate.Value);

            if (request.ToDate.HasValue)
                query = query.Where(x => x.PerformedOn <= request.ToDate.Value);

            string orderBy = request.SortField.ToString();
            if (QueryHelper.PropertyExists<Log>(orderBy))
                query = request.SortOrder == SortOrder.Ascending ? query.OrderByProperty(orderBy) : query.OrderByPropertyDescending(orderBy);
            else
                query = query.OrderByDescending(x => x.PerformedOn);

            var result = new PagedList<TraceLog>();
            await result.CreateAsync(query, request.PageIndex, request.PageSize);
            return result;
        }

        public async Task<IList<string>> GetPerformedUsernames()
        {
            var query = _traceLogRepository.Entities
                .Select(t => t.PerformedBy)
                .OrderBy(t => t)
                .Distinct();

            return await query.ToListAsync();
        }
    }
}