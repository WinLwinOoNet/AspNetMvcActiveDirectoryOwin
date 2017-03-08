using System.DirectoryServices.AccountManagement;
using AspNetMvcActiveDirectoryOwin.Core;
using AspNetMvcActiveDirectoryOwin.Core.Domain;

namespace AspNetMvcActiveDirectoryOwin.Web.Common.Security
{
    public class ActiveDirectoryService : IActiveDirectoryService
    {
        public bool ValidateCredentials(string domain, string userName, string password)
        {
            userName = userName.EnsureNotNull();
            userName = userName.Trim();

            password = password.EnsureNotNull();
            password = password.Trim();

            using (var context = new PrincipalContext(ContextType.Domain, domain))
            {
                return context.ValidateCredentials(userName, password);
            }
        }

        public User GetUserFromAd(string domain, string userName)
        {
            User result = null;
            userName = userName.EnsureNotNull();
            userName = userName.Trim();

            using (var context = new PrincipalContext(ContextType.Domain, domain))
            {
                var user = UserPrincipal.FindByIdentity(context, userName);
                if (user != null)
                {
                    result = new User
                    {
                        Id = 0,
                        UserName = userName,
                        FirstName = user.GivenName,
                        LastName = user.Surname,
                        Active = true
                    };
                }
            }
            return result;
        }
    }
}