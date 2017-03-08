using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using AspNetMvcActiveDirectoryOwin.Core;
using AspNetMvcActiveDirectoryOwin.Services.Messages;
using AspNetMvcActiveDirectoryOwin.Web.Common.Debugging;
using AspNetMvcActiveDirectoryOwin.Web.Common.Mapper;
using AspNetMvcActiveDirectoryOwin.Web.Common.Models.EmailTemplates;
using AspNetMvcActiveDirectoryOwin.Web.Common.Mvc.Alerts;
using AspNetMvcActiveDirectoryOwin.Web.Common.Security;
using Kendo.Mvc.UI;

namespace AspNetMvcActiveDirectoryOwin.Web.Areas.Administration.Controllers
{
    [TraceMvc]
    [Authorize(Roles = Constants.RoleNames.Developer)]
    public class EmailTemplatesController : Controller
    {
        private readonly IEmailTemplateService _emailTemplateService;
        private readonly IWebUserSession _webUserSession;
        private readonly IDateTime _dateTime;

        public EmailTemplatesController(
            IEmailTemplateService emailTemplateService,
            IWebUserSession webUserSession,
            IDateTime dateTime)
        {
            _emailTemplateService = emailTemplateService;
            _webUserSession = webUserSession;
            _dateTime = dateTime;
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
        public async Task<ActionResult> List([DataSourceRequest] DataSourceRequest request)
        {
            var entities = await _emailTemplateService.GetAllEmailTemplates();
            var models = entities.Select(e => e.ToModel()).ToList();
            var result = new DataSourceResult { Data = models, Total = models.Count };
            return Json(result);
        }

        public async Task<ActionResult> Edit(int id)
        {
            var emailTemplate = await _emailTemplateService.GetEmailTemplateById(id);

            if (emailTemplate == null)
                return RedirectToAction("List");

            var model = emailTemplate.ToModel();
            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> Edit(EmailTemplateModel model)
        {
            if (ModelState.IsValid)
            {
                var emailTemplate = await _emailTemplateService.GetEmailTemplateById(model.Id);

                if (emailTemplate == null)
                    return RedirectToAction("List");

                model.Body = HttpUtility.HtmlDecode(model.Body);
                model.Instruction = HttpUtility.HtmlDecode(model.Instruction);

                emailTemplate = model.ToEntity(emailTemplate);
                emailTemplate.ModifiedBy = _webUserSession.UserName;
                emailTemplate.ModifiedOn = _dateTime.Now;
                await _emailTemplateService.UpdateEmailTemplate(emailTemplate);
                return RedirectToAction("List").WithSuccess($"Email Template - {emailTemplate.Name} - was updated successfully.");
            }
            return View(model);
        }
    }
}