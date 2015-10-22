using System;
using System.Data.Entity;
using BH.Data.Databases;
using BH.Data.Migrations;
using BH.Models.DomainModels;
using BH.Services.DomainManagers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BH.Tests
{
    [TestClass]
    public class GroupTest
    {
        private GroupManager groupManager;

        [TestInitialize]
        public void SetupTests()
        {
            //Database.SetInitializer(new MigrateDatabaseToLatestVersion<BHDbContext, Configuration>());
            //using (var _context = new BHDbContext())
            //    _context.Database.Initialize(true);
            groupManager = new GroupManager();
        }

        [TestMethod]
        public void Can_Create_Group()
        {
            var newGroup = new Group()
            {
                Name = "Group A"
            };
            groupManager.Create(newGroup);
            Console.WriteLine("New Group ID: " + newGroup.Id);
            Console.WriteLine("New Group Name: " + newGroup.Name);
            Assert.IsNotNull(newGroup.Id);
        }

        [TestMethod]
        public void Can_Get_All_Groups()
        {
            var groupA = new Group()
            {
                Name = "Group A"
            };
            groupManager.Create(groupA);
            var groupB = new Group()
            {
                Name = "Group B"
            };
            groupManager.Create(groupB);

            var allGroups = groupManager.GetAll();

            foreach (var group in allGroups)
                Console.WriteLine("Group Name: " + group.Name);

            Assert.IsNotNull(allGroups);
        }

        [TestMethod]
        public void Can_Get_Group_By_Name()
        {
            var groupA = new Group()
            {
                Name = "Group A"
            };
            groupManager.Create(groupA);
            Console.WriteLine("Group Name: " + groupManager.GetByName("Group A").Name);
            Assert.IsInstanceOfType(groupA, typeof (Group));
        }

        [TestMethod]
        public void Can_Get_Group_By_Id()
        {
            var groupA = new Group()
            {
                Name = "Group A"
            };
            groupManager.Create(groupA);
            Console.WriteLine("Group Name: " + groupManager.GetById(groupA.Id).Name);
            Assert.IsInstanceOfType(groupA, typeof(Group));
        }

        [TestMethod]
        public void Can_Update_Group()
        {
            var groupB = new Group()
            {
                Name = "Group B"
            };
            groupManager.Create(groupB);
            Console.WriteLine("Initial Group Id: " + groupB.Id);
            Console.WriteLine("Initial Group Name: " + groupB.Name);
            var updatedGroupB = groupManager.GetById(groupB.Id);
            updatedGroupB.Name = "Group B (Updated)";
            groupManager.Update(updatedGroupB);
            Console.WriteLine("Updated Group Id: " + updatedGroupB.Id);
            Console.WriteLine("Updated Group Name: " + updatedGroupB.Name);
            Assert.AreNotEqual(groupB.Name, updatedGroupB.Name);
        }

        [TestMethod]
        public void Can_Delete_Group()
        {
            var groupC = new Group()
            {
                Name = "Group C"
            };
            groupManager.Create(groupC);
            Assert.IsNotNull(groupManager.GetById(groupC.Id));
            Assert.IsTrue(groupManager.Delete(groupC.Id));
            Assert.IsNull(groupManager.GetById(groupC.Id));
        }

        [TestMethod]
        public void Can_Assign_And_Get_Parent()
        {
            var groupA = new Group()
            {
                Name = "Group A"
            };
            groupManager.Create(groupA);
            var groupD = new Group()
            {
                Name = "Group D"
            };
            groupManager.Create(groupD);
            //var groupA = groupManager.GetByName("Group A");
            groupManager.AssignParent(groupD, groupA);
            var parentGroup = groupManager.GetParent(groupD.Id);
            Console.WriteLine("Parent Name: " + parentGroup.Name);
            Assert.IsNotNull(parentGroup);
        }

        [TestMethod]
        public void Can_Assign_And_Get_Children()
        {
            var groupA = new Group()
            {
                Name = "Group A"
            };
            groupManager.Create(groupA);
            var groupB = new Group()
            {
                Name = "Group B"
            };
            groupManager.Create(groupB);
            var groupC = new Group()
            {
                Name = "Group C"
            };
            groupManager.Create(groupC);
            groupManager.AddChild(groupA, groupB);
            groupManager.AddChild(groupA, groupC);
            var groupChildren = groupManager.GetChildren(groupA.Id);
            foreach (var child in groupChildren)
                Console.WriteLine("Child Name: " + child.Name);
            Assert.IsNotNull(groupChildren);

            //groupManager.Delete(groupA.Id);
        }


        [TestMethod]
        public void Can_Delete_Group_And_Reassign_Parent()
        {
            var groupA = new Group() { Name = "Group A" };
            groupManager.Create(groupA);

            var groupB = new Group() { Name = "Group B" };
            groupManager.Create(groupB);

            var groupC = new Group() { Name = "Group C" };
            groupManager.Create(groupC);

            var groupD = new Group() { Name = "Group D" };
            groupManager.Create(groupD);

            groupManager.AssignParent(groupB, groupA);
            groupManager.AssignParent(groupC, groupA);
            groupManager.AssignParent(groupD, groupC);

            var parentAChildren = groupManager.GetChildren(groupA.Id);
            Console.WriteLine("Root Parent: " + groupA.Name);
            Console.WriteLine();
            foreach (var parentAChild in parentAChildren)
            {
                Console.WriteLine(" --- Top Group ID: " + parentAChild.TopGroupId);
                Console.WriteLine(" --- Parent: " + parentAChild.ParentGroup.Name);
                Console.WriteLine(" --- Name: " + parentAChild.Name);
                Console.WriteLine();
                foreach (var childGroup in parentAChild.ChildGroups)
                {
                    Console.WriteLine(" --- Top Group ID: " + childGroup.TopGroupId);
                    Console.WriteLine(" ----- Parent: " + childGroup.ParentGroup.Name);
                    Console.WriteLine(" ----- Name: " + childGroup.Name);
                    Console.WriteLine();
                }
            }

            groupManager.Delete(groupC.Id);
            Console.WriteLine("Deleting Group C ... ");
            Console.WriteLine();

            parentAChildren = groupManager.GetChildren(groupA.Id);
            Console.WriteLine("Root Parent: " + groupA.Name);
            Console.WriteLine();
            foreach (var parentAChild in parentAChildren)
            {
                Console.WriteLine(" --- Top Group ID: " + parentAChild.TopGroupId);
                Console.WriteLine(" --- Parent: " + parentAChild.ParentGroup.Name);
                Console.WriteLine(" --- Name: " + parentAChild.Name);
                Console.WriteLine();
                foreach (var childGroup in parentAChild.ChildGroups)
                {
                    Console.WriteLine(" --- Top Group ID: " + childGroup.TopGroupId);
                    Console.WriteLine(" ----- Parent: " + childGroup.ParentGroup.Name);
                    Console.WriteLine(" ----- Name: " + childGroup.Name);
                    Console.WriteLine();
                }
            }

            groupManager.Delete(groupA.Id);
            Console.WriteLine("Deleting Group A ... ");


            Console.WriteLine("Top Group ID: " + groupB.TopGroupId);
            Console.WriteLine("Parent Id: " + (groupB.ParentGroupId.HasValue ? groupB.ParentGroupId.Value.ToString() : "No Parent!"));
            Console.WriteLine("Name: " + groupB.Name);
            Console.WriteLine();

            Console.WriteLine("Top Group ID: " + groupD.TopGroupId);
            Console.WriteLine("Parent Id: " + (groupD.ParentGroupId.HasValue ? groupD.ParentGroupId.Value.ToString() : "No Parent!"));
            Console.WriteLine("Name: " + groupD.Name);
            Console.WriteLine();
            //Assert.IsNotNull(groupManager.GetById(groupC.Id));
            //Assert.IsTrue(groupManager.Delete(groupC.Id));
            //Assert.IsNull(groupManager.GetById(groupC.Id));
        }

        [TestCleanup]
        public void CleanUpTests()
        {
            groupManager.DeleteAll();
        }
    }
}
