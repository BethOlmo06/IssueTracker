using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IssueTracker.Models;
using Microsoft.AspNet.Identity;

namespace IssueTracker.Controllers
{
    
    public class HomeController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        [Authorize]
        public ActionResult Index()
        {
            return View(db.Users.Find(User.Identity.GetUserId()));
        }
        [Authorize]
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}