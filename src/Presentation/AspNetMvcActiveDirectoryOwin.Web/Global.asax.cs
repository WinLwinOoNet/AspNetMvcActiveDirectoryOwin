using System.Security.Claims;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace AspNetMvcActiveDirectoryOwin.Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            // We use Owin Cookie Authentication, so we set AntiForgery token to use Name as claim types.
            // If you use different claim type, you need to synchronize with 
            // AspNetMvcActiveDirectoryOwin.Web.Common.Security.OwinAuthenticationService.
            AntiForgeryConfig.UniqueClaimTypeIdentifier = ClaimTypes.Name;

            AreaRegistration.RegisterAllAreas();
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }
    }
}
