using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using AspNetMvcActiveDirectoryOwin.Core;
using AspNetMvcActiveDirectoryOwin.Core.Data;
using AspNetMvcActiveDirectoryOwin.Core.Domain;
using AspNetMvcActiveDirectoryOwin.Services.Domains;
using AspNetMvcActiveDirectoryOwin.Services.Logging;
using AspNetMvcActiveDirectoryOwin.Services.Messages;
using AspNetMvcActiveDirectoryOwin.Services.Roles;
using AspNetMvcActiveDirectoryOwin.Services.Users;
using AspNetMvcActiveDirectoryOwin.Web.Areas.Administration.Controllers;
using AspNetMvcActiveDirectoryOwin.Web.Common.Mapper;
using AspNetMvcActiveDirectoryOwin.Web.Common.Models.Users;
using AspNetMvcActiveDirectoryOwin.Web.Common.Mvc.Alerts;
using AspNetMvcActiveDirectoryOwin.Web.Common.Security;
using Kendo.Mvc;
using Kendo.Mvc.UI;
using log4net;
using NSubstitute;
using NUnit.Framework;

namespace AspNetMvcActiveDirectoryOwin.Web.Tests.Controllers
{
    [TestFixture]
    class UserControllerTests
    {
        private User _user1;
        private User _user2;
        private User _user3;
        private IPagedList<User> _users;
        private IList<Role> _roles;
        private Domain _domain1;
        private Domain _domain2;
        private Domain _domain3;
        private IList<Domain> _domains;
        private IDateTime _dateTime;
        private ILogManager _logManager;
        private IMessageService _messageService;
        private IDomainService _domainService;

        [SetUp]
        public void SetUp()
        {
            _user1 = new User
            {
                Id = 1,
                UserName = "johndoe",
                FirstName = "John",
                LastName = "Doe",
                Active = true,
                CreatedBy = "Developer",
                CreatedOn = new DateTime(2016, 01, 01),
                ModifiedBy = "Developer",
                ModifiedOn = new DateTime(2016, 01, 01)
            };
            _user2 = new User
            {
                Id = 2,
                UserName = "janetdoe",
                FirstName = "John",
                LastName = "Doe",
                Active = true,
                CreatedBy = "Developer",
                CreatedOn = new DateTime(2016, 01, 02),
                ModifiedBy = "Developer",
                ModifiedOn = new DateTime(2016, 01, 02),
                Roles = new List<Role>
                {
                    new Role {Id = 1, Name = "Developer"}
                }
            };
            _user3 = new User
            {
                Id = 3,
                UserName = "123456789",
                FirstName = "Eric",
                LastName = "Newton",
                Active = false,
                CreatedBy = "Developer",
                CreatedOn = new DateTime(2016, 01, 02),
                ModifiedBy = "Developer",
                ModifiedOn = new DateTime(2016, 01, 02)
            };
            _users = new PagedList<User>
            {
                _user1,
                _user2,
                _user3
            };

            _roles = new List<Role>
            {
                new Role {Id = 1, Name = Constants.RoleNames.Developer},
                new Role {Id = 2, Name = Constants.RoleNames.ApplicationManager},
                new Role {Id = 3, Name = "Role1"},
                new Role {Id = 4, Name = "Role2"}
            };

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
            _domains = new List<Domain> { _domain1, _domain2, _domain3 };

            var mockDateTime = Substitute.For<IDateTime>();
            mockDateTime.Now.Returns(new DateTime(2017, 01, 01));
            _dateTime = mockDateTime;

            var mockMessageService = Substitute.For<IMessageService>();
            mockMessageService.SendAddNewUserNotification(Arg.Any<User>());
            _messageService = mockMessageService;

            var mockLogManager = Substitute.For<ILogManager>();
            var mockLog = Substitute.For<ILog>();
            mockLogManager.GetLog(Arg.Any<Type>()).Returns(mockLog);
            _logManager = mockLogManager;

            var mockDomainService = Substitute.For<IDomainService>();
            mockDomainService.GetAllDomainsAsync().Returns(_domains);
            _domainService = mockDomainService;

            AutoMapperConfiguration.Initialize();
        }

        [Test]
        public async Task List_UserNotInDeveloperRole_ReturnViewResultWithoutDeveloperRole()
        {
            // Arrange
            var userService = Substitute.For<IUserService>();

            var roleService = Substitute.For<IRoleService>();
            roleService.GetAllRoles().Returns(_roles);

            var webUserSession = Substitute.For<IWebUserSession>();
            webUserSession.IsInRole(Constants.RoleNames.Developer).Returns(true);

            var sut = new UsersController(null, null, null, _logManager, null, roleService, userService, webUserSession);

            // Act
            var result = await sut.List() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("List", result.ViewName);
            var model = (UserSearchModel) result.Model;
            Assert.AreEqual(5, model.AvailableRoles.Count);

            Assert.AreEqual("All", model.AvailableRoles[0].Text);
            Assert.AreEqual("", model.AvailableRoles[0].Value);

            Assert.AreEqual(_roles[0].Name, model.AvailableRoles[1].Text);
            Assert.AreEqual(_roles[0].Id.ToString(), model.AvailableRoles[1].Value);
        }

        [Test]
        public async Task List_UserInDeveloperRole_ReturnViewResultWithDeveloperRole()
        {
            // Arrange
            var roleService = Substitute.For<IRoleService>();
            roleService.GetAllRoles().Returns(_roles);

            var webUserSession = Substitute.For<IWebUserSession>();
            webUserSession.IsInRole(Constants.RoleNames.Developer).Returns(true);
			
            var sut = new UsersController(null, null, _domainService, _logManager, null, roleService, null, webUserSession);

            // Act
            var result = await sut.List() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("List", result.ViewName);
            var model = (UserSearchModel) result.Model;
            Assert.AreEqual(5, model.AvailableRoles.Count);

            Assert.AreEqual("All", model.AvailableRoles[0].Text);
            Assert.AreEqual("", model.AvailableRoles[0].Value);

            Assert.AreEqual(_roles[0].Name, model.AvailableRoles[1].Text);
            Assert.AreEqual(_roles[0].Id.ToString(), model.AvailableRoles[1].Value);
        }

        [Test]
        public async Task List_PostDataSourceRequestAndTraceLogSearchModel_ReturnJsonResult()
        {
            // Arrange
            var userService = Substitute.For<IUserService>();
            userService.GetUsersAsync(Arg.Any<UserPagedDataRequest>()).Returns(_users);

            var sut = new UsersController(null, null, null, _logManager, null, null, userService, null);

            var request = new DataSourceRequest {Sorts = new List<SortDescriptor>()};
            var model = new UserSearchModel();

            // Act
            var result = await sut.List(request, model) as JsonResult;

            // Assert
            Assert.IsNotNull(result);
            var dataSourceResult = (DataSourceResult) result.Data;
            var models = (IEnumerable<UserModel>) dataSourceResult.Data;
            Assert.AreEqual(3, models.Count());
        }

        [Test]
        public async Task Create_UserNotInDeveloperRole_ReturnViewResultWithoutDeveloperRole()
        {
            // Arrange
            var roleService = Substitute.For<IRoleService>();
            roleService.GetAllRoles().Returns(_roles);

            var webUserSession = Substitute.For<IWebUserSession>();
            webUserSession.IsInRole(Constants.RoleNames.Developer).Returns(false);

            var sut = new UsersController(null, null, _domainService, _logManager, null, roleService, null, webUserSession);

            // Act
            var result = await sut.Create() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Create", result.ViewName);
            var model = (UserCreateUpdateModel) result.Model;
            Assert.AreEqual(3, model.AvailableRoleNames.Count);
        }

        [Test]
        public async Task Create_UserInDeveloperRole_ReturnViewResultWithDeveloperRole()
        {
            // Arrange
            var roleService = Substitute.For<IRoleService>();
            roleService.GetAllRoles().Returns(_roles);

            var webUserSession = Substitute.For<IWebUserSession>();
            webUserSession.IsInRole(Constants.RoleNames.Developer).Returns(true);

            var sut = new UsersController(null, null, _domainService, _logManager, null, roleService, null, webUserSession);

            // Act
            var result = await sut.Create() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Create", result.ViewName);
            var model = (UserCreateUpdateModel) result.Model;
            Assert.AreEqual(4, model.AvailableRoleNames.Count);
        }

        [Test]
        public async Task Create_PostUserAlreadyExists_RedirectToListWithErrorMessage()
        {
            // Arrange
            var userService = Substitute.For<IUserService>();
            userService.GetUserByUserNameAsync(Arg.Any<string>()).Returns(_user1);

            var roleService = Substitute.For<IRoleService>();
            var webUserSession = Substitute.For<IWebUserSession>();

            var sut = new UsersController(null, null, _domainService, _logManager, _messageService, roleService, userService, webUserSession);

            var model = new UserCreateUpdateModel {UserName = "johndoe"};

            // Act
            var decoratorResult = await sut.Create(model) as AlertDecoratorResult;

            // Assert
            await userService.Received(1).GetUserByUserNameAsync(Arg.Any<string>());
            await userService.Received(0).AddUserAsync(Arg.Any<User>());
            await roleService.Received(0).GetAllRoles();
            await _messageService.Received(0).SendAddNewUserNotification(Arg.Any<User>());

            Assert.IsNotNull(decoratorResult);
            Assert.AreEqual(decoratorResult.Message, $"User with same username {model.UserName} alredy exists.");

            var routeResult = decoratorResult.InnerResult as RedirectToRouteResult;
            Assert.IsNotNull(routeResult);
            Assert.IsFalse(routeResult.Permanent);
            Assert.AreEqual(routeResult.RouteValues["action"], "List");
        }

        [Test]
        public async Task Create_PostValidUser_RedirectToListWithSuccessfulMessage()
        {
            // Arrange
            var userService = Substitute.For<IUserService>();
            userService.AddUserAsync(Arg.Any<User>()).Returns(1);
            userService.GetUserByUserNameAsync(Arg.Any<string>()).Returns((User) null);

            var roleService = Substitute.For<IRoleService>();
            roleService.GetAllRoles().Returns(_roles);

            var webUserSession = Substitute.For<IWebUserSession>();
            webUserSession.IsInRole(Constants.RoleNames.Developer).Returns(true);

            var sut = new UsersController(null, _dateTime, _domainService, _logManager, _messageService, roleService, userService, webUserSession);

            var model = new UserCreateUpdateModel {UserName = _user1.UserName, FirstName = _user1.FirstName};

            // Act
            var decoratorResult = await sut.Create(model) as AlertDecoratorResult;

            // Assert
            await userService.Received(1).GetUserByUserNameAsync(Arg.Any<string>());
            await userService.Received(1).AddUserAsync(Arg.Any<User>());
            await roleService.Received(1).GetAllRoles();
            await _messageService.Received(1).SendAddNewUserNotification(Arg.Any<User>());

            Assert.IsNotNull(decoratorResult);
            Assert.AreEqual(decoratorResult.Message, $"{_user1.FirstName}'s account was created successfully.");

            var routeResult = decoratorResult.InnerResult as RedirectToRouteResult;
            Assert.IsNotNull(routeResult);
            Assert.IsFalse(routeResult.Permanent);
            Assert.AreEqual(routeResult.RouteValues["action"], "List");
        }

        [Test]
        public async Task Edit_InvalidId_RedirectToList()
        {
            // Arrange
            var userService = Substitute.For<IUserService>();
            userService.GetUserByIdAsync(Arg.Any<int>()).Returns((User) null);

            var sut = new UsersController(null, _dateTime, _domainService, _logManager, _messageService, null, userService, null);

            // Act
            var routeResult = await sut.Edit(100) as RedirectToRouteResult;

            // Assert
            await userService.Received(1).GetUserByIdAsync(Arg.Any<int>());

            Assert.IsNotNull(routeResult);
            Assert.IsFalse(routeResult.Permanent);
            Assert.AreEqual(routeResult.RouteValues["action"], "List");
        }

        [Test]
        public async Task Edit_InvalidId_RedirectToListWithErrorMessage()
        {
            // Arrange
            var userService = Substitute.For<IUserService>();
            userService.GetUserByIdAsync(_user1.Id).Returns((User) null);

            var sut = new UsersController(null, null, _domainService, _logManager, null, null, userService, null);

            var model = new UserCreateUpdateModel {Id = _user1.Id, FirstName = _user1.FirstName};

            // Act
            var decoratorResult = await sut.Edit(model) as AlertDecoratorResult;

            // Assert
            await userService.Received(1).GetUserByIdAsync(Arg.Any<int>());
            await userService.Received(0).UpdateUserAsync(Arg.Any<User>());

            Assert.IsNotNull(decoratorResult);
            Assert.AreEqual(decoratorResult.Message, "Please select a user.");

            var routeResult = decoratorResult.InnerResult as RedirectToRouteResult;
            Assert.IsNotNull(routeResult);
            Assert.IsFalse(routeResult.Permanent);
            Assert.AreEqual(routeResult.RouteValues["action"], "List");
        }

        [Test]
        public async Task Edit_PostValidId_RedirectToListWithSuccessfulMessage()
        {
            // Arrange
            var userService = Substitute.For<IUserService>();
            userService.GetUserByIdAsync(_user1.Id).Returns(_user1);

            var roleService = Substitute.For<IRoleService>();
            roleService.GetAllRoles().Returns(_roles);

            var webUserSession = Substitute.For<IWebUserSession>();
            webUserSession.IsInRole(Arg.Any<string>()).Returns(true);
            
            var sut = new UsersController(null, _dateTime, _domainService, _logManager, _messageService, roleService, userService, webUserSession);

            var model = new UserCreateUpdateModel {Id = _user1.Id, FirstName = _user1.FirstName};

            // Act
            var decoratorResult = await sut.Edit(model) as AlertDecoratorResult;

            // Assert
            await userService.Received(1).GetUserByIdAsync(Arg.Any<int>());
            await roleService.Received(1).GetAllRoles();
            await userService.Received(1).UpdateUserAsync(Arg.Any<User>());

            Assert.IsNotNull(decoratorResult);
            Assert.AreEqual(decoratorResult.Message, $"{_user1.FirstName}'s account was updated successfully.");

            var routeResult = decoratorResult.InnerResult as RedirectToRouteResult;
            Assert.IsNotNull(routeResult);
            Assert.IsFalse(routeResult.Permanent);
            Assert.AreEqual(routeResult.RouteValues["action"], "List");
        }
    }
}