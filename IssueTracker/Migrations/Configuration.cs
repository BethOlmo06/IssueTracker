namespace IssueTracker.Migrations
{
    using IssueTracker.Models;
    using Microsoft.Ajax.Utilities;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Microsoft.JScript;
    using Polly;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using System.Runtime.Remoting.Contexts;

    internal sealed class Configuration : DbMigrationsConfiguration<IssueTracker.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        #region Seeded Users
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

            if (!context.Users.Any(u => u.Email == "sebastiana@mailinator.com"))
            {
                userManager.Create(new ApplicationUser()
                {
                    Email = "sebastiana@mailinator.com",
                    UserName = "sebastiana@mailinator.com",
                    FirstName = "Sebastian",
                    LastName = "Aravena",
                }, "Abc&123");

                var userId = userManager.FindByEmail("sebastiana@mailinator.com").Id;

                userManager.AddToRole(userId, "Developer");
            };

            if (!context.Users.Any(u => u.Email == "carieg@mailinator.com"))
            {
                userManager.Create(new ApplicationUser()
                {
                    Email = "carieg@mailinator.com",
                    UserName = "carieg@mailinator.com",
                    FirstName = "Carie",
                    LastName = "Guyah",
                }, "Abc&123");

                var userId = userManager.FindByEmail("carieg@mailinator.com").Id;

                userManager.AddToRole(userId, "Developer");
            };

            if (!context.Users.Any(u => u.Email == "admiralhopper@mailinator.com"))
            {
                userManager.Create(new ApplicationUser()
                {
                    Email = "admiralhopper@mailinator.com",
                    UserName = "admiralhopper@mailinator.com",
                    FirstName = "Admiral Grace",
                    LastName = "Hopper",
                }, "Abc&123");

                var userId = userManager.FindByEmail("admiralhopper@mailinator.com").Id;

                userManager.AddToRole(userId, "Developer");
            };



            if (!context.Users.Any(u => u.Email == "aliceog@mailinator.com"))
            {
                userManager.Create(new ApplicationUser()
                {
                    Email = "aliceog@mailinator.com",
                    UserName = "aliceog@mailinator.com",
                    FirstName = "Alice",
                    LastName = "Guilfoyle",
                }, "Abc&123");

                var userId = userManager.FindByEmail("aliceog@mailinator.com").Id;

                userManager.AddToRole(userId, "Submitter");
            };

            if (!context.Users.Any(u => u.Email == "gener@mailinator.com"))
            {
                userManager.Create(new ApplicationUser()
                {
                    Email = "gener@mailinator.com",
                    UserName = "gener@mailinator.com",
                    FirstName = "Gene",
                    LastName = "Razz",
                }, "Abc&123");

                var userId = userManager.FindByEmail("gener@mailinator.com").Id;

                userManager.AddToRole(userId, "Submitter");
            };

            if (!context.Users.Any(u => u.Email == "philipj@mailinator.com"))
            {
                userManager.Create(new ApplicationUser()
                {
                    Email = "philipj@mailinator.com",
                    UserName = "philipj@mailinator.com",
                    FirstName = "Phil",
                    LastName = "Black",
                }, "Abc&123");

                var userId = userManager.FindByEmail("philipj@mailinator.com").Id;

                userManager.AddToRole(userId, "Submitter");
            };

            if (!context.Users.Any(u => u.Email == "ourlucieb@mailinator.com"))
            {
                userManager.Create(new ApplicationUser()
                {
                    Email = "ourlucieb@mailinator.com",
                    UserName = "ourlucieb@mailinator.com",
                    FirstName = "Lucie",
                    LastName = "Black",
                }, "Abc&123");

                var userId = userManager.FindByEmail("ourlucieb@mailinator.com").Id;

                userManager.AddToRole(userId, "Submitter");
            };



            if (!context.Users.Any(u => u.Email == "declanc@mailinator.com"))
            {
                userManager.Create(new ApplicationUser()
                {
                    Email = "declanc@mailinator.com",
                    UserName = "declanc@mailinator.com",
                    FirstName = "Declan",
                    LastName = "Crommett",
                }, "Abc&123");

                var userId = userManager.FindByEmail("declanc@mailinator.com").Id;

                userManager.AddToRole(userId, "Project Manager");
            };

            if (!context.Users.Any(u => u.Email == "tifft@mailinator.com"))
            {
                userManager.Create(new ApplicationUser()
                {
                    Email = "tifft@mailinator.com",
                    UserName = "tifft@mailinator.com",
                    FirstName = "Tiffany",
                    LastName = "Thomas, MBA, PMP",
                }, "Abc&123");

                var userId = userManager.FindByEmail("tifft@mailinator.com").Id;

                userManager.AddToRole(userId, "Project Manager");
            };

            if (!context.Users.Any(u => u.Email == "Okiboricua63@mailinator.com"))
            {
                userManager.Create(new ApplicationUser()
                {
                    Email = "Okiboricua63@mailinator.com",
                    UserName = "Okiboricua63@mailinator.com",
                    FirstName = "Orlando J",
                    LastName = "Olmo, MBA, PMP",
                }, "Abc&123");

                var userId = userManager.FindByEmail("Okiboricua63@mailinator.com").Id;

                userManager.AddToRole(userId, "Project Manager");
            };
        
        #endregion

        context.SaveChanges();
        #region TicketType Seed
        context.TicketTypes.AddOrUpdate(
            tt => tt.Name,
            new TicketType() { Name = "Software"},
            new TicketType() { Name = "Hardware"},
            new TicketType() { Name = "UI"},
            new TicketType() { Name = "Defect"},
            new TicketType() { Name = "Feature Request"},
            new TicketType() { Name = "Other"}
            );

        #endregion

        #region TicketPriority Seed
        context.TicketPriorities.AddOrUpdate(
            tp => tp.Name,
            new TicketPriority() { Name = "Low"},
            new TicketPriority() { Name = "Medium"},
            new TicketPriority() { Name = "High"},
            new TicketPriority() { Name = "On Hold"}
            );

        #endregion

        #region TicketStatus Seed
        context.TicketStatuses.AddOrUpdate(
            ts => ts.Name,
            new TicketStatus() { Name = "Open"},
            new TicketStatus() { Name = "Assigned"},
            new TicketStatus() { Name = "Resolved"},
            new TicketStatus() { Name = "Reopened"},
            new TicketStatus() { Name = "Archived"}
            );

        #endregion

        #region Project Seed
        context.Projects.AddOrUpdate(
            p => p.Name,
            new Project() { Name = "Seed 1", Created = DateTime.Now.AddDays(-60), IsArchived = true},
            new Project() { Name = "Seed 2", Created = DateTime.Now.AddDays(-45)},
            new Project() { Name = "Seed 3", Created = DateTime.Now.AddDays(-30)},
            new Project() { Name = "Seed 4", Created = DateTime.Now.AddDays(-15)},
            new Project() { Name = "Seed 5", Created = DateTime.Now.AddDays(-7)}
            );

        #endregion
        }
    }
}
