using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspNetMvcActiveDirectoryOwin.Core.Data;
using AspNetMvcActiveDirectoryOwin.Core.Domain;
using AspNetMvcActiveDirectoryOwin.Services.Roles;
using NSubstitute;
using NUnit.Framework;

namespace AspNetMvcActiveDirectoryOwin.Services.Tests.Roles
{
    [TestFixture]
    class RoleServiceTests
    {
        private Role _role1;
        private Role _role2;
        private Role _role3;
        private IQueryable<Role> _roles;
        private IRepository<Role> _roleRepository;

        [SetUp]
        public void SetUp()
        {
            var user1 = new User
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

            _role1 = new Role
            {
                Id = 1,
                Name = "role1",
                Users = new List<User> {user1}
            };
            _role2 = new Role
            {
                Id = 2,
                Name = "role2"
            };
            _role3 = new Role
            {
                Id = 3,
                Name = "role3",
                Users = new List<User> {user1}
            };
            _roles = new List<Role>
            {
                _role1,
                _role2,
                _role3
            }.AsQueryable();

            var mockSet = NSubstituteHelper.CreateMockDbSet(_roles);
            var mockRepository = Substitute.For<IRepository<Role>>();
            mockRepository.Entities.Returns(mockSet);
            _roleRepository = mockRepository;
        }

        [Test]
        public async Task GetRoles_RetrieveAllRoles()
        {
            var sut = new RoleService(_roleRepository);
            var roles = await sut.GetAllRoles();
            Assert.AreEqual(3, roles.Count);
            Assert.IsTrue(roles.Contains(_role1));
            Assert.IsTrue(roles.Contains(_role2));
            Assert.IsTrue(roles.Contains(_role3));
        }

        [Test]
        public async Task GetRolesForUser_UserHasRoles_ReturnRoles()
        {
            var sut = new RoleService(_roleRepository);
            var roles = await sut.GetRolesForUser(1);
            Assert.AreEqual(2, roles.Count);
        }

        [Test]
        public async Task GetRolesForUser_UserDoesNotHaveRoles_ReturnBlankList()
        {
            var sut = new RoleService(_roleRepository);
            var roles = await sut.GetRolesForUser(2);
            Assert.AreEqual(0, roles.Count);
        }
    }
}
