using System;
using System.Security.Claims;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using AspNetMvcActiveDirectoryOwin.Services.Logging;
using AspNetMvcActiveDirectoryOwin.Web.Common.Mapper;
using AspNetMvcActiveDirectoryOwin.Web.Controllers;
using AspNetMvcActiveDirectoryOwin.Web.Infrastructure;

namespace AspNetMvcActiveDirectoryOwin.Web
{
    public class Global : HttpApplication
    {
        protected void Application_Start()
        {
            EngineContext.Initialize();
            AutoMapperConfiguration.Initialize();

            ConfigureAntiForgeryTokens();

            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            var log = EngineContext.Current.Locator.GetInstance<ILogManager>().GetLog(typeof(Global));
            log.Info("Application started.");
        }

        private void ConfigureAntiForgeryTokens()
        {
            // We use Owin Cookie Authentication, so we set AntiForgery token to use Name as claim types.
            // If you use different claim type, you need to synchronize with 
            // AspNetMvcActiveDirectoryOwin.Web.Common.Security.OwinAuthenticationService.
            AntiForgeryConfig.UniqueClaimTypeIdentifier = ClaimTypes.Name;

            // Anti-Forgery cookie requires SSL to be sent across the wire. 
            //AntiForgeryConfig.RequireSsl = true;
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            var exception = Server.GetLastError();

            LogException(exception);

            if (exception is HttpAntiForgeryException)
            {
                Response.Clear();
                Server.ClearError();
                Response.TrySkipIisCustomErrors = true;

                // Call target Controller and pass the routeData.
                IController controller = EngineContext.Current.Locator.GetInstance<CommonController>();

                var routeData = new RouteData();
                routeData.Values.Add("controller", "Common");
                routeData.Values.Add("action", "AntiForgery");

                var requestContext = new RequestContext(new HttpContextWrapper(Context), routeData);
                controller.Execute(requestContext);
            }
            else
            {
                // Process 404 HTTP errors
                var httpException = exception as HttpException;
                if (httpException != null && httpException.GetHttpCode() == 404)
                {
                    Response.Clear();
                    Server.ClearError();
                    Response.TrySkipIisCustomErrors = true;

                    // Call target Controller and pass the routeData.
                    IController controller = EngineContext.Current.Locator.GetInstance<CommonController>();

                    var routeData = new RouteData();
                    routeData.Values.Add("controller", "Common");
                    routeData.Values.Add("action", "PageNotFound");

                    var requestContext = new RequestContext(new HttpContextWrapper(Context), routeData);
                    controller.Execute(requestContext);
                }
            }
        }

        private void LogException(Exception ex)
        {
            if (ex == null)
                return;

            // Ignore 404 HTTP errors
            var httpException = ex as HttpException;
            if (httpException != null &&
                httpException.GetHttpCode() == 404)
                return;

            try
            {
                var log = EngineContext.Current.Locator.GetInstance<ILogManager>().GetLog(typeof(Global));
                log.Error(ex);
            }
            catch (Exception)
            {
                // Don't throw new exception if occurs
            }
        }
    }
}
