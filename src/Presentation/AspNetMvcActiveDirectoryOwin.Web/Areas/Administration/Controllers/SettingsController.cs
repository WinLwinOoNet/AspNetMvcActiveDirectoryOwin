using System.Linq;
using System.Web.Mvc;
using AspNetMvcActiveDirectoryOwin.Core;
using AspNetMvcActiveDirectoryOwin.Services.Settings;
using AspNetMvcActiveDirectoryOwin.Web.Common.Debugging;
using AspNetMvcActiveDirectoryOwin.Web.Common.Mapper;
using AspNetMvcActiveDirectoryOwin.Web.Common.Models.Settings;
using AspNetMvcActiveDirectoryOwin.Web.Common.Mvc.Alerts;
using Kendo.Mvc.UI;

namespace AspNetMvcActiveDirectoryOwin.Web.Areas.Administration.Controllers
{
    [TraceMvc]
    [Authorize(Roles = Constants.RoleNames.Developer)]
    public class SettingsController : Controller
    {
        private readonly ISettingService _settingService;

        public SettingsController(ISettingService settingService)
        {
            _settingService = settingService;
        }

        public ActionResult Index()
        {
            return RedirectToAction("List");
        }

        public ActionResult List()
        {
            return View();
        }

        [HttpPost]
        public JsonResult List([DataSourceRequest] DataSourceRequest request)
        {
            var entities = _settingService.GetAllSettings();
            var models = entities.Select(x => x.ToModel()).ToList();
            var result = new DataSourceResult { Data = models, Total = models.Count };
            return Json(result);
        }

        public ActionResult Edit(int id)
        {
            var setting = _settingService.GetSettingById(id);

            if (setting == null)
                return RedirectToAction("List");

            var model = setting.ToModel();
            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(SettingModel model)
        {
            if (ModelState.IsValid)
            {
                var entity = _settingService.GetSettingById(model.Id);

                if (entity == null)
                    return RedirectToAction("List");

                entity = model.ToEntity(entity);
                _settingService.UpdateSetting(entity);
                return RedirectToAction("List").WithSuccess($"Setting - {entity.Name} - was updated successfully.");
            }
            return View(model);
        }
    }
}