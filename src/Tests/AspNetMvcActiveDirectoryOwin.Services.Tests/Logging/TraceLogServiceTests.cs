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
    class TraceLogServiceTests
    {
        private TraceLog _traceLog1;
        private TraceLog _traceLog2;
        private TraceLog _traceLog3;
        private IQueryable<TraceLog> _traceLogs;
        private IRepository<TraceLog> _traceLogRepository;

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
            _traceLogs = new List<TraceLog>
            {
                _traceLog1,
                _traceLog2,
                _traceLog3
            }.AsQueryable();

            var mockSet = NSubstituteHelper.CreateMockDbSet(_traceLogs);
            var mockRepository = Substitute.For<IRepository<TraceLog>>();
            mockRepository.Entities.Returns(mockSet);
            _traceLogRepository = mockRepository;
        }

        [Test]
        public async Task GetTraceLogById_ValidId_ReturnTraceLog()
        {
            var sut = new TraceLogService(_traceLogRepository);
            var traceLog = await sut.GetTraceLogById(2);
            Assert.AreEqual(_traceLog2, traceLog);
        }

        [Test]
        public async Task GetTraceLogById_InvalidId_ReturnNull()
        {
            var sut = new TraceLogService(_traceLogRepository);
            var traceLog = await sut.GetTraceLogById(100);
            Assert.IsNull(traceLog);
        }

        [Test]
        public async Task GetTraceLogs_RetrieveAllTraceLogs()
        {
            var sut = new TraceLogService(_traceLogRepository);
            var traceLogs = await sut.GetTraceLogs(new TraceLogPagedDataRequest());
            Assert.AreEqual(3, traceLogs.Count);
            Assert.IsTrue(traceLogs.Contains(_traceLog1));
            Assert.IsTrue(traceLogs.Contains(_traceLog2));
            Assert.IsTrue(traceLogs.Contains(_traceLog3));
        }

        [Test]
        public async Task GetTraceLogs_FitlerByController_Return1TraceLog()
        {
            var sut = new TraceLogService(_traceLogRepository);
            var traceLogs = await sut.GetTraceLogs(new TraceLogPagedDataRequest {Controller = "TraceLog"});
            Assert.AreEqual(1, traceLogs.Count);
            Assert.IsTrue(traceLogs.Contains(_traceLog2));
        }

        [Test]
        public async Task GetTraceLogs_FilterByAction_Return2TraceLog()
        {
            var sut = new TraceLogService(_traceLogRepository);
            var traceLogs = await sut.GetTraceLogs(new TraceLogPagedDataRequest {Action = "List"});
            Assert.AreEqual(2, traceLogs.Count);
            Assert.IsTrue(traceLogs.Contains(_traceLog1));
            Assert.IsTrue(traceLogs.Contains(_traceLog3));
        }

        [Test]
        public async Task GetTraceLogs_FilterByMessage_Return1TraceLog()
        {
            var sut = new TraceLogService(_traceLogRepository);
            var traceLogs = await sut.GetTraceLogs(new TraceLogPagedDataRequest {Message = "{}"});
            Assert.AreEqual(1, traceLogs.Count);
            Assert.IsTrue(traceLogs.Contains(_traceLog2));
        }

        [Test]
        public async Task GetTraceLogs_FilterByPerformedBy_Return1TraceLog()
        {
            var sut = new TraceLogService(_traceLogRepository);
            var traceLogs = await sut.GetTraceLogs(new TraceLogPagedDataRequest {PerformedBy = "janetdoe"});
            Assert.AreEqual(1, traceLogs.Count);
            Assert.IsTrue(traceLogs.Contains(_traceLog3));
        }
    }
}