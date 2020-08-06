namespace IssueTracker.Migrations
{
    using IssueTracker.Models;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<IssueTracker.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(IssueTracker.Models.ApplicationDbContext context)
        {
            //  This method will be called after migrating to the latest version.
            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data.
            var roleManager = new RoleManager<IdentityRole>(
                new RoleStore<IdentityRole>(context));

            //Check whether a particular Role already exists in the DB.
            //If not, create it.
            if (!context.Roles.Any(r => r.Name == "Admin"))
            {
                roleManager.Create(new IdentityRole { Name = "Admin" });
            }

            if (!context.Roles.Any(r => r.Name == "Project Manager"))
            {
                roleManager.Create(new IdentityRole { Name = "Project Manager" });
            }

            if (!context.Roles.Any(r => r.Name == "Developer"))
            {
                roleManager.Create(new IdentityRole { Name = "Developer" });
            }

            if (!context.Roles.Any(r => r.Name == "Submitter"))
            {
                roleManager.Create(new IdentityRole { Name = "Submitter" });
            }


            var userManager = new UserManager<ApplicationUser>(
               new UserStore<ApplicationUser>(context));

            if (!context.Users.Any(u => u.Email == "betholmo@gmail.com"))
            {
                userManager.Create(new ApplicationUser()
                {
                    Email = "betholmo@gmail.com",
                    UserName = "betholmo@gmail.com",
                    FirstName = "Beth",
                    LastName = "Olmo",
                }, "OkiMomo06!");

                var userId = userManager.FindByEmail("betholmo@gmail.com").Id;

                userManager.AddToRole(userId, "Admin");
            };


            if (!context.Users.Any(u => u.Email == "beth.olmo@olmoweinman.com"))
            {
                userManager.Create(new ApplicationUser()
                {
                    Email = "beth.olmo@olmoweinman.com",
                    UserName = "beth.olmo@olmoweinman.com",
                    FirstName = "Beth",
                    LastName = "Olmo",
                }, "Abc&123");

                var userId = userManager.FindByEmail("beth.olmo@olmoweinman.com").Id;

                userManager.AddToRole(userId, "Developer");
            };


            if (!context.Users.Any(u => u.Email == "betholmo13@gmail.com"))
            {
                userManager.Create(new ApplicationUser()
                {
                    Email = "betholmo13@gmail.com",
                    UserName = "betholmo13@gmail.com",
                    FirstName = "Beth",
                    LastName = "Olmo",
                }, "Abc&123");

                var userId = userManager.FindByEmail("betholmo13@gmail.com").Id;

                userManager.AddToRole(userId, "Submitter");
            };

            if (!context.Users.Any(u => u.Email == "ojolmo@gmail.com"))
            {
                userManager.Create(new ApplicationUser()
                {
                    Email = "ojolmo@gmail.com",
                    UserName = "ojolmo@gmail.com",
                    FirstName = "Orlando J",
                    LastName = "Olmo, MBA, PMP",
                }, "Abc&123");

                var userId = userManager.FindByEmail("ojolmo@gmail.com").Id;

                userManager.AddToRole(userId, "Project Manager");
            };
        }
    }
}
