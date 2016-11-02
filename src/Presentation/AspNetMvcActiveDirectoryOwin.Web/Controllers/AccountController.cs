using System.Web.Mvc;
using AspNetMvcActiveDirectoryOwin.Web.Common.Security;
using AspNetMvcActiveDirectoryOwin.Web.Models;

namespace AspNetMvcActiveDirectoryOwin.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly IActiveDirectoryService _activeDirectoryService;
        private readonly IAuthenticationService _authenticationService;

        public AccountController(
            IActiveDirectoryService activeDirectoryService,
            IAuthenticationService authenticationService)
        {
            _activeDirectoryService = activeDirectoryService;
            _authenticationService = authenticationService;
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                bool result = _activeDirectoryService.ValidateCredentials(model.Domain, model.UserName, model.Password);
                if (result)
                {
                    var user = _activeDirectoryService.GetUser(model.Domain, model.UserName);
                    if (user != null)
                    {
                        _authenticationService.SignIn(user);

                        if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                            return RedirectToLocal(returnUrl);

                        return RedirectToAction("Index", "Home");
                    }
                }
                ModelState.AddModelError("", "Incorrect username or password.");
            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Logout()
        {
            _authenticationService.SignOut();
            return RedirectToAction("Login", "Account");
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }
    }
}