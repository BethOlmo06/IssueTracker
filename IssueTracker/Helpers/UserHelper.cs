using IssueTracker.Models;
using Microsoft.AspNet.Identity;
using Microsoft.Exchange.WebServices.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IssueTracker.Helpers
{
    public class UserHelper
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public string GetFullName(string userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                return "";
            }
            var user = db.Users.Find(userId);
            var firstName = user.FirstName;
            var lastName = user.LastName;
            return firstName + " " + lastName;
        }

        public string LastNameFirst(string userId)
        {
            var user = db.Users.Find(userId);
            return user.FullName;
        }

        //public string GetUserRole()
        //{
       
            
        //    if (HttpContext.Current.User == null)
        //    {
        //        return "No Role";
        //    }
        //    var userId = HttpContext.Current.User.Identity.GetUserId();
        //    return userId;

        //}

        //public string GetUserRole(string userId)
        //{
        //    return null;
        //}
    }
}