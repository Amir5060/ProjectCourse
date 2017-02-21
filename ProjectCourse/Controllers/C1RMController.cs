using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ProjectCourse.Models;
using Microsoft.AspNet.Identity;

namespace ProjectCourse.Controllers
{
    public class C1RMController : Controller
    {
        private aspnetEntities db = new aspnetEntities();

        // GET: C1RM
        public ActionResult Index()
        {
            var c1RM = db.C1RM.Include(c => c.EWPUser);
            return View(c1RM.ToList());
        }

        // GET: C1RM/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            C1RM c1RM = db.C1RM.Find(id);
            if (c1RM == null)
            {
                return HttpNotFound();
            }
            return View(c1RM);
        }

        // GET: C1RM/Create
        public ActionResult Create()
        {
            ViewBag.UserID = new SelectList(db.EWPUsers, "UserID", "FirstName");
            return View();
        }

        // POST: C1RM/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "RMID,UserID,UserWeight,RMDate")] C1RM c1RM)
        {
            if (ModelState.IsValid)
            {
                c1RM.UserID = User.Identity.GetUserId();
                db.C1RM.Add(c1RM);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.UserID = new SelectList(db.EWPUsers, "UserID", "FirstName", c1RM.UserID);
            return View(c1RM);
        }

        // GET: C1RM/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            C1RM c1RM = db.C1RM.Find(id);
            if (c1RM == null)
            {
                return HttpNotFound();
            }
            ViewBag.UserID = new SelectList(db.EWPUsers, "UserID", "FirstName", c1RM.UserID);
            return View(c1RM);
        }

        // POST: C1RM/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "RMID,UserID,UserWeight,RMDate")] C1RM c1RM)
        {
            if (ModelState.IsValid)
            {
                db.Entry(c1RM).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.UserID = new SelectList(db.EWPUsers, "UserID", "FirstName", c1RM.UserID);
            return View(c1RM);
        }

        // GET: C1RM/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            C1RM c1RM = db.C1RM.Find(id);
            if (c1RM == null)
            {
                return HttpNotFound();
            }
            return View(c1RM);
        }

        // POST: C1RM/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            C1RM c1RM = db.C1RM.Find(id);
            db.C1RM.Remove(c1RM);
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
