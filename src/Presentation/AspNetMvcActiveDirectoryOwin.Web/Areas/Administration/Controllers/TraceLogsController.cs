using System;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using AspNetMvcActiveDirectoryOwin.Core;
using AspNetMvcActiveDirectoryOwin.Core.Data;
using AspNetMvcActiveDirectoryOwin.Core.Extensions;
using AspNetMvcActiveDirectoryOwin.Services.Logging;
using AspNetMvcActiveDirectoryOwin.Web.Common.Debugging;
using AspNetMvcActiveDirectoryOwin.Web.Common.Mapper;
using AspNetMvcActiveDirectoryOwin.Web.Common.Models.TraceLogs;
using Kendo.Mvc;
using Kendo.Mvc.UI;

namespace AspNetMvcActiveDirectoryOwin.Web.Areas.Administration.Controllers
{
    [TraceMvc]
    [Authorize(Roles = Constants.RoleNames.Developer)]
    public class TraceLogsController : Controller
    {
        private readonly ITraceLogService _traceLogService;

        public TraceLogsController(ITraceLogService traceLogService)
        {
            _traceLogService = traceLogService;
        }

        public ActionResult Index()
        {
            return RedirectToAction("List");
        }

        public async Task<ActionResult> List()
        {
            var userNames = await _traceLogService.GetPerformedUsernames();
            var model = new TraceLogSearchModel {AvailableUsernames = userNames.Select(u => new SelectListItem {Text = u, Value = u}).ToList()};
            model.AvailableUsernames.Insert(0, new SelectListItem {Text = "All", Value = ""});
            return View("List", model);
        }

        [HttpPost]
        public async Task<ActionResult> List([DataSourceRequest] DataSourceRequest request, TraceLogSearchModel model)
        {
            var dataRequest = ParsePagedDataRequest(request, model);
            var entities = await _traceLogService.GetTraceLogs(dataRequest);
            var models = entities.Select(x => x.ToModel()).ToList();
            var result = new DataSourceResult {Data = models.ToList(), Total = entities.TotalCount};
            return Json(result);
        }

        private TraceLogPagedDataRequest ParsePagedDataRequest(DataSourceRequest request, TraceLogSearchModel model)
        {
            var dataRequest = new TraceLogPagedDataRequest
            {
                Controller = model.Controller,
                Action = model.Action,
                Message = model.Message,
                PerformedBy = model.PerformedBy,
                PageIndex = request.Page - 1,
                PageSize = request.PageSize
            };

            if (model.FromDate.HasValue)
                dataRequest.FromDate = model.FromDate.Value;

            if (model.ToDate.HasValue)
                dataRequest.ToDate = model.ToDate.Value.ToEndOfDay();

            SortDescriptor sort = request.Sorts.FirstOrDefault();
            if (sort != null)
            {
                TraceLogSortField sortField;
                if (!Enum.TryParse(sort.Member, out sortField))
                    sortField = TraceLogSortField.PerformedOn;
                dataRequest.SortField = sortField;
                dataRequest.SortOrder = sort.SortDirection == ListSortDirection.Ascending ? SortOrder.Ascending : SortOrder.Descending;
            }
            return dataRequest;
        }
    }
}