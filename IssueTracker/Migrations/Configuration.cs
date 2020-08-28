namespace IssueTracker.Migrations
{
    using Amazon.CognitoIdentity.Model;
    using IssueTracker.Helpers;
    using IssueTracker.Models;
    using Microsoft.Ajax.Utilities;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Microsoft.Exchange.WebServices.Data;
    using Microsoft.JScript;
    using Polly;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using System.Runtime.Remoting.Contexts;
    using System.Web.Configuration;

    internal sealed class Configuration : DbMigrationsConfiguration<IssueTracker.Models.ApplicationDbContext>
    {
        private ProjectHelper projectHelper = new ProjectHelper();
        private RolesHelper rolesHelper = new RolesHelper();
        Random random = new Random();
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;

        }

            #region Roles Creation

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

            #region Users Creation
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
            var DemoAdminPassword = WebConfigurationManager.AppSettings["DemoAdminPassword"];
            var DemoPMPassword = WebConfigurationManager.AppSettings["DemoPMPassword"];
            var DemoDevPassword = WebConfigurationManager.AppSettings["DemoDevPassword"];
            var DemoSubPassword = WebConfigurationManager.AppSettings["DemoSubPassword"];


            if (!context.Users.Any(u => u.Email == "DemAd06@mailinator.com"))
            {
                userManager.Create(new ApplicationUser()
                {
                    Email = "DemAd06@mailinator.com",
                    UserName = "DemAd06@mailinator.com",
                    FirstName = "Admiral",
                    LastName = "Grace Hopper",
                    AvatarPath = "/Avatars/AdmHopperAvatar40x40.png"
                }, DemoAdminPassword); ;

                var userId = userManager.FindByEmail("DemAd06@mailinator.com").Id;

                userManager.AddToRole(userId, "Admin");
            };

            if (!context.Users.Any(u => u.Email == "DemPM14@mailinator.com"))
            {
                userManager.Create(new ApplicationUser()
                {
                    Email = "DemPM14@mailinator.com",
                    UserName = "DemPM14@mailinator.com",
                    FirstName = "Kamilah",
                    LastName = "Taylor",
                    AvatarPath = "/Avatars/KamilahTAvatar40x40.png"
                }, DemoPMPassword);

                var userId = userManager.FindByEmail("DemPM14@mailinator.com").Id;

                userManager.AddToRole(userId, "Project Manager");
            };

            if (!context.Users.Any(u => u.Email == "DemPM15@mailinator.com"))
            {
                userManager.Create(new ApplicationUser()
                {
                    Email = "DemPM15@mailinator.com",
                    UserName = "DemPM15@mailinator.com",
                    FirstName = "Katherine",
                    LastName = "Goble Johnson",
                    AvatarPath = "/Avatars/KathGJnsnAvatar40x40.png"
                }, DemoPMPassword);

                var userId = userManager.FindByEmail("DemPM15@mailinator.com").Id;

                userManager.AddToRole(userId, "Project Manager");
            };

            if (!context.Users.Any(u => u.Email == "DemDev15@mailinator.com"))
            {
                userManager.Create(new ApplicationUser()
                {
                    Email = "DemDev15@mailinator.com",
                    UserName = "DemDev15@mailinator.com",
                    FirstName = "Maria",
                    LastName = "Gutierrez",
                    AvatarPath = "/Avatars/MGutierrezAvatar40x40.png"
                }, DemoDevPassword);

                var userId = userManager.FindByEmail("DemDev15@mailinator.com").Id;

                userManager.AddToRole(userId, "Developer");
            };

            if (!context.Users.Any(u => u.Email == "DemDev16@mailinator.com"))
            {
                userManager.Create(new ApplicationUser()
                {
                    Email = "DemDev16@mailinator.com",
                    UserName = "DemDev16@mailinator.com",
                    FirstName = "Dorothy",
                    LastName = "Vaughan",
                    AvatarPath = "/Avatars/DorothyVaughanAvatar40x40.png"
                }, DemoDevPassword);

                var userId = userManager.FindByEmail("DemDev16@mailinator.com").Id;

                userManager.AddToRole(userId, "Developer");
            };

            if (!context.Users.Any(u => u.Email == "DemSub16@mailinator.com"))
            {
                userManager.Create(new ApplicationUser()
                {
                    Email = "DemSub16@mailinator.com",
                    UserName = "DemSub16@mailinator.com",
                    FirstName = "Dr.",
                    LastName = "Jamika Burge",
                    AvatarPath = "/Avatars/DrBurgeAvatar40x40.png"
                }, DemoSubPassword);

                var userId = userManager.FindByEmail("DemSub16@mailinator.com").Id;

                userManager.AddToRole(userId, "Submitter");
            };

            if (!context.Users.Any(u => u.Email == "DemSub17@mailinator.com"))
            {
                userManager.Create(new ApplicationUser()
                {
                    Email = "DemSub17@mailinator.com",
                    UserName = "DemSub17@mailinator.com",
                    FirstName = "Mary",
                    LastName = "Jackson",
                    AvatarPath = "/Avatars/MaryJacksonAvatar40x40.png"
                }, DemoSubPassword);

                var userId = userManager.FindByEmail("DemSub17@mailinator.com").Id;

                userManager.AddToRole(userId, "Submitter");
            };

            #endregion

            #region Role Assignment


            var adminId = userManager.FindByEmail("DemAd06@mailinator.com").Id;
            userManager.AddToRole(adminId, "Admin");

            var projId = userManager.FindByEmail("DemPM14@mailinator.com").Id;
            userManager.AddToRole(projId, "Project Manager");

            var projId2 = userManager.FindByEmail("DemPM15@mailinator.com").Id;
            userManager.AddToRole(projId2, "Project Manager");

            var devId = userManager.FindByEmail("DemDev15@mailinator.com").Id;
            userManager.AddToRole(devId, "Developer");

            var devId2 = userManager.FindByEmail("DemDev16@mailinator.com").Id;
            userManager.AddToRole(devId2, "Developer");

            var subId = userManager.FindByEmail("DemSub16@mailinator.com").Id;
            userManager.AddToRole(subId, "Submitter");

            var subId2 = userManager.FindByEmail("DemSub17@mailinator.com").Id;
            userManager.AddToRole(subId2, "Submitter");

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

            #region Ticket Seed PREP

            List<Ticket> ticketList = new List<Ticket>();
            List<ApplicationUser> projectManagers = new List<ApplicationUser>();
            List<ApplicationUser> developers = new List<ApplicationUser>();
            List<ApplicationUser> submitters = new List<ApplicationUser>();

            projectManagers.AddRange(rolesHelper.UsersInRole("Project Manager"));
            developers.AddRange(rolesHelper.UsersInRole("Developer"));
            submitters.AddRange(rolesHelper.UsersInRole("Submitter"));
            #endregion

            #region Assign Users to Projects BY ROLE
            foreach (var project in context.Projects)
            {
                foreach (var user in rolesHelper.UsersInRole("Admin"))
                {
                    projectHelper.AddUserToProject(user.Id, project.Id);
                }

                projectHelper.AddUserToProject(projectManagers[random.Next(projectManagers.Count)].Id, project.Id);

                var firstDev = developers[random.Next(developers.Count)].Id;
                var secondDev = developers[random.Next(developers.Count)].Id;
                while (firstDev == secondDev)
                {
                    secondDev = developers[random.Next(developers.Count)].Id;
                }
                projectHelper.AddUserToProject(firstDev, project.Id);
                projectHelper.AddUserToProject(secondDev, project.Id);

                var firstSub = submitters[random.Next(submitters.Count)].Id;
                var secondSub = submitters[random.Next(submitters.Count)].Id;
                while (firstSub == secondSub)
                {
                    secondSub = submitters[random.Next(submitters.Count)].Id;
                }
                projectHelper.AddUserToProject(firstSub, project.Id);
                projectHelper.AddUserToProject(secondSub, project.Id);
            }
            #endregion

            #region Ticket Seed

            foreach (var project in context.Projects.ToList())
            {
                projectManagers = projectHelper.ListUsersOnProjectInRole("Project Manager", project.Id);
                developers = projectHelper.ListUsersOnProjectInRole("Developer", project.Id);
                submitters = projectHelper.ListUsersOnProjectInRole("Submitter", project.Id);
                foreach (var type in context.TicketTypes.ToList())
                {
                    foreach (var status in context.TicketStatuses.ToList())
                    {
                        foreach (var priority in context.TicketPriorities.ToList())
                        {
                            var developerId = developers[random.Next(developers.Count)].Id;
                            if (status.Name == "Open")
                            {
                                developerId = null;
                            }

                            var resolved = false;
                            var archived = false;
                            if (status.Name == "Resolved")
                            {
                                resolved = true;
                            }
                            if (status.Name == "Archived" || project.IsArchived)
                            {
                                archived = true;
                            }
                            var newTicket = new Ticket()
                            {
                                ProjectId = project.Id,
                                TicketPriorityId = priority.Id,
                                TicketTypeId = type.Id,
                                TicketStatusId = status.Id,
                                SubmitterId = submitters[random.Next(submitters.Count)].Id,
                                DeveloperId = developerId,
                                Created = DateTime.Now,
                                Issue = $"This is a seeded ticket on the {project.Name} project.",
                                IssueDescription = $"This is a description of a seeded ticket on the {project.Name} project.",
                                IsResolved = resolved,
                                IsArchived = archived
                            };
                            ticketList.Add(newTicket);
                        }
                    }
                }
            }

            context.Tickets.AddRange(ticketList);
            context.SaveChanges();
            #endregion
        }

    }
}
