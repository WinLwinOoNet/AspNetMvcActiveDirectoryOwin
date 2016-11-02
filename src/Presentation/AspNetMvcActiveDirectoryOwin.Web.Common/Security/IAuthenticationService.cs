namespace AspNetMvcActiveDirectoryOwin.Web.Common.Security
{
    public interface IAuthenticationService
    {
        void SignIn(User user);
        void SignOut();
    }
}
