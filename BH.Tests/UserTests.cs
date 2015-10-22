using System;
using BH.Data.Databases;
using BH.Models.DomainModels;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BH.Tests
{
    [TestClass]
    public class UserTests
    {
        private UserManager<User> userManager;
        private RoleManager<IdentityRole> roleManager;
            
        [TestInitialize]
        public void SetupTest()
        {
            userManager = new UserManager<User>(new UserStore<User>(new BHDbContext()));
            roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(new BHDbContext()));
        }

        [TestMethod]
        public void Can_Authenticate_User()
        {
            var adminUser = userManager.Find("admin", "adminpass123");
            Assert.IsNotNull(adminUser);
        }

        [TestMethod]
        public void Can_Fail_Authenticateion()
        {
            var failedUser = userManager.Find("user", "somepassword");
            Assert.IsNull(failedUser);
        }

        [TestMethod]
        public void Can_Check_User_Roles()
        {
            var adminUser = userManager.Find("admin", "adminpass123");
            var isAdmin = userManager.IsInRole(adminUser.Id, "Admin");
            Assert.IsTrue(isAdmin);
        }

        [TestMethod]
        public void Can_Display_User_Roles()
        {
            var adminUser = userManager.Find("admin", "adminpass123");
            foreach (var role in adminUser.Roles)
            {
                var identityRole = roleManager.FindById(role.RoleId);
                Console.WriteLine(identityRole.Name);
            }
            Assert.IsTrue(adminUser.Roles.Count > 0);
        }
    }
}
