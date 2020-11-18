using IssueTracker.Models;
using System;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace IssueTracker.Helpers
{
    public class TicketManager
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private RolesHelper rolesHelper = new RolesHelper();
        private HistoryHelper historyHelper = new HistoryHelper();
        EmailService svc = new EmailService();
        string from = "TrackIT app<issuetrackerclientcare@gmail.com>";

     

        public bool CanEditTicket(int ticketId)
        {
            var userId = HttpContext.Current.User.Identity.GetUserId();
            var myRole = rolesHelper.ListUserRoles(userId).FirstOrDefault();
            switch (myRole)
            {
                case "Admin":
                    return true;

                case "Project Manager":
                    var user = db.Users.Find(userId);
                    return user.Projects.SelectMany(p => p.Tickets).Any(t => t.Id == ticketId);

                case "Developer":
                case "Submitter":
                    var ticket = db.Tickets.Find(ticketId);
                    if (ticket.DeveloperId == userId || ticket.SubmitterId == userId)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                default:
                    return false;
            }
        }

        public bool CanMakeComment(int ticketId)
        {
            var userId = HttpContext.Current.User.Identity.GetUserId();
            var myRole = rolesHelper.ListUserRoles(userId).FirstOrDefault();
            switch (myRole)
            {
                case "Admin":
                    return true;

                case "Project Manager":
                    var user = db.Users.Find(userId);
                    return user.Projects.SelectMany(p => p.Tickets).Any(t => t.Id == ticketId);

                case "Developer":
                case "Submitter":
                    var ticket = db.Tickets.Find(ticketId);
                    if (ticket.DeveloperId == userId || ticket.SubmitterId == userId)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                default:
                    return false;
            }
        }

        public List<Ticket> GetMyTickets()
        {
            var myTickets = new List<Ticket>();
            var userId = HttpContext.Current.User.Identity.GetUserId();
            var user = db.Users.Find(userId);
            var myRole = rolesHelper.ListUserRoles(userId).FirstOrDefault();
            
            switch (myRole)
            {
                case "Admin":
                    myTickets.AddRange(db.Tickets); //only allowing Admin to see or interact with archived projects
                    break;
                case "Project Manager":
                    myTickets.AddRange(user.Projects.Where(p => !p.IsArchived).SelectMany(p => p.Tickets));
                    break;
                case "Developer":
                    myTickets.AddRange(db.Tickets.Where(t => !t.IsArchived).Where(t => t.DeveloperId == userId));
                    break;
                case "Submitter":
                    myTickets.AddRange(db.Tickets.Where(t => !t.IsArchived).Where(t => t.SubmitterId == userId));
                    break;
            }
            return myTickets;
        }

        public List<Ticket> MyTopTickets()
        {
            var myTickets = new List<Ticket>();
            var userId = HttpContext.Current.User.Identity.GetUserId();
            var user = db.Users.Find(userId);
            var myRole = rolesHelper.ListUserRoles(userId).FirstOrDefault();

            switch (myRole)
            {
                case "Admin":
                    myTickets.AddRange(user.Projects.Where(p => !p.IsArchived).SelectMany(p => p.Tickets).Where(t => t.TicketPriority.Name == "High"));
                    break;
                case "Project Manager":
                    myTickets.AddRange(user.Projects.Where(p => !p.IsArchived).SelectMany(p => p.Tickets).Where(t => t.TicketPriority.Name == "High"));
                    break;
                case "Developer":
                    myTickets.AddRange(db.Tickets.Where(t => !t.IsArchived && !t.IsResolved && t.DeveloperId == userId && t.TicketPriority.Name == "High"));
                    break;
                case "Submitter":
                    myTickets.AddRange(db.Tickets.Where(t => !t.IsArchived && !t.IsResolved && t.SubmitterId == userId && t.TicketPriority.Name == "High"));
                    break;
            }
            return myTickets;
        }

        public List<Ticket> ListProjectTickets(int projectId)
        {
            var projTickets = new List<Ticket>();
            var project = db.Projects.Find(projectId);
            return projTickets;
        }


        public async Task EditedTicket(Ticket oldTicket, Ticket newTicket)
        {
            historyHelper.ManageHistories(oldTicket, newTicket);
           await ManageTicketNotifications(oldTicket, newTicket);
        }

        public async Task ManageTicketNotifications(Ticket oldTicket, Ticket newTicket)
        {

            var ticketAssigned = oldTicket.DeveloperId == null && newTicket.DeveloperId != null;
            var ticketUnassigned = oldTicket.DeveloperId != null && newTicket.DeveloperId == null;
            var ticketReassigned = oldTicket.DeveloperId != null && newTicket.DeveloperId != null && oldTicket.DeveloperId != newTicket.DeveloperId;

            if (ticketAssigned)
            {
                await AddAssignmentNotification(newTicket);
            }
            else if (ticketUnassigned)
            {
                await AddUnassignmentNotification(oldTicket);
            }
            else if (ticketReassigned)
            {
                await AddAssignmentNotification(newTicket);
                await AddUnassignmentNotification(oldTicket);
            }
        }

        private async Task AddAssignmentNotification(Ticket newTicket)
        {
            var notification = new TicketNotification
            {
                TicketId = newTicket.Id,
                IsRead = false,
                UserId = newTicket.DeveloperId,
                Created = DateTime.Now,
                Message = $"Please take note, {newTicket.Developer.FullName}; Support Ticket {newTicket.Id} has been assigned to you."
            };
            db.TicketNotifications.Add(notification);
            db.SaveChanges();

            var userId = notification.UserId;
            var userEmail = db.Users.Find(userId).Email;
           
            try
            {
                var email = new MailMessage(from, userEmail) {
                    Subject = "Ticket Assignment",
                    Body = notification.Message,
                    IsBodyHtml = true
                };
                await svc.SendAsync(email);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                await Task.FromResult(0);
            }

        }

        private async Task AddUnassignmentNotification(Ticket oldTicket)
        {
            var notification = new TicketNotification

            {
                TicketId = oldTicket.Id,
                IsRead = false,
                UserId = oldTicket.DeveloperId,
                Created = DateTime.Now,
                Message = $"Please take note, {oldTicket.DeveloperId}; you have been unassigned from Support Ticket {oldTicket.Id}."
            };
            db.TicketNotifications.Add(notification);
            db.SaveChanges();

            var userId = notification.UserId;
            var userEmail = db.Users.Find(userId).Email;

            try
            {
                var email = new MailMessage(from, notification.User.Email) {
                    Subject = "Ticket Unassignment",
                    Body = notification.Message,
                    IsBodyHtml = true
                };
                await svc.SendAsync(email);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                await Task.FromResult(0);
            }
            
        }

        public List<TicketNotification> GetUnreadNotifications()
        {
            var currentUserId = HttpContext.Current.User.Identity.GetUserId();
            return db.TicketNotifications.Where(t => t.UserId == currentUserId && !t.IsRead).ToList();
        }

        public void AttachmentNotifications(Ticket ticket)
        {

            var newNotification = new TicketNotification()
            {
                TicketId = ticket.Id,
                UserId = ticket.DeveloperId,
                Created = DateTime.Now,
                Subject = $"Attachment added on the {ticket.Project.Name} project.",
                Message = $"Please take note, {ticket.Developer.FullName}; Support Ticket {ticket.Id} has a new attachment."
            };

            db.TicketNotifications.Add(newNotification);
            db.SaveChanges();
        }

        public void CommentNotifications(Ticket ticket)
        {

            var newNotification = new TicketNotification()
            {
                TicketId = ticket.Id,
                UserId = ticket.DeveloperId,
                Created = DateTime.Now,
                Subject = $"New Comment added on the {ticket.Project.Name} project.",
                Message = $"Please take note, {ticket.Developer.FullName}; Support Ticket {ticket.Id} has a new comment."
            };

            db.TicketNotifications.Add(newNotification);
            db.SaveChanges();
        }

    }

}