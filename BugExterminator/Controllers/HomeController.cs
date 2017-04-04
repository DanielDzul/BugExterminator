using BugExterminator.Helpers;
using BugExterminator.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BugExterminator.Controllers
{
    public class HomeController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        public ProjectsHelper ph = new ProjectsHelper();
        public UserRolesHelper uh = new UserRolesHelper();
        public TicketHelper th = new TicketHelper();

        public ActionResult LandingPage()
        {
            //TODO:work on landing page
            return View();
        }

        public ActionResult Index()
        {
            return View();
        }
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
        public ActionResult AdminView()
        {
            var UserInRoles = uh.UsersInRole(User.Identity.GetUserId()).ToList();
            //dynamic Mymodel = new ExpandoObject();
            //Mymodel = new ApplicationUser();
            //Mymodel = new Project();
            //Mymodel = new Tickets();
            //Mymodel = new TicketComment();
            //Mymodel = new TicketAttachment();
            ViewBag.projects = db.Project;
            ViewBag.tickets = db.Ticket;
            ViewBag.ticketattachments = db.TicketAttachment; 

            return View(UserInRoles);
        }

        public ActionResult AssignRoles()
        {
            
            ViewBag.userList = new MultiSelectList(db.Users, "Id", "DisplayName");
            ViewBag.roleList = new MultiSelectList(db.Roles, "Name","Name");
            ViewBag.projectList = new MultiSelectList(db.Project, "Id", "Name");
            ViewBag.ticketList = new MultiSelectList(db.Ticket, "Id", "Title");
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AssignRoles(string user, string roleName, List<string>userList, List<string>roleList, List<int>projectList, List<int>ticketList)
        {
            if(userList == null)
                return RedirectToAction("AdminView", "Home");
            if (roleList == null)
                return RedirectToAction("AdminView", "Home");
            foreach (var userId in userList)
                foreach (var role in roleList)
                    uh.AddUserToRole(userId, roleName);
                return RedirectToAction("AdminView", "Home");
        }

        public ActionResult RemoveRoles(string userId, string roleName)
        {
            var userList = uh.IsUserInRole(userId, roleName).ToString();
            ViewBag.RemoveRole = new MultiSelectList(userList, "Id", "Name");
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RemoveRoles(string userId, string roleName, List<string>RemoveRole)
        {
            if (RemoveRole == null)
                return RedirectToAction("AdminView", "Home");

            foreach (var user in RemoveRole)
                uh.RemoveUserFromRole(user, roleName );

            return RedirectToAction("AdminView", "Home");
        }

        public ActionResult AddUser(int projectId)
        {
            var userList = ph.UsersNotOnProject(projectId);
            ViewBag.UnassignedUsers = new MultiSelectList(userList, "Id", "Name");

            return RedirectToAction("AssignRoles","Home");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddUser(int projectId, List<string>UnassignedUsers)
        {
            if (UnassignedUsers == null)
                return RedirectToAction("AdminView","Home");

            foreach (var userId in UnassignedUsers)
                ph.AddUserToProject(userId, projectId);

            return RedirectToAction("AdminView", "Home");
        }
        
        public ActionResult RemoveUser(int projectId)
        {
            var userList = ph.UsersOnProject(projectId);
            ViewBag.AssignUsers = new MultiSelectList(userList, "Id", "Name");

            return View(db.Project.FirstOrDefault(p => p.Id == projectId));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RemoveUser(int projectId, List<string>AssignUser)
        {
            if(AssignUser == null)
                return RedirectToAction("AdminView", "Home");

            foreach (var userId in AssignUser)
                ph.RemoveUserFromProject(userId, projectId);

            return RedirectToAction("AdminView", "Home");         
        }

        public ActionResult ReassignUser(int projectId)
        {
            var AssignedUserId = ph.UsersOnProject(projectId);
            ViewBag.ReassignUser = new MultiSelectList(AssignedUserId, "Id", "Name");
             
            return View(db.Project.FirstOrDefault(p => p.Id == projectId));
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ReassignUser(int projectId, List<string> ReassignUser)
        {
            foreach (var userId in ReassignUser)
                ph.RemoveUserFromProject(userId, projectId);
            if(ReassignUser == null)
            {
                foreach (var userId in ReassignUser)
                    ph.AddUserToProject(userId, projectId);
            }
            return RedirectToAction("AdminView", "Home");
        }
        public ActionResult ReassignUserByRoles(int projectId)
        {
            var users = db.Users;
            var alldevs = new List<ApplicationUser>();
            var allpms = new List<ApplicationUser>();

            foreach(var user in users)
            {
                if (uh.IsUserInRole(user.Id, "Developer"))
                    alldevs.Add(user);
                if (uh.IsUserInRole(user.Id, "Manager"))
                    allpms.Add(user);
            }

            var AssignedDeveloper = new List<string>();
            var AssignedManager = new List<string>();

            foreach(var user in alldevs)
            {
                if (ph.IsUserOnProject(user.Id, projectId))
                    AssignedDeveloper.Add(user.Id);
            }

            foreach (var pm in allpms)
            {
                if (ph.IsUserOnProject(pm.Id, projectId))
                    AssignedManager.Add(pm.Id);
            }

            ViewBag.ReassignDeveloper = new MultiSelectList(alldevs, "Id", "Name", AssignedDeveloper);
            ViewBag.ReassignManager = new MultiSelectList(allpms, "Id", "Name", AssignedManager);

            return View(db.Project.FirstOrDefault(p => p.Id == projectId));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ReassignUserByRoles(int projectId, List<string>ReassignDeveloper, List<string>ReassignManager)
        {
            foreach(var user in db.Users)
            {
                if(ph.IsUserOnProject(user.Id, projectId))
                {
                    if(uh.IsUserInRole(user.Id, "Developer") ||
                            uh.IsUserInRole(user.Id, "Manager"))
                        {
                        ph.RemoveUserFromProject(user.Id, projectId);
                        }
                } 

            }

            if(ReassignDeveloper != null)
            {
                foreach (var userId in ReassignDeveloper)
                    ph.AddUserToProject(userId, projectId);
            }
            
            if(ReassignManager != null)
            {
                foreach (var userId in ReassignManager)
                    ph.AddUserToProject(userId, projectId);
            } 
            return RedirectToAction("AdminView","Home");
        }
    }
}