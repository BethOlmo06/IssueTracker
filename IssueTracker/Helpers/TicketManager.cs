//using IssueTracker.Models;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web;
//using Microsoft.AspNet.Identity;
//using IssueTracker.Helpers;

//namespace IssueTracker.Helpers
//{
//    public class TicketManager
//    {
//        private ApplicationDbContext db = new ApplicationDbContext();
//        private ProjectHelper projectHelper = new ProjectHelper();
//        private RolesHelper rolesHelper = new RolesHelper();
//        private UserHelper userHelper = new UserHelper();

        
//        public List<Ticket>GetProjectTickets()
//        //{
//        //    var userId = User.Identity.GetUserId();
//        //    var user = db.Users.Find(userId);
//        //    var ticketList = new List<Ticket>();
//        //    ticketList = user.Projects.SelectMany(p => p.Tickets).ToList();
//        //    return ticketList;

//        //}
        
//        public List<Ticket> GetMyTickets(string userId)
//        {
//            var tickets = new List<Ticket>();
//            var myRole = rolesHelper.ListUserRoles(userId).FirstOrDefault();

//            switch (myRole)
//            {
//                case "Admin":
//                    tickets.AddRange(db.Tickets);
//                    break;
//                case "ProjectManager":
//                    tickets.AddRange(db.Users.Find(userId).Projects.SelectMany(p => p.Tickets));
//                    break;
//                case "Developer":
//                    tickets.AddRange(db.Tickets.Where(t => t.DeveloperId == userId));
//                    break;
//                case "Submitter":
//                    tickets.AddRange(db.Tickets.Where(t => t.SubmitterId == userId));
//                    break;
//            }

//            return tickets;
//        }
//    }
//}