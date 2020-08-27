using IssueTracker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity;
using IssueTracker.Helpers;
using System.Security.Policy;
using System.Net.Mail;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Amazon.SimpleEmail.Model;

namespace IssueTracker.Helpers
{
    public class TicketManager
    {
        private RolesHelper rolesHelper = new RolesHelper();
        private ApplicationDbContext db = new ApplicationDbContext();
        private HistoryHelper historyHelper = new HistoryHelper();
        EmailService svc = new EmailService();
        string from = "TrackIT app";
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


        public async Task EditedTicket(Ticket oldTicket, Ticket newTicket)
        {
            historyHelper.ManageHistories(oldTicket, newTicket);
           await ManageTicketNotifications(oldTicket, newTicket);
        }

        public async Task ManageTicketNotifications(Ticket oldTicket, Ticket newTicket)
        {

            var ticketAssigned = oldTicket.DeveloperId == "" && newTicket.DeveloperId != "";
            var ticketUnassigned = oldTicket.DeveloperId != "" && newTicket.DeveloperId == "";
            var ticketReassigned = oldTicket.DeveloperId != "" && newTicket.DeveloperId != "" && oldTicket.DeveloperId != newTicket.DeveloperId;

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
            var notification = new TicketNotification()
            {
                TicketId = newTicket.Id,
                IsRead = false,
                UserId = newTicket.DeveloperId,
                Created = DateTime.Now,
                Body = $"Please take note, {newTicket.Developer.FullName}; Support Ticket {newTicket.Id} has been assigned to you."
            };
            db.TicketNotifications.Add(notification);
            db.SaveChanges();

            try
            {
                var email = new MailMessage(from, notification.User.Email) {
                    Subject = "Ticket Assignment",
                    Body = notification.Body,
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
            var notification = new TicketNotification()
            {
                TicketId = oldTicket.Id,
                IsRead = false,
                UserId = oldTicket.DeveloperId,
                Created = DateTime.Now,
                Body = $"Please take note, {oldTicket.Developer.FullName}; you have been unassigned from Support Ticket {oldTicket.Id}."
            };
            db.TicketNotifications.Add(notification);
            db.SaveChanges();

            try
            {
                var email = new MailMessage(from, notification.User.Email) {
                    Subject = "Ticket Unassignment",
                    Body = notification.Body,
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

        public void AttachmentNotifications(Ticket ticket)
        {

            var newNotification = new TicketNotification()
            {
                TicketId = ticket.Id,
                UserId = ticket.DeveloperId,
                Created = DateTime.Now,
                Subject = $"Attachment added on the {ticket.Project.Name} project.",
                Body = $"Please take note, {ticket.Developer.FullName}; Support Ticket {ticket.Id} has a new attachment."
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
                Body = $"Please take note, {ticket.Developer.FullName}; Support Ticket {ticket.Id} has a new comment."
            };

            db.TicketNotifications.Add(newNotification);
            db.SaveChanges();
        }

    }

}