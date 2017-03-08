using System.Web.Mvc;
using AspNetMvcActiveDirectoryOwin.Core;
using AspNetMvcActiveDirectoryOwin.Web.Common.Debugging;

namespace AspNetMvcActiveDirectoryOwin.Web.Areas.Administration.Controllers
{
    [TraceMvc]
    [Authorize(Roles = Constants.RoleNames.Developer + "," + Constants.RoleNames.ApplicationManager)]
    public class ReleaseHistoryController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}