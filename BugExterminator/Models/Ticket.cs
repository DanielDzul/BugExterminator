using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BugExterminator.Models
{
    public class Ticket
    {
        public Ticket()
        {
            this.TicketAttachments = new HashSet<TicketAttachment>();
            this.TicketComments = new HashSet<TicketComment>();
            this.TicketHistories = new HashSet<TicketHistory>();
            this.TicketNotifications = new HashSet<TicketNotification>();
        }
        public int Id { get; set; }
        public string Title { get; set; }
        public string OwnerUserId { get; set; }
        public string AssignToUserId { get; set; }
        [AllowHtml]
        public string Description { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}",
       ApplyFormatInEditMode = true)]
        public DateTime Created { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}",
       ApplyFormatInEditMode = true)]
        public DateTime Updated { get; set; }
        public int? ProjectId { get; set; }
        public int?  TicketTypeId { get; set; }
        public int? TicketPriorityId { get; set; }
        public int? TicketStatusId { get; set; }

        public virtual Project Project { get; set; }
        public virtual TicketStatus TicketStatus { get; set; }
        public virtual TicketPriority TicketPriority { get; set; }
        public virtual TicketType TicketType { get; set; }

        public virtual ICollection<TicketComment> TicketComments { get; set; }
        public virtual ICollection<TicketAttachment> TicketAttachments { get; set; }
        public virtual ICollection<TicketHistory> TicketHistories { get; set; }
        public virtual ICollection<TicketNotification> TicketNotifications { get; set; }




    }
}