using BH.Data.Databases;
using BH.Models.DomainModels;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace BH.Data.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    public sealed class Configuration : DbMigrationsConfiguration<BH.Data.Databases.BHDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(BH.Data.Databases.BHDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            var userManager = new UserManager<User>(new UserStore<User>(new BHDbContext()));
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(new BHDbContext()));

            if (roleManager.FindByName("Admin") == null)
            {
                var adminRole = new IdentityRole()
                {
                    Name = "Admin"
                };
                roleManager.Create(adminRole);
            }

            if (roleManager.FindByName("User") == null)
            {
                var userRole = new IdentityRole()
                {
                    Name = "User"
                };
                roleManager.Create(userRole);
            }

            if (userManager.FindByName("admin") == null)
            {
                var defaultAdmin = new User()
                {
                    UserName = "admin",
                    Email = "alexander.burias@gmail.com",
                    EmailConfirmed = true,
                    FirstName = "Alexander",
                    LastName = "Burias"
                };
                userManager.Create(defaultAdmin, "adminpass123");

                userManager.AddToRole(defaultAdmin.Id, "Admin");
            }
        }
    }
}
