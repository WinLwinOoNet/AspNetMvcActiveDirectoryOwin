using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Threading.Tasks;
using AspNetMvcActiveDirectoryOwin.Core.Data;
using AspNetMvcActiveDirectoryOwin.Core.Domain;
using AspNetMvcActiveDirectoryOwin.Services.Users;
using NSubstitute;
using NUnit.Framework;

namespace AspNetMvcActiveDirectoryOwin.Services.Tests.Users
{
    [TestFixture]
    class UserServiceTests
    {
        private User _user1;
        private User _user2;
        private User _user3;
        private IQueryable<User> _users;
        private IRepository<User> _userRepository;

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
            _users = new List<User>
            {
                _user1,
                _user2,
                _user3
            }.AsQueryable();

            var mockSet = NSubstituteHelper.CreateMockDbSet(_users);
            var mockRepository = Substitute.For<IRepository<User>>();
            mockRepository.Entities.Returns(mockSet);
            _userRepository = mockRepository;
        }

        [Test]
        public async Task GetUserById_ValidId_Return1User()
        {
            var sut = new UserService(_userRepository);
            var user = await sut.GetUserByIdAsync(2);
            Assert.AreEqual(_user2, user);
        }

        [Test]
        public async Task GetUserById_InvalidId_ReturnNull()
        {
            var sut = new UserService(_userRepository);
            var user = await sut.GetUserByIdAsync(100);
            Assert.IsNull(user);
        }

        [Test]
        public async Task GetUserByUserName_ValidUserName_Return1User()
        {
            var sut = new UserService(_userRepository);
            var user = await sut.GetUserByUserNameAsync("123456789");
            Assert.AreEqual(_user3, user);
        }

        [Test]
        public async Task GetUserByUserName_InvalidUsername_ReturnNull()
        {
            var sut = new UserService(_userRepository);
            var user = await sut.GetUserByUserNameAsync("invalid");
            Assert.IsNull(user);
        }

        [Test]
        public async Task GetUsers_NoFilter_RetrieveAll3Users()
        {
            var sut = new UserService(_userRepository);
            var users = await sut.GetUsersAsync(new UserPagedDataRequest());
            Assert.AreEqual(3, users.Count);
            Assert.IsTrue(users.Contains(_user1));
            Assert.IsTrue(users.Contains(_user2));
            Assert.IsTrue(users.Contains(_user3));
        }

        [Test]
        public async Task GetUsers_FilterByActive_Return2Users()
        {
            var sut = new UserService(_userRepository);
            var users = await sut.GetUsersAsync(new UserPagedDataRequest {Active = true});
            Assert.AreEqual(2, users.Count);
            Assert.IsTrue(users.Contains(_user1));
            Assert.IsTrue(users.Contains(_user2));
        }

        [Test]
        public async Task GetUsers_FilterByInactive_Return1User()
        {
            var sut = new UserService(_userRepository);
            var users = await sut.GetUsersAsync(new UserPagedDataRequest {Active = false});
            Assert.AreEqual(1, users.Count);
            Assert.IsTrue(users.Contains(_user3));
        }

        [Test]
        public async Task GetUsers_FilterByRole_Return1User()
        {
            var sut = new UserService(_userRepository);
            var users = await sut.GetUsersAsync(new UserPagedDataRequest {RoleName = "Developer"});
            Assert.AreEqual(1, users.Count);
            Assert.IsTrue(users.Contains(_user2));
        }

        [Test]
        public async Task GetUsers_FilterByUsername_Return1User()
        {
            var sut = new UserService(_userRepository);
            var users = await sut.GetUsersAsync(new UserPagedDataRequest {LastName = "Newton"});
            Assert.AreEqual(1, users.Count);
            Assert.IsTrue(users.Contains(_user3));
        }

        [Test]
        public async Task AddUser_CallAddFromDbSetAndCallSaveChangesAsync_ReturnId()
        {
            // Arrange
            var mockSet = Substitute.For<IDbSet<User>>();
            var mockRepository = Substitute.For<IRepository<User>>();
            mockRepository.Entities.Returns(mockSet);
            var service = new UserService(mockRepository);

            // Act
            int id = await service.AddUserAsync(_user1);

            // Assert
            Assert.AreEqual(id, _user1.Id);
            mockSet.Received(1).Add(Arg.Any<User>());
            await mockRepository.Received(1).SaveChangesAsync();
        }

        [Test]
        public async Task UpdateUser_CallAddOrUpdateFromDbSetAndCallSaveChangesAsync()
        {
            // Arrange
            var mockSet = NSubstituteHelper.CreateMockDbSet<User>();
            var mockRepository = Substitute.For<IRepository<User>>();
            mockRepository.Entities.Returns(mockSet);
            var service = new UserService(mockRepository);

            // Act
            await service.UpdateUserAsync(_user1);

            // Assert
            mockSet.Received(1).AddOrUpdate(Arg.Any<User>());
            await mockRepository.Received(1).SaveChangesAsync();
        }
    }
}