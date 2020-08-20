namespace IssueTracker.Migrations
{
    using IssueTracker.Helpers;
    using IssueTracker.Models;
    using Microsoft.Ajax.Utilities;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Microsoft.Exchange.WebServices.Data;
    using Microsoft.JScript;
    using Polly;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using System.Runtime.Remoting.Contexts;
    using System.Web.Configuration;

    internal sealed class Configuration : DbMigrationsConfiguration<IssueTracker.Models.ApplicationDbContext>
    {
       
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

            #region Roles Seed

        protected override void Seed(IssueTracker.Models.ApplicationDbContext context)
        {
            //  This method will be called after migrating to the latest version.
            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data.

            var roleManager = new RoleManager<IdentityRole>(
                new RoleStore<IdentityRole>(context));

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
            #endregion

            #region Users Seed
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
            var DemoAdminPassword = WebConfigurationManager.AppSettings["DemoAdminPassword"];
            var DemoPMPassword = WebConfigurationManager.AppSettings["DemoPMPassword"];
            var DemoDevPassword = WebConfigurationManager.AppSettings["DemoDevPassword"];
            var DemoSubPassword = WebConfigurationManager.AppSettings["DemoSubPassword"];

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

            if (!context.Users.Any(u => u.Email == "DemAd06@mailinator.com"))
            {
                userManager.Create(new ApplicationUser()
                {
                    Email = "DemAd06@mailinator.com",
                    UserName = "DemAd06@mailinator.com",
                    FirstName = "Demo",
                    LastName = "Admin",
                }, "DemoAdminPassword");

                var userId = userManager.FindByEmail("DemAd06@mailinator.com").Id;

                userManager.AddToRole(userId, "Admin");
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

            if (!context.Users.Any(u => u.Email == "DemPM14@mailinator.com"))
            {
                userManager.Create(new ApplicationUser()
                {
                    Email = "DemPM14@mailinator.com",
                    UserName = "DemPM14@mailinator.com",
                    FirstName = "Demo",
                    LastName = "PM",
                }, "DemoPMPassword");

                var userId = userManager.FindByEmail("DemPM14@mailinator.com").Id;

                userManager.AddToRole(userId, "Admin");
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

            if (!context.Users.Any(u => u.Email == "DemDev15@mailinator.com"))
            {
                userManager.Create(new ApplicationUser()
                {
                    Email = "DemDev15@mailinator.com",
                    UserName = "DemDev15@mailinator.com",
                    FirstName = "Demo",
                    LastName = "Dev",
                }, "DemoDevPassword");

                var userId = userManager.FindByEmail("DemDev15@mailinator.com").Id;

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

            if (!context.Users.Any(u => u.Email == "DemSub16@mailinator.com"))
            {
                userManager.Create(new ApplicationUser()
                {
                    Email = "DemSub16@mailinator.com",
                    UserName = "DemSub16@mailinator.com",
                    FirstName = "Demo",
                    LastName = "Sub",
                }, "DemoSubPassword");

                var userId = userManager.FindByEmail("DemSub16@mailinator.com").Id;

                userManager.AddToRole(userId, "Submitter");
            };

            #endregion

            context.SaveChanges();
            #region TicketType Seed
            context.TicketTypes.AddOrUpdate(
                tt => tt.Name,
                new TicketType() { Name = "Software" },
                new TicketType() { Name = "Hardware" },
                new TicketType() { Name = "UI" },
                new TicketType() { Name = "Defect" },
                new TicketType() { Name = "Feature Request" },
                new TicketType() { Name = "Other" }
                );

            #endregion

            #region TicketPriority Seed
            context.TicketPriorities.AddOrUpdate(
                tp => tp.Name,
                new TicketPriority() { Name = "Low" },
                new TicketPriority() { Name = "Medium" },
                new TicketPriority() { Name = "High" },
                new TicketPriority() { Name = "On Hold" }
                );

            #endregion

            #region TicketStatus Seed
            context.TicketStatuses.AddOrUpdate(
                ts => ts.Name,
                new TicketStatus() { Name = "Open" },
                new TicketStatus() { Name = "Assigned" },
                new TicketStatus() { Name = "Resolved" },
                new TicketStatus() { Name = "Reopened" },
                new TicketStatus() { Name = "Archived" }
                );

            #endregion

            #region Project Seed
            context.Projects.AddOrUpdate(
                p => p.Name,
                new Project() { Name = "Seed 1", Created = DateTime.Now.AddDays(-60), IsArchived = true },
                new Project() { Name = "Seed 2", Created = DateTime.Now.AddDays(-45) },
                new Project() { Name = "Seed 3", Created = DateTime.Now.AddDays(-30) },
                new Project() { Name = "Seed 4", Created = DateTime.Now.AddDays(-15) },
                new Project() { Name = "Seed 5", Created = DateTime.Now.AddDays(-7) }
                );

            #endregion

            context.SaveChanges();
            var userList = context.Users.ToList();
            var projectList = context.Projects.ToList();
            foreach (var project in projectList)
            {
                foreach (var user in userList)
                {
                    projectHelper.AddUserToProject(user.Id, project.Id);
                }
            }

            #region Tickets Seed
            #endregion


        }

    }
}
