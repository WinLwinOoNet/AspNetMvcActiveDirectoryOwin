using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using AspNetMvcActiveDirectoryOwin.Core.Data;
using AspNetMvcActiveDirectoryOwin.Core.Domain;
using AspNetMvcActiveDirectoryOwin.Services.Logging;
using AspNetMvcActiveDirectoryOwin.Web.Areas.Administration.Controllers;
using AspNetMvcActiveDirectoryOwin.Web.Common.Mapper;
using AspNetMvcActiveDirectoryOwin.Web.Common.Models.TraceLogs;
using Kendo.Mvc;
using Kendo.Mvc.UI;
using NSubstitute;
using NUnit.Framework;

namespace AspNetMvcActiveDirectoryOwin.Web.Tests.Controllers
{
    [TestFixture]
    public class TraceLogsControllerTests
    {
        private TraceLog _traceLog1;
        private TraceLog _traceLog2;
        private TraceLog _traceLog3;
        private IPagedList<TraceLog> _traceLogs;

        [SetUp]
        public void SetUp()
        {
            _traceLog1 = new TraceLog
            {
                Id = 1,
                Controller = "Logs",
                Action = "List",
                Message = "{\"request\":{\"Page\":1,\"PageSize\":10,\"Sorts\":[],\"Filters\":[]}}",
                PerformedOn = new DateTime(2016, 01, 01),
                PerformedBy = "johndoe",
            };
            _traceLog2 = new TraceLog
            {
                Id = 2,
                Controller = "TraceLog",
                Action = "Index",
                Message = "{}",
                PerformedOn = new DateTime(2016, 01, 01),
                PerformedBy = "johndoe",
            };
            _traceLog3 = new TraceLog
            {
                Id = 1,
                Controller = "Logs",
                Action = "List",
                Message = "{\"request\":{\"Page\":1,\"PageSize\":10,\"Sorts\":[],\"Filters\":[]}}",
                PerformedOn = new DateTime(2016, 01, 01),
                PerformedBy = "janetdoe",
            };
            _traceLogs = new PagedList<TraceLog>
            {
                _traceLog1,
                _traceLog2,
                _traceLog3
            };

            AutoMapperConfiguration.Initialize();
        }

        [Test]
        public async Task List_ReturnViewResult()
        {
            // Arrange
            var traceLogService = Substitute.For<ITraceLogService>();
            var usernames = _traceLogs.Select(t => t.PerformedBy).OrderBy(t => t).Distinct().ToList();
            traceLogService.GetTraceLogs(Arg.Any<TraceLogPagedDataRequest>()).Returns(_traceLogs);
            traceLogService.GetPerformedUsernames().Returns(usernames);

            var sut = new TraceLogsController(traceLogService);

            // Act
            var result = await sut.List() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("List", result.ViewName);
            var model = (TraceLogSearchModel) result.Model;
            Assert.AreEqual(3, model.AvailableUsernames.Count);

            Assert.AreEqual("All", model.AvailableUsernames[0].Text);
            Assert.AreEqual("", model.AvailableUsernames[0].Value);

            Assert.AreEqual(usernames[0], model.AvailableUsernames[1].Text);
            Assert.AreEqual(usernames[0], model.AvailableUsernames[1].Value);

            Assert.AreEqual(usernames[1], model.AvailableUsernames[2].Text);
            Assert.AreEqual(usernames[1], model.AvailableUsernames[2].Value);
        }

        [Test]
        public async Task List_PostDataSourceRequestAndTraceLogSearchModel_ReturnJsonResult()
        {
            // Arrange
            var traceLogService = Substitute.For<ITraceLogService>();
            traceLogService.GetTraceLogs(Arg.Any<TraceLogPagedDataRequest>()).Returns(_traceLogs);

            var sut = new TraceLogsController(traceLogService);

            var request = new DataSourceRequest {Sorts = new List<SortDescriptor>()};
            var model = new TraceLogSearchModel();

            // Act
            var result = await sut.List(request, model) as JsonResult;

            // Assert
            Assert.IsNotNull(result);
            var dataSourceResult = (DataSourceResult) result.Data;
            var models = (IEnumerable<TraceLogModel>) dataSourceResult.Data;
            Assert.AreEqual(3, models.Count());
        }
    }
}