using BugTracker_FridayNights.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BugTracker_FridayNights.Helpers
{
    public class UserHelper
    {
        private static ApplicationDbContext db = new ApplicationDbContext();
        public static string GetDisplayNameFromId(string Id)
        {
            if (string.IsNullOrEmpty(Id))
                return db.Users.FirstOrDefault(t => t.Email == "unassigned@mailinator.com").DisplayName;
            else
                return db.Users.Find(Id).DisplayName;
        }
    }
}