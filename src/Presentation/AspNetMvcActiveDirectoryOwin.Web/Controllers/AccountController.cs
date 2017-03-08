using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using AspNetMvcActiveDirectoryOwin.Core;
using AspNetMvcActiveDirectoryOwin.Core.Extensions;
using AspNetMvcActiveDirectoryOwin.Services.Domains;
using AspNetMvcActiveDirectoryOwin.Services.Logging;
using AspNetMvcActiveDirectoryOwin.Services.Users;
using AspNetMvcActiveDirectoryOwin.Web.Common.Models.Account;
using AspNetMvcActiveDirectoryOwin.Web.Common.Security;
using log4net;

namespace AspNetMvcActiveDirectoryOwin.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly IActiveDirectoryService _activeDirectoryService;
        private readonly IAuthenticationService _authenticationService;
        private readonly IDomainService _domainService;
        private readonly IUserService _userService;
        private readonly IDateTime _dateTime;
        private readonly ILog _log;

        public AccountController(
            IActiveDirectoryService activeDirectoryService,
            IAuthenticationService authenticationService,
            IDomainService domainService,
            IDateTime dateTime,
            IUserService userService,
            ILogManager logManager)
        {
            _activeDirectoryService = activeDirectoryService;
            _authenticationService = authenticationService;
            _domainService = domainService;
            _userService = userService;
            _dateTime = dateTime;
            _log = logManager.GetLog(typeof(AccountController));
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult> Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            var model = new LoginModel {AvailableDomains = await GetDomains()};
            return View("Login", model);
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                bool result = _activeDirectoryService.ValidateCredentials(model.Domain, model.UserName, model.Password);
                if (result)
                {
                    var user = await _userService.GetUserByUserNameAsync(model.UserName);
                    if (user != null && user.Active)
                    {
                        var roleNames = user.Roles.Select(r => r.Name).ToList();
                        _authenticationService.SignIn(user, roleNames);

                        // Update LastLoginDate for future reference
                        user.LastLoginDate = _dateTime.Now;
                        await _userService.UpdateUserAsync(user);

                        _log.Info($"Login Successful: {user.UserName}");

                        // Redirect to return URL
                        if (!string.IsNullOrEmpty(returnUrl) && !string.Equals(returnUrl, "/") && Url.IsLocalUrl(returnUrl))
                            return RedirectToLocal(returnUrl);

                        // User is in a role, so redirect to Administration area
                        if (roleNames.Contains(Constants.RoleNames.Developer) ||
                            roleNames.Contains(Constants.RoleNames.ApplicationManager))
                            return RedirectToRoute("Dashboard");

                        return RedirectToAction("Index", "Home");
                    }
                    _log.Info($"Authorization Fail: {model.UserName}");
                    ModelState.AddModelError("", Constants.Messages.NotAuthorized);
                }
                else
                {
                    _log.Info($"Login Fail: {model.UserName}");
                    ModelState.AddModelError("", "Incorrect username or password.");
                }
            }
            model.AvailableDomains = await GetDomains();
            return View("Login", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Logout()
        {
            _log.Info($"'{User.Identity.Name}' is logged-out.");
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

        private async Task<IList<SelectListItem>> GetDomains()
        {
            var domains = await _domainService.GetAllDomainsAsync();

            return domains
                .Select(d => new SelectListItem {Text = d.Name, Value = d.Name})
                .ToList();
        }
    }
}