using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;
using AspNetMvcActiveDirectoryOwin.Core;
using AspNetMvcActiveDirectoryOwin.Core.Domain;
using AspNetMvcActiveDirectoryOwin.Services.Domains;
using AspNetMvcActiveDirectoryOwin.Services.Logging;
using AspNetMvcActiveDirectoryOwin.Services.Users;
using AspNetMvcActiveDirectoryOwin.Web.Common.Models.Account;
using AspNetMvcActiveDirectoryOwin.Web.Common.Security;
using AspNetMvcActiveDirectoryOwin.Web.Controllers;
using log4net;
using NSubstitute;
using NUnit.Framework;

namespace AspNetMvcActiveDirectoryOwin.Web.Tests.Controllers
{
    [TestFixture]
    public class AccountControllerTests
    {
        private Domain _domain1;
        private Domain _domain2;
        private Domain _domain3;
        private IList<Domain> _domains;
        private IDateTime _dateTime;
        private ILogManager _logManager;
        private IDomainService _domainService;

        [SetUp]
        public void SetUp()
        {
            _domain1 = new Domain
            {
                Id = 1,
                Name = "domain1.org",
                CreatedBy = "Developer",
                CreatedOn = new DateTime(2016, 01, 01),
                ModifiedBy = "Developer",
                ModifiedOn = new DateTime(2016, 01, 01)
            };
            _domain2 = new Domain
            {
                Id = 2,
                Name = "domain2.org",
                CreatedBy = "Developer",
                CreatedOn = new DateTime(2016, 01, 01),
                ModifiedBy = "Developer",
                ModifiedOn = new DateTime(2016, 01, 01)
            };
            _domain3 = new Domain
            {
                Id = 3,
                Name = "domain3.org",
                CreatedBy = "Developer",
                CreatedOn = new DateTime(2016, 01, 01),
                ModifiedBy = "Developer",
                ModifiedOn = new DateTime(2016, 01, 01)
            };
            _domains = new List<Domain> {_domain1, _domain2, _domain3};

            var mockDateTime = Substitute.For<IDateTime>();
            mockDateTime.Now.Returns(new DateTime(2017, 01, 01));
            _dateTime = mockDateTime;

            var mockLogManager = Substitute.For<ILogManager>();
            var mockLog = Substitute.For<ILog>();
            mockLogManager.GetLog(Arg.Any<Type>()).Returns(mockLog);
            _logManager = mockLogManager;

            var mockDomainService = Substitute.For<IDomainService>();
            mockDomainService.GetAllDomainsAsync().Returns(_domains);
            _domainService = mockDomainService;
        }

        [Test]
        public void Login_ReturnViewResult()
        {
            // Arrange
            var sut = new AccountController(null, null, _domainService, null, null, _logManager);

            // Act
            var result = sut.Login("").Result as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Login", result.ViewName);
            var model = (LoginModel) result.Model;
            Assert.AreEqual(3, model.AvailableDomains.Count);

            Assert.AreEqual("domain1.org", model.AvailableDomains[0].Text);
            Assert.AreEqual("domain1.org", model.AvailableDomains[0].Value);

            Assert.AreEqual("domain2.org", model.AvailableDomains[1].Value);
            Assert.AreEqual("domain2.org", model.AvailableDomains[1].Value);

            Assert.AreEqual("domain3.org", model.AvailableDomains[2].Value);
            Assert.AreEqual("domain3.org", model.AvailableDomains[2].Value);
        }

        [Test]
        public async Task Login_PostInvalidADAccount_ReturnViewWithModelError()
        {
            // Arrange
            var activeDirectoryService = Substitute.For<IActiveDirectoryService>();
            activeDirectoryService.ValidateCredentials(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>()).Returns(false);

            var sut = new AccountController(activeDirectoryService, null, _domainService, _dateTime, null, _logManager);

            var model = new LoginModel();

            // Act
            var result = await sut.Login(model, "") as ViewResult;

            // Assert
            activeDirectoryService.Received(1).ValidateCredentials(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>());

            Assert.IsNotNull(result);
            Assert.AreEqual("Login", result.ViewName);
            model = (LoginModel) result.Model;
            Assert.AreEqual(3, model.AvailableDomains.Count);

            Assert.True(!result.ViewData.ModelState.IsValid);
            Assert.AreEqual(result.ViewData.ModelState[""].Errors.Count, 1);
            Assert.AreEqual(result.ViewData.ModelState[""].Errors[0].ErrorMessage, "Incorrect username or password.");
        }

        [Test]
        public async Task Login_PostValidUser_RedirectToHome()
        {
            // Arrange
            var activeDirectoryService = Substitute.For<IActiveDirectoryService>();
            activeDirectoryService.ValidateCredentials(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>()).Returns(true);

            var authenticationService = Substitute.For<IAuthenticationService>();

            var user = new User {Active = true};
            var userService = Substitute.For<IUserService>();
            userService.GetUserByUserNameAsync(Arg.Any<string>()).Returns(user);

            var sut = new AccountController(activeDirectoryService, authenticationService, _domainService, _dateTime, userService, _logManager);

            // Act
            var result = await sut.Login(new LoginModel(), "") as RedirectToRouteResult;

            // Assert
            activeDirectoryService.Received(1).ValidateCredentials(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>());

            Assert.IsNotNull(result);
            Assert.IsFalse(result.Permanent);
            Assert.AreEqual(result.RouteValues["controller"], "Home");
            Assert.AreEqual(result.RouteValues["action"], "Index");
        }

        [Test]
        public async Task Login_PostValidDeveloper_RedirectToAdminDashboard()
        {
            // Arrange
            var user = new User {Active = true, Roles = new List<Role> {new Role {Name = Constants.RoleNames.Developer}}};

            var activeDirectoryService = Substitute.For<IActiveDirectoryService>();
            activeDirectoryService.ValidateCredentials(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>()).Returns(true);

            var authenticationService = Substitute.For<IAuthenticationService>();

            var userService = Substitute.For<IUserService>();
            userService.GetUserByUserNameAsync(Arg.Any<string>()).Returns(user);

            var sut = new AccountController(activeDirectoryService, authenticationService, _domainService, _dateTime, userService, _logManager);

            // Act
            var result = await sut.Login(new LoginModel(), "") as RedirectToRouteResult;

            // Assert
            activeDirectoryService.Received(1).ValidateCredentials(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>());
            await userService.Received(1).GetUserByUserNameAsync(Arg.Any<string>());
            authenticationService.Received(1).SignIn(Arg.Any<User>(), Arg.Is<IList<string>>(x => x[0] == Constants.RoleNames.Developer));

            Assert.IsNotNull(result);
            Assert.IsFalse(result.Permanent);
            Assert.IsNull(result.RouteValues["controller"]);
            Assert.IsNull(result.RouteValues["action"]);
            Assert.AreEqual(result.RouteName, "Dashboard");
        }
    }
}