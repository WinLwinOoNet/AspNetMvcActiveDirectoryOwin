using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using AspNetMvcActiveDirectoryOwin.Core.Data;
using AspNetMvcActiveDirectoryOwin.Core.Domain;

namespace AspNetMvcActiveDirectoryOwin.Services.Roles
{
    public class RoleService : IRoleService
    {
        private readonly IRepository<Role> _roleRepository;

        public RoleService(IRepository<Role> roleRepository)
        {
            _roleRepository = roleRepository;
        }

        public async Task<IList<Role>> GetAllRoles()
        {
            var query = _roleRepository.Entities;

            return await query.ToListAsync();
        }

        public async Task<IList<Role>> GetRolesForUser(int userId)
        {
            var query = _roleRepository.Entities
                .Where(r => r.Users.Any(u => u.Id == userId))
                .Select(r => r)
                .OrderBy(r => r.Name);

            return await query.ToListAsync();
        }
    }
}