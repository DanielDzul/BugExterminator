using BugTracker_FridayNights.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BugTracker_FridayNights.Helpers
{
    public class TicketHelper
    {
        private UserRolesHelper roleHelper = new UserRolesHelper();
        private static ApplicationDbContext db = new ApplicationDbContext();

        //Help to abstract away the ticket retrieval process and keep it out of the controller
        public List<Ticket> GetUserTickets(string userId)
        {
            var tickets = new List<Ticket>();
            var myRoles = roleHelper.ListUserRoles(userId).ToList();

            if (myRoles.Any(role => role == "Admin"))
                tickets = db.Tickets.ToList();

            else if (myRoles.Any(role => role == "ProjectManager"))           
                tickets = db.Users.Find(userId).Projects.SelectMany(t => t.Tickets).ToList();

            else if (myRoles.Any(role => role == "Developer"))
                tickets = db.Tickets.Where(t => t.AssignedToUserID == userId).ToList();

            else if (myRoles.Any(role => role == "Submitter"))
                tickets = db.Tickets.Where(t => t.OwnerUserId == userId).ToList();

            return tickets;
        }

        public void AddTicketHistory(Ticket oldTicket, Ticket newTicket, string userId)
        {
            //Cycle over the Ticket properties and compare their values..
            foreach (var prop in typeof(Ticket).GetProperties())
            {
                if (prop.GetValue(oldTicket) == null)
                    prop.SetValue(oldTicket, "", null);
                if (prop.GetValue(newTicket) == null)
                    prop.SetValue(newTicket, "", null);

                //If the value of a Ticket property has changed...
                if (prop.GetValue(oldTicket).ToString() != prop.GetValue(newTicket).ToString()) 
                {
                    //Create a TicketHistory object to populate a new record
                    var ticketHistory = new TicketHistory
                    {
                        TicketId = oldTicket.Id,
                        Property = prop.Name.ToString(),
                        OldValue = prop.GetValue(oldTicket).ToString(),
                        NewValue = prop.GetValue(newTicket).ToString(),
                        Changed = DateTimeOffset.Now,
                        UserId = userId
                    };

                    //Create a new TicketHistory entry
                    db.TicketHistorys.Add(ticketHistory);
                }
            }
            db.SaveChanges();
        }


        public static string GetTicketPriorityNameById(int Id)
        {
            return db.TicketPriorities.AsNoTracking().FirstOrDefault(tp => tp.Id == Id).Name;
        }
        public static string GetTicketStatusNameById(int Id)
        {
            return db.TicketStatuses.AsNoTracking().FirstOrDefault(tp => tp.Id == Id).Name;
        }

        public static string GetTicketTypeNameById(int Id)
        {
            return db.TicketTypes.AsNoTracking().FirstOrDefault(tp => tp.Id == Id).Name;
        }

    }
}