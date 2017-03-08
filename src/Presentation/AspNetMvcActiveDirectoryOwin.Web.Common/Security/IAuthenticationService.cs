using System.Collections.Generic;
using AspNetMvcActiveDirectoryOwin.Core.Domain;

namespace AspNetMvcActiveDirectoryOwin.Web.Common.Security
{
    public interface IAuthenticationService
    {
        void SignIn(User user, IList<string> roleNames);
        void SignOut();
    }
}
