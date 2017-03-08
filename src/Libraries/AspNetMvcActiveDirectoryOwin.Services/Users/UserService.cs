using System;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Threading.Tasks;
using AspNetMvcActiveDirectoryOwin.Core.Data;
using AspNetMvcActiveDirectoryOwin.Core.Domain;
using AspNetMvcActiveDirectoryOwin.Data;

namespace AspNetMvcActiveDirectoryOwin.Services.Users
{
    public class UserService : IUserService
    {
        private readonly IRepository<User> _repository;

        public UserService(IRepository<User> repository)
        {
            _repository = repository;
        }

        public async Task<IPagedList<User>> GetUsersAsync(UserPagedDataRequest request)
        {
            var query = _repository.Entities.AsQueryable();

            if (!string.IsNullOrWhiteSpace(request.LastName))
                query = query.Where(x => x.LastName.StartsWith(request.LastName));

            if (!string.IsNullOrWhiteSpace(request.RoleName))
                query = query.Where(x => x.Roles.Any(r => r.Name == request.RoleName));

            if (request.Active.HasValue)
                query = query.Where(x => x.Active == request.Active.Value);

            string orderBy = request.SortField.ToString();
            if (QueryHelper.PropertyExists<User>(orderBy))
                query = request.SortOrder == SortOrder.Ascending ? query.OrderByProperty(orderBy) : query.OrderByPropertyDescending(orderBy);
            else
                query = query.OrderBy(x => x.LastName);

            var result = new PagedList<User>();
            await result.CreateAsync(query, request.PageIndex, request.PageSize);
            return result;
        }

        public async Task<User> GetUserByUserNameAsync(string userName)
        {
            var query = _repository.Entities
                .Where(x => x.UserName == userName);

            return await query.FirstOrDefaultAsync();
        }

        public async Task<User> GetUserByIdAsync(int userId)
        {
            var query = _repository.Entities
                .Where(x => x.Id == userId);

            return await query.FirstOrDefaultAsync();
        }

        public async Task<int> AddUserAsync(User user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            _repository.Entities.Add(user);
            await _repository.SaveChangesAsync();

            return user.Id;
        }

        public async Task UpdateUserAsync(User user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            _repository.Entities.AddOrUpdate(user);
            await _repository.SaveChangesAsync();
        }
    }
}