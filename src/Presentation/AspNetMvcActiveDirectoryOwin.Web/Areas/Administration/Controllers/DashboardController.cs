using System.Web.Mvc;
using AspNetMvcActiveDirectoryOwin.Core;
using AspNetMvcActiveDirectoryOwin.Web.Common.Debugging;

namespace AspNetMvcActiveDirectoryOwin.Web.Areas.Administration.Controllers
{
    [TraceMvc]
    [Authorize(Roles = Constants.RoleNames.Developer + "," +
                       Constants.RoleNames.ApplicationManager + "," +
                       Constants.RoleNames.User)]
    public class DashboardController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}