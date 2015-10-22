using System;
using System.Collections.Generic;
using BH.Data.Databases;
using BH.Models.DomainModels;
using BH.Services.DomainManagers;
using BH.Services.IdentityManagers;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BH.Tests
{
    [TestClass]
    public class UserGroupTests
    {
        private BHUserManager userManager;
        private GroupManager groupManager;

        [TestInitialize]
        public void SetupTest()
        {
            userManager = new BHUserManager(new UserStore<User>(BHDbContext.Create()));
            groupManager = new GroupManager();
        }

        [TestMethod]
        public void Can_Add_User_To_Group()
        {
            var existingUser = userManager.FindByName("testuser");
            if(existingUser != null)
                userManager.Delete(existingUser);

            var existingGroup = groupManager.GetByName("Group A");
            if(existingGroup != null)
                groupManager.Delete(existingGroup.Id);

            var newUser = new User()
            {
                FirstName = "Test",
                LastName = "User",
                EmailConfirmed = true,
                Email = "test@user.com",
                UserName = "testuser"
            };
            userManager.Create(newUser, "password");

            var group = new Group() {Name = "Group A"};
            groupManager.Create(group);

            Assert.IsTrue(groupManager.AddUser(group.Id, newUser.Id));

            var groupUsers = groupManager.GetById(group.Id, new List<string>() {"Users"}).Users;
            Console.WriteLine("Group: " + group.Name + " Has the following assigned Users!");
            foreach (var groupUser in groupUsers)
            {
                Console.WriteLine("UserID: " + groupUser.Id);
                Console.WriteLine("Name: " + groupUser.FirstName + " " + groupUser.LastName);
            }


            userManager.Delete(newUser);
            groupManager.Delete(group.Id);
        }

        [TestMethod]
        public void Can_Get_My_Child_Groups()
        {
            var existingUser = userManager.FindByName("testuser");
            if (existingUser != null)
                userManager.Delete(existingUser);

            var existingGroup = groupManager.GetByName("Group A");
            if (existingGroup != null)
                groupManager.Delete(existingGroup.Id);

            var existingGroup1 = groupManager.GetByName("Group B");
            if (existingGroup1 != null)
                groupManager.Delete(existingGroup1.Id);

            var newUser = new User()
            {
                FirstName = "Test",
                LastName = "User",
                EmailConfirmed = true,
                Email = "test@user.com",
                UserName = "testuser"
            };
            userManager.Create(newUser, "password");

            var group = new Group() { Name = "Group A" };
            groupManager.Create(group);

            var group1 = new Group() {Name = "Group B"};
            groupManager.Create(group1);

            groupManager.AssignParent(group1, group);

            Assert.IsTrue(groupManager.AddUser(group.Id, newUser.Id));

            var userGroups = userManager.GetById(newUser.Id, new List<string>() {"Groups", "Groups.ChildGroups"});
            var allMyGroups = new List<Group>();
            foreach (var myGroup in userGroups.Groups)
            {
                allMyGroups.Add(myGroup);
                allMyGroups.AddRange(myGroup.ChildGroups);
            }

            Console.WriteLine("All My Groups I Can View Below!");
            foreach (var allMyGroup in allMyGroups)
                Console.WriteLine("Group Name: " + allMyGroup.Name);


            userManager.Delete(newUser);
            groupManager.Delete(group.Id);
            groupManager.Delete(group1.Id);
        }
    }
}
