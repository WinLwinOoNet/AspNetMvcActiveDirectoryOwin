namespace AspNetMvcActiveDirectoryOwin.Web.Common.Security
{
    public interface IActiveDirectoryService
    {
        bool ValidateCredentials(string domain, string userName, string password);

        User GetUser(string domain, string userName);
    }
}