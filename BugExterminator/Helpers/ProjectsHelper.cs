
using BugExterminator.Models;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;


namespace BugExterminator.Helpers
{
    public class ProjectsHelper
    {
        private static ApplicationDbContext db = new ApplicationDbContext();

        public bool IsUserOnProject(string userId, int projectId)
        {
            var project = db.Project.Find(projectId);
            var flag = project.Users.Any(u => u.Id == userId);
            return (flag);
        }

        public ICollection<Project> ListUserProjects(string userId)
        {
            var user = db.Users.Find(userId);

            var projects = user.Projects.ToList();
            return (projects);
        }

        public void AddUserToProject(string userId, int projectId)
        {
            if (!IsUserOnProject(userId, projectId))
            {
                Project proj = db.Project.Find(projectId);
                var newUser = db.Users.Find(userId);

                proj.Users.Add(newUser);
                db.SaveChanges();
            }
        }

        public void RemoveUserFromProject(string userId, int projectId)
        {
            if (IsUserOnProject(userId, projectId))
            {
                Project proj = db.Project.Find(projectId);
                var delUser = db.Users.Find(userId);

                proj.Users.Remove(delUser);
                db.Entry(proj).State = EntityState.Modified; // just saves this obj instance.
                db.SaveChanges();
            }
        }
        
        public ICollection<ApplicationUser> UsersOnProject(int projectId)
        {
            return db.Project.Find(projectId).Users;
        }

        public List<string> UserIdsOnProject(int projectId)
        {
            var userIds = new List<string>();
            var users = db.Project.Find(projectId).Users;
            userIds.AddRange(users.Select(u => u.Id));
            return userIds;
        }

        public ICollection<ApplicationUser> UsersNotOnProject(int projectId)
        {
            return db.Users.Where(u => u.Projects.All(p => p.Id != projectId)).ToList();
        }

        public static string GetProjectNameById(int Id)
        {
            return db.Project.AsNoTracking().FirstOrDefault(p => p.Id == Id).Name;
        }
    }
}
    