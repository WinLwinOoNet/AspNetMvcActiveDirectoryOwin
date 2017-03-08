using System;
using System.Collections.Generic;
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
using AspNetMvcActiveDirectoryOwin.Web.Common.Models.Logs;
using Kendo.Mvc;
using Kendo.Mvc.UI;

namespace AspNetMvcActiveDirectoryOwin.Web.Areas.Administration.Controllers
{
    [TraceMvc]
    [Authorize(Roles = Constants.RoleNames.Developer)]
    public class LogsController : Controller
    {
        private readonly ILogService _logService;

        public LogsController(ILogService logService)
        {
            _logService = logService;
        }

        public ActionResult Index()
        {
            return RedirectToAction("List");
        }

        public async Task<ActionResult> List()
        {
            var availableLevels = new List<SelectListItem>
            {
                new SelectListItem {Text = "All", Value = ""},
                new SelectListItem {Text = "Debug", Value = "Debug"},
                new SelectListItem {Text = "Info", Value = "Info"},
                new SelectListItem {Text = "Warn", Value = "Warn"},
                new SelectListItem {Text = "Error", Value = "Error"},
                new SelectListItem {Text = "Fatal", Value = "Fatal"},
            };
            var entities = await _logService.GetUsernames();
            var availableUsernames = entities.Select(x => new SelectListItem {Text = x, Value = x}).ToList();
            availableUsernames.Insert(0, new SelectListItem {Text = "All", Value = ""});
            availableUsernames.Insert(1, new SelectListItem {Text = "Not Available", Value = "NOT AVAILABLE"});
            var model = new LogSearchModel {AvailableLevels = availableLevels, AvailableUsernames = availableUsernames};
            return View("List", model);
        }

        [HttpPost]
        public async Task<ActionResult> List([DataSourceRequest] DataSourceRequest request, LogSearchModel model)
        {
            var dataRequest = ParsePagedDataRequest(request, model);
            var entities = await _logService.GetLogs(dataRequest);
            var models = entities.Select(x => x.ToModel()).ToList();
            var result = new DataSourceResult {Data = models.ToList(), Total = entities.TotalCount};
            return Json(result);
        }

        private LogPagedDataRequest ParsePagedDataRequest(DataSourceRequest request, LogSearchModel model)
        {
            var dataRequest = new LogPagedDataRequest
            {
                Thread = model.Thread,
                Username = model.SelectedUsername,
                Level = model.SelectedLevel,
                Message = model.Message,
                Exception = model.Exception,
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
                LogSortField sortField;
                if (!Enum.TryParse(sort.Member, out sortField))
                    sortField = LogSortField.Date;
                dataRequest.SortField = sortField;
                dataRequest.SortOrder = sort.SortDirection == ListSortDirection.Ascending ? SortOrder.Ascending : SortOrder.Descending;
            }
            return dataRequest;
        }
    }
}