using System.Web.Mvc;
using System.Web.Routing;
using AspNetMvcActiveDirectoryOwin.Core;

namespace AspNetMvcActiveDirectoryOwin.Web
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("favicon.ico");
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            // Home page
            routes.MapRoute(
                name: "Home",
                url: "",
                defaults: new { controller = "Home", action = "Index" });

            // Dashboard
            routes.MapRoute(
                name: "Dashboard",
                url: "Administration",
                defaults: new { area = Constants.Areas.Administration, controller = "Dashboard", action = "Index" });

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
