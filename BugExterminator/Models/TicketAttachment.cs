using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace BugExterminator.Models
{
    public class TicketAttachment
    {
        public int Id { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}",
       ApplyFormatInEditMode = true)]
        public DateTimeOffset Created { get; set; }
        [AllowHtml]
        public string Description { get; set; }
        public int TicketId { get; set; }
        public string UserId { get; set; }
        public string FilePath { get; set; }

        public virtual Ticket Ticket { get; set; }
        public virtual ApplicationUser User { get; set; }
    }
}