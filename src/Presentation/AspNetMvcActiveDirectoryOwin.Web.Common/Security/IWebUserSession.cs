using System;
using System.Security.Claims;
using System.Web;

namespace AspNetMvcActiveDirectoryOwin.Web.Common.Security
{
    public interface IUserSession
    {
        string FirstName { get; }
        string LastName { get; }
        string UserName { get; }
        string FullName { get; }
    }

    public interface IWebUserSession : IUserSession
    {
        Uri RequestUri { get; }
        string HttpRequestMethod { get; }
    }

    public class UserSession : IWebUserSession
    {
        public string FirstName => ((ClaimsPrincipal)HttpContext.Current.User)?.FindFirst(ClaimTypes.GivenName)?.Value;
        
        public string LastName => ((ClaimsPrincipal) HttpContext.Current.User)?.FindFirst(ClaimTypes.Surname)?.Value;

        public string UserName => ((ClaimsPrincipal)HttpContext.Current.User)?.FindFirst(ClaimTypes.Name)?.Value;

        public Uri RequestUri => HttpContext.Current.Request.Url;

        public string HttpRequestMethod => HttpContext.Current.Request.HttpMethod;
        
        public string FullName => string.IsNullOrWhiteSpace(FirstName) ? $"{LastName}, {FirstName}" : LastName;
    }
}
