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
using AspNetMvcActiveDirectoryOwin.Web.Common.Models.Logs;
using Kendo.Mvc;
using Kendo.Mvc.UI;
using NSubstitute;
using NUnit.Framework;

namespace AspNetMvcActiveDirectoryOwin.Web.Tests.Controllers
{
    [TestFixture]
    public class LogsControllerTests
    {
        private Log _log1;
        private Log _log2;
        private Log _log3;
        private IPagedList<Log> _logs;

        [SetUp]
        public void SetUp()
        {
            _log1 = new Log
            {
                Id = 1,
                Date = new DateTime(2016, 01, 01),
                Username = "johndoe",
                Thread = "",
                Level = "",
                Logger = "", 
                Message = "",
                Exception = ""
            };
            _log2 = new Log
            {
                Id = 2,
                Date = new DateTime(2016, 01, 01),
                Username = "johndoe",
                Thread = "",
                Level = "",
                Logger = "",
                Message = "",
                Exception = ""
            };
            _log3 = new Log
            {
                Id = 1,
                Date = new DateTime(2016, 01, 01),
                Username = "janetdoe",
                Thread = "",
                Level = "",
                Logger = "",
                Message = "",
                Exception = ""
            };
            _logs = new PagedList<Log>
            {
                _log1,
                _log2,
                _log3
            };

            AutoMapperConfiguration.Initialize();
        }

        [Test]
        public async Task List_ReturnViewResult()
        {
            // Arrange
            var logService = Substitute.For<ILogService>();
            var usernames = _logs.Select(t => t.Username).OrderBy(t => t).Distinct().ToList();
            logService.GetLogs(Arg.Any<LogPagedDataRequest>()).Returns(_logs);
            logService.GetUsernames().Returns(usernames);
            
            var sut = new LogsController(logService);

            // Act
            var result = await sut.List() as ViewResult;

            // Assert
			Assert.IsNotNull(result);
            Assert.AreEqual("List", result.ViewName);
            var model = (LogSearchModel) result.Model;
            Assert.AreEqual(4, model.AvailableUsernames.Count);

            Assert.AreEqual("All", model.AvailableUsernames[0].Text);
            Assert.AreEqual("", model.AvailableUsernames[0].Value);

            Assert.AreEqual("Not Available", model.AvailableUsernames[1].Text);
            Assert.AreEqual("NOT AVAILABLE", model.AvailableUsernames[1].Value);

            Assert.AreEqual(usernames[0], model.AvailableUsernames[2].Text);
            Assert.AreEqual(usernames[0], model.AvailableUsernames[2].Value);

            Assert.AreEqual(usernames[1], model.AvailableUsernames[3].Text);
            Assert.AreEqual(usernames[1], model.AvailableUsernames[3].Value);
        }

        [Test]
        public async Task List_PostDataSourceRequestAndLogSearchModel_ReturnJsonResult()
        {
            // Arrange
            var logService = Substitute.For<ILogService>();
            logService.GetLogs(Arg.Any<LogPagedDataRequest>()).Returns(_logs);

            var sut = new LogsController(logService);

            var request = new DataSourceRequest {Sorts = new List<SortDescriptor>()};
            var model = new LogSearchModel();

            // Act
            var jsonResult = (JsonResult) await sut.List(request, model);

            // Assert
            var dataSourceResult = (DataSourceResult) jsonResult.Data;
            var models = (IEnumerable<LogModel>) dataSourceResult.Data;
            Assert.AreEqual(3, models.Count());
        }
    }
}