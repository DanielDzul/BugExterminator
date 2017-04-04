
using BugExterminator.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace BugExterminator.Helpers
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
                tickets = db.Ticket.ToList();

            else if (myRoles.Any(role => role == "Project Manager"))           
                tickets = db.Users.Find(userId).Projects.SelectMany(t => t.Ticket).ToList();

            else if (myRoles.Any(role => role == "Developer"))
                tickets = db.Ticket.Where(t => t.AssignToUserId == userId).ToList();

            else if (myRoles.Any(role => role == "Submitter"))
                tickets = db.Ticket.Where(t => t.OwnerUserId == userId).ToList();

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
                        Created = DateTime.Now,
                        UserId = userId
                    };

                    //Create a new TicketHistory entry
                    db.TicketHistory.Add(ticketHistory);
                }
            }
            db.SaveChanges();
        }


        public static string GetTicketPriorityNameById(int Id)
        {
            return db.TicketPriority.AsNoTracking().FirstOrDefault(tp => tp.Id == Id).Name;
        }
        public static string GetTicketStatusNameById(int Id)
        {
            return db.TicketStatus.AsNoTracking().FirstOrDefault(tp => tp.Id == Id).Name;
        }

        public static string GetTicketTypeNameById(int Id)
        {
            return db.TicketType.AsNoTracking().FirstOrDefault(tp => tp.Id == Id).Name;
        }

    }
}