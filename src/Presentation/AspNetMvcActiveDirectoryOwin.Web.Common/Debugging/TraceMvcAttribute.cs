using System.Web.Mvc;
using AspNetMvcActiveDirectoryOwin.Core;
using AspNetMvcActiveDirectoryOwin.Core.Domain;
using AspNetMvcActiveDirectoryOwin.Web.Common.Security;
using Newtonsoft.Json;

namespace AspNetMvcActiveDirectoryOwin.Web.Common.Debugging
{
    /// <summary>
    /// NOTE: Do not Global register this attribute to prevent logging user's passwords.
    /// </summary>
    public class TraceMvcAttribute : ActionFilterAttribute
    {
        public IWebUserSession WebUserSession { get; set; }
        public IDateTime DateTime { get; set; }
        public ITraceListener TraceListener { get; set; }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            string message;
            try
            {
                message = JsonConvert.SerializeObject(filterContext.ActionParameters);
            }
            catch
            {
                message = "Newtonsoft Error: HttpInputStream has file attachment. Message cannot be serialized.";
            }

            var traceLog = new TraceLog
            {
                Controller = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName,
                Action = filterContext.ActionDescriptor.ActionName,
                Message = message,
                PerformedOn = DateTime.Now,
                PerformedBy = WebUserSession?.UserName
            };
            TraceListener.AddTraceLogAsync(traceLog);
            base.OnActionExecuting(filterContext);
        }
    }
}