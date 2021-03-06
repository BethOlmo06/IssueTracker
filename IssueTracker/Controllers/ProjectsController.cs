﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using IssueTracker.Helpers;
using IssueTracker.Models;
using IssueTracker.ViewModels;
using Amazon.DynamoDBv2;

namespace IssueTracker.Controllers
{
    [Authorize]
    public class ProjectsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private UserHelper userHelper = new UserHelper();
        private RolesHelper rolesHelper = new RolesHelper();
        private ProjectHelper projectHelper = new ProjectHelper();


        // GET: Projects
        public ActionResult Index()
        {
            return View(db.Projects.ToList());
        }

        // GET: Projects/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Project project = db.Projects.Find(id);
            if (project == null)
            {
                return HttpNotFound();
            }

            ViewBag.TicketPriorityId = new SelectList(db.TicketPriorities, "Id", "Name");
            ViewBag.TicketTypeId = new SelectList(db.TicketTypes, "Id", "Name");

            return View(project);
        }

        // GET: Projects/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Projects/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        
        public ActionResult Create([Bind(Include = "Id,Name")] Project project)
        {
            if (ModelState.IsValid)
            {
                project.Created = DateTime.Now;
                project.IsArchived = false;
                db.Projects.Add(project);
                db.SaveChanges();
                projectHelper.AddUserToProject(User.Identity.GetUserId(), project.Id);
                return RedirectToAction("Index");
            }

            return View(project);
        }
        
        public ActionResult ProjectWizard()
        {
            ViewBag.ProjectManagerId = new SelectList(rolesHelper.UsersInRole("Project Manager" ), "Id", "FullName");
            ViewBag.DeveloperIds = new MultiSelectList(rolesHelper.UsersInRole("Developer"), "Id", "FullName");
            ViewBag.SubmitterIds = new MultiSelectList(rolesHelper.UsersInRole("Submitter"), "Id", "FullName");
            ViewBag.Errors = "";
            var model = new ProjectWizardVM();

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ProjectWizard(ProjectWizardVM model)
        {
            #region Fail Cases
            ViewBag.Erros = "";
            if(model.ProjectManagerId == null)
            {
                ViewBag.Errors += "<p>Please select a Project Manger.</p>";
            }
            if(model.DeveloperIds.Count == 0)
            {
                ViewBag.Erros += "<p>Please select at least one Developer.</p>";
            }
            if(model.SubmitterIds.Count == 0)
            {
                ViewBag.Error += "<p>Please select at least one Submitter.</p>";
            }
            
            #endregion

            if (ModelState.IsValid)
            {
                Project project = new Project();
                project.Name = model.Name;
                project.Created = DateTime.Now;
                project.IsArchived = false;
                db.Projects.Add(project);
                db.SaveChanges();
                projectHelper.AddUserToProject(User.Identity.GetUserId(), project.Id);

                projectHelper.AddUserToProject(model.ProjectManagerId, project.Id);
                foreach(var userId in model.DeveloperIds)
                {
                    projectHelper.AddUserToProject(userId, project.Id);
                }
                foreach (var userId in model.SubmitterIds)
                {
                    projectHelper.AddUserToProject(userId, project.Id);
                }

                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.ProjectManagerId = new SelectList(rolesHelper.UsersInRole("Project Manager"), "Id", "FullName");
                ViewBag.DeveloperIds = new MultiSelectList(rolesHelper.UsersInRole("Developer"), "Id", "FullName");
                ViewBag.SubmitterIds = new MultiSelectList(rolesHelper.UsersInRole("Submitter"), "Id", "FullName");
                return View(model);
            }
        }


        // GET: Projects/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Project project = db.Projects.Find(id);
            if (project == null)
            {
                return HttpNotFound();
            }
            return View(project);
        }

        // POST: Projects/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        
        public ActionResult Edit([Bind(Include = "Id,Name,Created,IsArchived")] Project project)
        {
            if (ModelState.IsValid)
            {
                db.Entry(project).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(project);
        }

        // GET: Projects/Delete/5
        
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Project project = db.Projects.Find(id);
            if (project == null)
            {
                return HttpNotFound();
            }
            return View(project);
        }

        // POST: Projects/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        
        //public ActionResult DeleteConfirmed(int id)
        //{
        //    Project project = db.Projects.Find(id);
        //    db.Projects.Remove(project);
        //    db.SaveChanges();
        //    return RedirectToAction("Index");
        //}

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
