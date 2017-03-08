using System.Web.Mvc;
using AspNetMvcActiveDirectoryOwin.Web.Common.Debugging;

namespace AspNetMvcActiveDirectoryOwin.Web.Controllers
{
    [TraceMvc]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}