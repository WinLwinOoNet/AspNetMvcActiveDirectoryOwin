using System.Collections.Generic;
using System.Threading.Tasks;
using AspNetMvcActiveDirectoryOwin.Core.Domain;

namespace AspNetMvcActiveDirectoryOwin.Services.Roles
{
    public interface IRoleService
    {
        Task<IList<Role>> GetAllRoles();

        Task<IList<Role>> GetRolesForUser(int userId);
    }
}