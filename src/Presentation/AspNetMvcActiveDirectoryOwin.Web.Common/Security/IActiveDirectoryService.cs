using AspNetMvcActiveDirectoryOwin.Core.Domain;

namespace AspNetMvcActiveDirectoryOwin.Web.Common.Security
{
    public interface IActiveDirectoryService
    {
        bool ValidateCredentials(string domain, string userName, string password);

        User GetUserFromAd(string domain, string userName);
    }
}