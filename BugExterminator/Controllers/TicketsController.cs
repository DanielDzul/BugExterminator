using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BugExterminator.Models;
using Microsoft.AspNet.Identity;
using BugExterminator.Helpers;

namespace BugExterminator.Controllers
{
    public class TicketsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private ProjectsHelper ph = new ProjectsHelper();
        private UserRolesHelper rh = new UserRolesHelper();
        

        [Authorize(Roles = "Admin, Project Manager")]
        // GET: Tickets
        public ActionResult Index()
        {
            var tickets = db.Ticket.Include(t => t.Project).ToList();
            return View(tickets);
        }

        [Authorize]
        public ActionResult MyIndex()
        {
            var userId = User.Identity.GetUserId();
            var tickets = db.Ticket.Include(t => t.Project).Where(t => t.AssignToUserId == userId).ToList();   
            return View("Index", tickets);
        }

        // GET: Tickets/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ticket ticket = db.Ticket.Find(id);
            if (ticket == null)
            {
                return HttpNotFound();
            }
            return View(ticket);
        }
        [Authorize]
        // GET: Tickets/Create
        public ActionResult Create()
        {
            var ticket = new Ticket();
            ticket.AssignToUserId = User.Identity.GetUserId();
            ViewBag.DeveloperId = new SelectList(rh.UsersInRole("Developer"), "Id", "DisplayName");
            ViewBag.ProjectId = new SelectList(db.Project, "Id", "Name");
            ViewBag.TypeId = new SelectList(db.TicketType, "Id","Name");
            ViewBag.StatusId = new SelectList(db.TicketStatus, "Id", "Name");
            ViewBag.PriorityId = new SelectList(db.TicketPriority, "Id", "Name");
            
            return View();
        }

        // POST: Tickets/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Title,,AssignToUserId,Description,ProjectId,TicketTypeId,TicketPriorityId,TicketStatusId")] Ticket ticket, string DeveloperId, string TypeId, string StatusId, string PriorityId, string ProjectId)
        {
            if (ModelState.IsValid)
            {
                ticket.Created = DateTime.Now;
                ticket.Updated = DateTime.Now;
                ticket.AssignToUserId = DeveloperId;
                ticket.OwnerUserId = User.Identity.GetUserId();
                db.Ticket.Add(ticket);               
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ProjectId = new SelectList(db.Project, "Id", "Name", ticket.ProjectId);
            return View(ticket);
        }

        // GET: Tickets/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ticket ticket = db.Ticket.Find(id);
            if (ticket == null)
            {
                return HttpNotFound();
            }
            ViewBag.ProjectId = new SelectList(db.Project, "Id", "Name", ticket.ProjectId);
            return View(ticket);
        }

        // POST: Tickets/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Title,AuthorId,AssignToUserId,Description,Created,Updated,ProjectId,TicketTypeId,TicketPriorityId,TicketStatusId")] Ticket ticket)
        {
            if (ModelState.IsValid)
            {
                db.Entry(ticket).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ProjectId = new SelectList(db.Project, "Id", "Name", ticket.ProjectId);
            return View(ticket);
        }

        // GET: Tickets/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ticket ticket = db.Ticket.Find(id);
            if (ticket == null)
            {
                return HttpNotFound();
            }
            return View(ticket);
        }

        // POST: Tickets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Ticket ticket = db.Ticket.Find(id);
            db.Ticket.Remove(ticket);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
