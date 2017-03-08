using System.Data.Entity;
using System.Threading.Tasks;

namespace AspNetMvcActiveDirectoryOwin.Core.Data
{
    public interface IRepository<T> where T : BaseEntity
    {
        IDbSet<T> Entities { get; }
        Task<int> SaveChangesAsync();
    }
}
