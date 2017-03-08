using System.Collections.Generic;
using System.Threading.Tasks;
using AspNetMvcActiveDirectoryOwin.Core.Data;
using AspNetMvcActiveDirectoryOwin.Core.Domain;

namespace AspNetMvcActiveDirectoryOwin.Services.Domains
{
    public interface IDomainService
    {
        Task<IList<Domain>> GetAllDomainsAsync();

        Task<Domain> GetDomainByIdAsync(int userId);

        Task<int> AddDomainAsync(Domain user);

        Task UpdateDomainAsync(Domain user);
    }
}