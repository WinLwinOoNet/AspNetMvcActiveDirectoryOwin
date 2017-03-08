using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspNetMvcActiveDirectoryOwin.Core.Data;
using AspNetMvcActiveDirectoryOwin.Core.Domain;
using AspNetMvcActiveDirectoryOwin.Services.Logging;
using NSubstitute;
using NUnit.Framework;

namespace AspNetMvcActiveDirectoryOwin.Services.Tests.Logging
{
    [TestFixture]
    class LogServiceTests
    {
        private Log _log1;
        private Log _log2;
        private Log _log3;
        private IQueryable<Log> _logs;
        private IRepository<Log> _logRepository;

        [SetUp]
        public void SetUp()
        {
            _log1 = new Log
            {
                Id = 1,
                Date = new DateTime(2016, 01, 01),
                Username = "johndoe",
                Thread = "1",
                Level = "INFO",
                Logger = "MyApp.Web.Global",
                Message = "Demo Message 1",
                Exception = ""
            };
            _log2 = new Log
            {
                Id = 2,
                Date = new DateTime(2016, 01, 01),
                Username = "janetdoe",
                Thread = "1",
                Level = "DEBUG",
                Logger = "MyApp.Web.Global",
                Message = "Demo Message 2",
                Exception = ""
            };
            _log3 = new Log
            {
                Id = 3,
                Date = new DateTime(2016, 01, 01),
                Username = "123456789",
                Thread = "3",
                Level = "ERROR",
                Logger = "MyApp.Web.Global",
                Message = "Demo Message 3",
                Exception = ""
            };
            _logs = new List<Log>
            {
                _log1,
                _log2,
                _log3
            }.AsQueryable();

            var mockSet = NSubstituteHelper.CreateMockDbSet(_logs);
            var mockRepository = Substitute.For<IRepository<Log>>();
            mockRepository.Entities.Returns(mockSet);
            _logRepository = mockRepository;
        }

        [Test]
        public async Task GetLogById_ValidId_ReturnLog()
        {
            var sut = new LogService(_logRepository);
            var log = await sut.GetLogById(2);
            Assert.AreEqual(_log2, log);
        }

        [Test]
        public async Task GetLogById_InvalidId_ReturnNull()
        {
            var sut = new LogService(_logRepository);
            var log = await sut.GetLogById(100);
            Assert.IsNull(log);
        }

        [Test]
        public async Task GetLogs_RetrieveAllLogs()
        {
            var sut = new LogService(_logRepository);
            var logs = await sut.GetLogs(new LogPagedDataRequest());
            Assert.AreEqual(3, logs.Count);
            Assert.IsTrue(logs.Contains(_log1));
            Assert.IsTrue(logs.Contains(_log2));
            Assert.IsTrue(logs.Contains(_log3));
        }

        [Test]
        public async Task GetLogs_FilterByUsername_Return1Log()
        {
            var sut = new LogService(_logRepository);
            var logs = await sut.GetLogs(new LogPagedDataRequest {Username = "janetdoe"});
            Assert.AreEqual(1, logs.Count);
            Assert.IsTrue(logs.Contains(_log2));
        }

        [Test]
        public async Task GetLogs_FilterByLevel_Return1Log()
        {
            var sut = new LogService(_logRepository);
            var logs = await sut.GetLogs(new LogPagedDataRequest {Level = "ERROR"});
            Assert.AreEqual(1, logs.Count);
            Assert.IsTrue(logs.Contains(_log3));
        }
    }
}