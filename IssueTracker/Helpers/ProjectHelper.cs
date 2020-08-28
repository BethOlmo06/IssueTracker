using IssueTracker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.XPath;

namespace IssueTracker.Helpers

{
    public class ProjectHelper
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private RolesHelper roleHelper = new RolesHelper();


        public bool IsUserOnProject(string userId, int projectId)

        {
            Project project = db.Projects.Find(projectId);
            return project.Users.Any(u => u.Id == userId);
        }
        //Add one or more users to a project
        public void AddUserToProject(string userId, int projectId)
        {
            Project project = db.Projects.Find(projectId);
            var user = db.Users.Find(userId);
            project.Users.Add(user);
            db.SaveChanges();
        }
        //Remove one more users from a project
        public bool RemoveUserFromProject(string userId, int projectId)
        {
            Project project = db.Projects.Find(projectId);
            var user = db.Users.Find(userId);
            var result = project.Users.Remove(user);
            db.SaveChanges();
            return result;
            
        }

        //List users on a project
        public List<ApplicationUser> ListUsersOnProject(int projectId)
        {
            Project project = db.Projects.Find(projectId);
            var resultList = new List<ApplicationUser>();
            resultList.AddRange(project.Users);
            return resultList;
        }

        //List users NOT on a project
        public List<ApplicationUser> ListUsersNotOnProject(int projectId)
        {
            var resultList = new List<ApplicationUser>();
            foreach (var user in db.Users.ToList())
            {
                if(!IsUserOnProject(user.Id, projectId))
                {
                    resultList.Add(user);
                }
            }
            return resultList;
        }
        

        //List users on project that occupy a specific role
        public List<ApplicationUser> ListUsersOnProjectInRole(string roleName, int projectId)
        {
            var userList = ListUsersOnProject(projectId);
            var resultList = new List<ApplicationUser>();
            foreach(var user in userList)
            {
                if(roleHelper.IsUserInRole(user.Id, roleName))
                {
                    resultList.Add(user);
                }
            }
            return resultList;
        }

        //List projects for a specific user
        public List<Project> ListUserProjects(string userId)
        {
            var user = db.Users.Find(userId);
            var resultList = new List<Project>();
            resultList.AddRange(user.Projects);
            return resultList;
        }

    }
}