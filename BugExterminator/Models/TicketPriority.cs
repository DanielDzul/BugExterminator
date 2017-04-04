using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BugExterminator.Models
{
    public class TicketPriority
    {
        public TicketPriority()
        {
            this.Ticket = new HashSet<Ticket>();
        }
        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Ticket> Ticket { get; set; }
    }
}