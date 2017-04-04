
using System.Threading.Tasks;
using System.Net.Mail;
using System.Configuration;
using System.Linq;
using System.Data.Entity;
using System.Text;
using System.Web.Configuration;
using System;
using BugExterminator.Models;

namespace BugExterminator.Helpers
{
    public class NotificationHelper
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public async Task HandleNotifications(Ticket oldTicket, Ticket newTicket)
        {
            await GenerateEmailNotification(oldTicket, newTicket);
            InsertNotificationTable(newTicket);
        }

        public async Task GenerateEmailNotification(Ticket oldTicket, Ticket newTicket)
        {
            var db = new ApplicationDbContext();
            var unassignedEmail = db.Users.FirstOrDefault(t => t.Id == oldTicket.AssignToUserId).Email;

            //Lets handle some scenarios where developers are notified of being assigned/unassigned to Tickets
            if(unassignedEmail == "unassigned@mailinator.com" && !string.IsNullOrEmpty(newTicket.AssignToUserId))
            {
                var myEmail = new EmailModel();
                myEmail.FromEmail = WebConfigurationManager.AppSettings["emailfrom"];
                myEmail.ToEmail = db.Users.AsNoTracking().FirstOrDefault(u => u.Id == newTicket.AssignToUserId).Email;
                myEmail.Subject = string.Format("You have been assigned to Ticket Id: {0}", oldTicket.Id);
                myEmail.Body = ComposeEmailBody(newTicket);

                await SendNotification(myEmail);
            }
        }

        public void InsertNotificationTable(Ticket ticket)
        {
            var tn = new TicketNotification();
            tn.Detail = ComposeEmailBody(ticket);
            tn.TicketId = ticket.Id ; //Test guys...
            tn.GeneratedDt = DateTime.Now;
            tn.UserId = ticket.AssignToUserId;

            db.TicketNotification.Add(tn);
            try
            {
                db.SaveChanges();
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public string ComposeEmailBody(Ticket ticket)
        {
            var emailBody = new StringBuilder();
            emailBody.AppendFormat("<b>Ticket Title:</b> {0}", ticket.Title);
            emailBody.AppendLine(" ");
            emailBody.AppendLine("<b>Ticket Description:</b>");
            emailBody.AppendFormat("<p>{0}</p>", ticket.Description);
            return emailBody.ToString();
        }

        public async Task SendNotification(EmailModel model)
        {
            var body = "<p>Email From: <b>{0}</b> <br /> ({1})</p><p>Message:</p><p>{2}</p>";
            var email = new MailMessage(model.FromEmail, model.ToEmail)
            {
                Subject = model.Subject,
                Body = string.Format(body, model.FromName, model.FromEmail, model.Body),
                IsBodyHtml = true
            };
            var svc = new NotificationSvc();
            await svc.SendAsync(email);
        }
    }
}