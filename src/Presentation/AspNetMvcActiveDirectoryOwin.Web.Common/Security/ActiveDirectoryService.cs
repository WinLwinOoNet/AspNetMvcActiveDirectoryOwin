using System.DirectoryServices.AccountManagement;

namespace AspNetMvcActiveDirectoryOwin.Web.Common.Security
{
    public class ActiveDirectoryService : IActiveDirectoryService
    {
        public bool ValidateCredentials(string domain, string userName, string password)
        {
            using (var context = new PrincipalContext(ContextType.Domain, domain))
            {
                return context.ValidateCredentials(userName, password);
            }
        }

        public User GetUser(string domain, string userName)
        {
            User result = null;
            using (var context = new PrincipalContext(ContextType.Domain, domain))
            {
                var user = UserPrincipal.FindByIdentity(context, userName);
                if (user != null)
                {
                    result = new User
                    {
                        UserName = userName,
                        FirstName = user.GivenName,
                        LastName = user.Surname
                    };
                }
            }
            return result;
        }
    }
}