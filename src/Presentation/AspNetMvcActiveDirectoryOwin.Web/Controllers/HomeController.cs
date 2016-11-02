using System.Web.Mvc;
using AspNetMvcActiveDirectoryOwin.Web.Common.Security;
using AspNetMvcActiveDirectoryOwin.Web.Models;

namespace AspNetMvcActiveDirectoryOwin.Web.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly IWebUserSession _webUserSession;

        public HomeController(IWebUserSession webUserSession)
        {
            _webUserSession = webUserSession;
        }

        // GET: Home
        public ActionResult Index()
        {
            var model = new HomeModel
            {
                UserName = _webUserSession.UserName,
                FirstName = _webUserSession.FirstName,
                LastName = _webUserSession.LastName
            };
            return View(model);
        }
    }
}