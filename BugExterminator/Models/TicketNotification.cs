using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace BugExterminator.Models
{
    public class TicketNotification
    {
        public int Id { get; set; }
        public int TicketId { get; set; }
        public string UserId { get; set; }
        [AllowHtml]
        public string  Detail { get; set; }
        public bool IsActive { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}",
       ApplyFormatInEditMode = true)]
        public DateTime GeneratedDt { get; set; }


        public virtual Ticket Ticket { get; set; }
        public virtual ApplicationUser User { get; set; }
    }
}