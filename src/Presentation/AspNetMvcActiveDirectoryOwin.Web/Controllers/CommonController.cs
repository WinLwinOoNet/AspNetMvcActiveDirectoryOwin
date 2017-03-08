using System;
using System.Web.Mvc;
using AspNetMvcActiveDirectoryOwin.Services.Logging;
using AspNetMvcActiveDirectoryOwin.Web.Common.Security;
using log4net;

namespace AspNetMvcActiveDirectoryOwin.Web.Controllers
{
    public class CommonController : Controller
    {
        private readonly IWebUserSession _webUserSession;
        private readonly ILog _log;

        public CommonController(
            IWebUserSession webUserSession,
            ILogManager logManager)
        {
            _webUserSession = webUserSession;
            _log = logManager.GetLog(typeof(CommonController));
        }

        [AllowAnonymous]
        public ActionResult Error()
        {
            Response.StatusCode = 503;
            Response.TrySkipIisCustomErrors = true;

            return View();
        }

        [AllowAnonymous]
        public ActionResult PageNotFound()
        {
            Response.StatusCode = 404;
            Response.TrySkipIisCustomErrors = true;

            return View();
        }

        [AllowAnonymous]
        public ActionResult AdAccountNotFound()
        {
            Response.StatusCode = 503;
            Response.TrySkipIisCustomErrors = true;

            return View();
        }

        [AllowAnonymous]
        public ActionResult AntiForgery()
        {
            Response.TrySkipIisCustomErrors = true;

            return View();
        }

        public ActionResult AccessDenied(string pageUrl)
        {
            if (_webUserSession != null)
            {
                _log.Info($"Access denied to user '{_webUserSession.UserName}' on {pageUrl}");
                return View();
            }
            _log.Info($"Access denied to anonymous request on {pageUrl}");
            return View();
        }

        [HttpPost]
        public ActionResult Excel_Export_Save(string contentType, string base64, string fileName)
        {
            byte[] fileContents = Convert.FromBase64String(base64);
            return File(fileContents, contentType, fileName);
        }

        [HttpPost]
        public ActionResult Pdf_Export_Save(string contentType, string base64, string fileName)
        {
            byte[] fileContents = Convert.FromBase64String(base64);
            return File(fileContents, contentType, fileName);
        }
    }
}