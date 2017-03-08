using System.Threading.Tasks;
using AspNetMvcActiveDirectoryOwin.Core.Data;
using AspNetMvcActiveDirectoryOwin.Core.Domain;

namespace AspNetMvcActiveDirectoryOwin.Services.Users
{
    public interface IUserService
    {
        Task<IPagedList<User>> GetUsersAsync(UserPagedDataRequest request);

        Task<User> GetUserByUserNameAsync(string userName);

        Task<User> GetUserByIdAsync(int userId);

        Task<int> AddUserAsync(User user);

        Task UpdateUserAsync(User user);
    }
}