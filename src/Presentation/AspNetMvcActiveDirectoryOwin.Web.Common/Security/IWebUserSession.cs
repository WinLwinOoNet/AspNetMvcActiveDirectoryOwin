using System;
using System.Security.Claims;
using System.Web;

namespace AspNetMvcActiveDirectoryOwin.Web.Common.Security
{
    public interface IUserSession
    {
        int Id { get; }
        string FirstName { get; }
        string LastName { get; }
        string UserName { get; }
        string FullName { get; }
        bool IsInRole(string roleName);
    }

    public interface IWebUserSession : IUserSession
    {
        Uri RequestUri { get; }
        string HttpRequestMethod { get; }
    }

    public class UserSession : IWebUserSession
    {
        public int Id => Convert.ToInt32(((ClaimsPrincipal) HttpContext.Current.User)?.FindFirst(ClaimTypes.Sid)?.Value);

        public string FirstName => ((ClaimsPrincipal) HttpContext.Current.User)?.FindFirst(ClaimTypes.GivenName)?.Value;

        public string LastName => ((ClaimsPrincipal) HttpContext.Current.User)?.FindFirst(ClaimTypes.Surname)?.Value;

        public string UserName => ((ClaimsPrincipal) HttpContext.Current.User)?.FindFirst(ClaimTypes.Name)?.Value;

        public bool IsInRole(string roleName) => HttpContext.Current.User.IsInRole(roleName);

        public Uri RequestUri => HttpContext.Current.Request.Url;

        public string HttpRequestMethod => HttpContext.Current.Request.HttpMethod;

        public string FullName => string.IsNullOrWhiteSpace(FirstName) ? $"{LastName}, {FirstName}" : LastName;
    }
}
