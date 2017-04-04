using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BugExterminator.Models
{
    public class TicketHistory
    {
        public int Id { get; set; }
        public int TicketId { get; set; }
        public string Property { get; set; }
        public string OldValue { get; set; }
        public string NewValue { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}",
       ApplyFormatInEditMode = true)]
        public DateTime Created { get; set; }
        public string UserId { get; set; }
        public string Change { get; set; }

        public virtual Ticket Ticket { get; set; }
        public virtual ApplicationUser User { get; set; }
    }
}