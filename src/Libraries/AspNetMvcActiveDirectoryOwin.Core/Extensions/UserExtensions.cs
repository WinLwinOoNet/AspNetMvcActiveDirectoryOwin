using AspNetMvcActiveDirectoryOwin.Core.Domain;

namespace AspNetMvcActiveDirectoryOwin.Core.Extensions
{
    public static class UserExtensions
    {
        public static string FullName(this User user)
        {
            return !string.IsNullOrWhiteSpace(user.FirstName) ? $"{user.LastName}, {user.FirstName}" : user.LastName;
        }
    }
}
