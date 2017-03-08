using System.Collections.Generic;
using System.Threading.Tasks;
using AspNetMvcActiveDirectoryOwin.Core.Data;
using AspNetMvcActiveDirectoryOwin.Core.Domain;

namespace AspNetMvcActiveDirectoryOwin.Services.Logging
{
    public interface ILogService
    {
        Task<Log> GetLogById(int id);

        Task<IPagedList<Log>> GetLogs(LogPagedDataRequest request);

        Task<IList<string>> GetUsernames();
    }
}