using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using AspNetMvcActiveDirectoryOwin.Core;
using AspNetMvcActiveDirectoryOwin.Core.Data;
using AspNetMvcActiveDirectoryOwin.Core.Domain;
using AspNetMvcActiveDirectoryOwin.Services.Domains;
using AspNetMvcActiveDirectoryOwin.Services.Logging;
using AspNetMvcActiveDirectoryOwin.Services.Messages;
using AspNetMvcActiveDirectoryOwin.Services.Roles;
using AspNetMvcActiveDirectoryOwin.Services.Users;
using AspNetMvcActiveDirectoryOwin.Web.Common.Debugging;
using AspNetMvcActiveDirectoryOwin.Web.Common.Mapper;
using AspNetMvcActiveDirectoryOwin.Web.Common.Models.Users;
using AspNetMvcActiveDirectoryOwin.Web.Common.Mvc.Alerts;
using AspNetMvcActiveDirectoryOwin.Web.Common.Security;
using Kendo.Mvc;
using Kendo.Mvc.UI;
using log4net;

namespace AspNetMvcActiveDirectoryOwin.Web.Areas.Administration.Controllers
{
    [TraceMvc]
    [Authorize(Roles = Constants.RoleNames.Developer)]
    public class UsersController : Controller
    {
        private readonly IUserService _userService;
        private readonly IDateTime _dateTime;
        private readonly IDomainService _domainService;
        private readonly IWebUserSession _webUserSession;
        private readonly IRoleService _roleService;
        private readonly IActiveDirectoryService _activeDirectoryService;
        private readonly IMessageService _messageService;
        private readonly ILog _log;

        public UsersController(
            IActiveDirectoryService activeDirectoryService,
            IDateTime dateTime,
            IDomainService domainService,
            ILogManager logManager, 
            IMessageService messageService,
            IRoleService roleService,
            IUserService userService,
            IWebUserSession webUserSession)
        {
            _userService = userService;
            _dateTime = dateTime;
            _domainService = domainService;
            _webUserSession = webUserSession;
            _roleService = roleService;
            _activeDirectoryService = activeDirectoryService;
            _messageService = messageService;
            _log = logManager.GetLog(typeof(UsersController));
        }

        public ActionResult Index()
        {
            return RedirectToAction("List");
        }

        public async Task<ActionResult> List()
        {
            var roleNames = await GetAvailableRoleNames();
            roleNames.Insert(0, new SelectListItem {Text = "All", Value = ""});
            var model = new UserSearchModel {AvailableRoles = roleNames, Status = ""};
            return View("List", model);
        }

        [HttpPost]
        public async Task<ActionResult> List([DataSourceRequest] DataSourceRequest request, UserSearchModel model)
        {
            var dataRequest = ParsePagedDataRequest(request, model);
            var entities = await _userService.GetUsersAsync(dataRequest);
            var models = entities.Select(user => user.ToModel()).ToList();
            var result = new DataSourceResult {Data = models, Total = entities.TotalCount};
            return Json(result);
        }

        [Authorize(Roles = Constants.RoleNames.Developer)]
            public async Task<ActionResult> Create()
        {
            var model = new UserCreateUpdateModel
            {
                AvailableRoleNames = await GetAvailableRoleNames(),
                AvailableDomains = await GetAvailableDomains(),
                Active = true
            };
            return View("Create", model);
        }

        [HttpPost]
        [Authorize(Roles = Constants.RoleNames.Developer)]
        public async Task<ActionResult> Create(UserCreateUpdateModel model)
        {
            if (ModelState.IsValid)
            {
                var checkingUser = await _userService.GetUserByUserNameAsync(model.UserName);
                if (checkingUser != null)
                    return RedirectToAction("List").WithError($"User with same username {model.UserName} alredy exists.");

                var user = new User
                {
                    UserName = model.UserName.ToLowerInvariant(),
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Active = model.Active,
                    LastLoginDate = _dateTime.Now,
                    CreatedBy = _webUserSession.UserName,
                    CreatedOn = _dateTime.Now,
                    ModifiedBy = _webUserSession.UserName,
                    ModifiedOn = _dateTime.Now
                };

                bool isDeveloper = _webUserSession.IsInRole(Constants.RoleNames.Developer);
                var allRoles = await _roleService.GetAllRoles();
                foreach (var role in allRoles)
                {
                    // Only developer can add developer role.
                    if (role.Name == Constants.RoleNames.Developer && !isDeveloper)
                        continue;

                    if (model.SelectedRoleIds.Any(r => r == role.Id))
                        user.Roles.Add(role);
                }
                await _userService.AddUserAsync(user);
                await _messageService.SendAddNewUserNotification(user);
                return RedirectToAction("List").WithSuccess($"{user.FirstName}'s account was created successfully.");
            }

            // If we got this far, something failed, redisplay form
            model.AvailableRoleNames =  await GetAvailableRoleNames();
            return View("Create", model);
        }

        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
                return RedirectToAction("List");

            var user = await _userService.GetUserByIdAsync(id.Value);
            if (user == null)
                return RedirectToAction("List");

            var model = user.ToCreateUpdateModel();
            model.AvailableRoleNames = await GetAvailableRoleNames();
            model.AvailableDomains = await GetAvailableDomains();
            return View("Edit", model);
        }

        [HttpPost]
        public async Task<ActionResult> Edit(UserCreateUpdateModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userService.GetUserByIdAsync(model.Id);
                if (user == null)
                {
                    return RedirectToAction("List").WithError("Please select a user.");
                }

                user.FirstName = model.FirstName;
                user.LastName = model.LastName;
                user.Active = model.Active;
                user.ModifiedOn = _dateTime.Now;
                user.ModifiedBy = _webUserSession.UserName;

                bool isDeveloper = _webUserSession.IsInRole(Constants.RoleNames.Developer);
                var allRoles = await _roleService.GetAllRoles();
                foreach (var role in allRoles)
                {
                    // Only developer can add developer role.
                    if (role.Name == Constants.RoleNames.Developer && !isDeveloper)
                        continue;

                    if (model.SelectedRoleIds.Any(r => r == role.Id))
                    {
                        if (user.Roles.All(r => r.Id != role.Id))
                            user.Roles.Add(role);
                    }
                    else
                    {
                        if (user.Roles.Any(r => r.Id == role.Id))
                            user.Roles.Remove(role);
                    }
                }

                await _userService.UpdateUserAsync(user);
                return RedirectToAction("List").WithSuccess($"{user.FirstName}'s account was updated successfully.");
            }
            // If we got this far, something failed, redisplay form
            model.AvailableRoleNames = await GetAvailableRoleNames();
            model.AvailableDomains = await GetAvailableDomains();
            return View("Edit", model);
        }

        private UserPagedDataRequest ParsePagedDataRequest(DataSourceRequest request, UserSearchModel model)
        {
            var dataRequest = new UserPagedDataRequest
            {
                LastName = model.LastName,
                PageIndex = request.Page - 1,
                PageSize = request.PageSize
            };

            switch (model.Status)
            {
                case "1":
                    dataRequest.Active = true;
                    break;
                case "0":
                    dataRequest.Active = false;
                    break;
            }

            SortDescriptor sort = request.Sorts.FirstOrDefault();
            if (sort != null)
            {
                UserSortField sortField;
                Enum.TryParse(sort.Member, out sortField);
                dataRequest.SortField = sortField;

                dataRequest.SortOrder = sort.SortDirection == ListSortDirection.Ascending
                    ? SortOrder.Ascending
                    : SortOrder.Descending;
            }

            return dataRequest;
        }

        private async Task<IList<SelectListItem>> GetAvailableRoleNames()
        {
            var roles = await _roleService.GetAllRoles();
            var roleNames = new List<SelectListItem>();
            var isDeveloper = _webUserSession.IsInRole(Constants.RoleNames.Developer);
            foreach (var role in roles)
            {
                if (role.Name == Constants.RoleNames.Developer)
                {
                    if (isDeveloper)
                        roleNames.Add(new SelectListItem {Text = role.Name, Value = role.Id.ToString()});
                }
                else
                    roleNames.Add(new SelectListItem {Text = role.Name, Value = role.Id.ToString()});
            }

            return roleNames;
        }

        private async Task<IList<SelectListItem>> GetAvailableDomains()
        {
            return (await _domainService.GetAllDomainsAsync())
                .Select(d => new SelectListItem {Text = d.Name, Value = d.Name })
                .ToList();
        }

        [HttpGet]
        public JsonResult GetUserFromAd(string domain, string username)
        {
            User user = null;
            try
            {
                user = _activeDirectoryService.GetUserFromAd(domain, username);
            }
            catch (Exception ex)
            {
                // This is a very rare case that we cannot query Debbi's information from AD.
                _log.Error(ex);
            }
            return Json(user, JsonRequestBehavior.AllowGet);
        }
    }
}