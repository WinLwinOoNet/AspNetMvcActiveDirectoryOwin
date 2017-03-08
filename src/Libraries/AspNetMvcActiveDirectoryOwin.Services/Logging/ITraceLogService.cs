using System.Collections.Generic;
using System.Threading.Tasks;
using AspNetMvcActiveDirectoryOwin.Core.Data;
using AspNetMvcActiveDirectoryOwin.Core.Domain;

namespace AspNetMvcActiveDirectoryOwin.Services.Logging
{
    public interface ITraceLogService
    {
        Task<TraceLog> GetTraceLogById(int id);
        
        Task<IPagedList<TraceLog>> GetTraceLogs(TraceLogPagedDataRequest request);

        Task<IList<string>> GetPerformedUsernames();
    }
}