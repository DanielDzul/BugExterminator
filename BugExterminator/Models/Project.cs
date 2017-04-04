using BugExterminator.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BugExterminator.Models
{
    public class Project
    { 
        public Project()
        {
            this.Ticket = new HashSet<Ticket>();
            this.Users = new HashSet<ApplicationUser>();
        }     
        public int Id { get; set; }
        public string Name { get; set; }
        [UIHint("tinymce_jquery_full"), AllowHtml]
        public string  Description { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}",
               ApplyFormatInEditMode = true)]
        public DateTimeOffset Created { get; set; }

        public virtual ICollection<Ticket> Ticket { get; set; }
        public virtual ICollection<ApplicationUser> Users { get; set; }
    }   
}