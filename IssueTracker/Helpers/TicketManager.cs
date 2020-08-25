using IssueTracker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity;
using IssueTracker.Helpers;
using System.Security.Policy;

namespace IssueTracker.Helpers
{
    public class TicketManager
    {
        private RolesHelper rolesHelper = new RolesHelper();
        private ApplicationDbContext db = new ApplicationDbContext();
        private HistoryHelper historyHelper = new HistoryHelper();
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


        public void EditedTicket(Ticket oldTicket, Ticket newTicket)
        {
            historyHelper.ManageHistories(oldTicket, newTicket);
            ManageTicketNotifications(oldTicket, newTicket);
        }

        public void ManageTicketNotifications(Ticket oldTicket, Ticket newTicket)
        {
            if (oldTicket.DeveloperId != newTicket.DeveloperId && newTicket.DeveloperId != null)
            {
                var newNotification = new TicketNotification()
                {
                    TicketId = newTicket.Id,
                    UserId = newTicket.DeveloperId,
                    Created = DateTime.Now,
                    Subject = $"New Ticket assignment on the {newTicket.Project.Name} project.",
                    Body = $"Please take note, {newTicket.Developer.FullName}; Support Ticket {newTicket.Id} has been assigned to you."
                };

                db.TicketNotifications.Add(newNotification);
                db.SaveChanges();
            }



            //TODO Case 2: Ticket UN-assignment


            //TODO Case 3: Ticket RE-assignment => this could create two notifications - one for each Dev


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