using System.Threading.Tasks;
using AspNetMvcActiveDirectoryOwin.Core.Domain;

namespace AspNetMvcActiveDirectoryOwin.Web.Common.Debugging
{
    public interface ITraceListener
    {
        Task AddTraceLogAsync(TraceLog traceLog);
    }
}