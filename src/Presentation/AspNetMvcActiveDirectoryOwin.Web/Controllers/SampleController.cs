using System.Web.Mvc;
using AspNetMvcActiveDirectoryOwin.Web.Common.Debugging;

namespace AspNetMvcActiveDirectoryOwin.Web.Controllers
{
    [TraceMvc]
    public class SampleController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}