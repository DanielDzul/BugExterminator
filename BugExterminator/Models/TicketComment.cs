using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BugExterminator.Models
{
    public class TicketComment
    {
        public int Id { get; set; }
        [AllowHtml]
        public string Comment { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}",
       ApplyFormatInEditMode = true)]
        public DateTimeOffset Created { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}",
       ApplyFormatInEditMode = true)]
        public DateTimeOffset? Updated { get; set; }
        public int TicketId { get; set; }
        public string UserId { get; set; }
        
        public virtual Ticket Ticket { get; set; }
        public virtual ApplicationUser User { get; set; }
    }
}