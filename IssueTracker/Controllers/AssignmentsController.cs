using IssueTracker.Helpers;
using IssueTracker.Models;
using Microsoft.Ajax.Utilities;
using Microsoft.Exchange.WebServices.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IssueTracker.Controllers
{
    public class AssignmentsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private RolesHelper roleHelper = new RolesHelper();
        // GET: Assignments
        public ActionResult ManageRoles()
        {
            //Use ViewBag to hold a multi select list of users in the system
            //new MultiSelectList(the data, "Id", "Email")
            ViewBag.UserIds = new MultiSelectList(db.Users, "Id", "Email");

            //Use ViewBag to hold a select list of Roles
            ViewBag.RoleName = new SelectList(db.Roles, "Name", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ManageRoles(List<string> userIds, string roleName)
        {
            if (userIds == null)
                return RedirectToAction("ManageRoles");

            foreach (var userId in userIds)
            {
                foreach (var role in roleHelper.ListUserRoles(userId).ToList())
                {
                    roleHelper.RemoveUserFromRole(userId, role);
                }
            }

            if (!string.IsNullOrEmpty(roleName))
            {
                roleHelper.AddUserToRole(userId, roleName);
            }

            return RedirectToAction("ManageRoles");
        }

        public ActionResult ManageUserRoles()
        {
            return View();
        }
    }
}