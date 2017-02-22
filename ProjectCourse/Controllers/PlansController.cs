using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ProjectCourse.Models;
using System.Web.Security;
using Microsoft.AspNet.Identity;

namespace ProjectCourse.Controllers
{
    public class PlansController : Controller
    {
        private aspnetEntities db = new aspnetEntities();

        // GET: Plans
        public ActionResult Index()
        {
            var currentUserID = User.Identity.GetUserId(); 
            var plans = db.Plans.Where(p => p.UserID == currentUserID); 
            return View(plans.ToList());
        }

        // GET: Plans/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Plan plan = db.Plans.Find(id);
            if (plan == null)
            {
                return HttpNotFound();
            }
            return View(plan);
        }

        // GET: Plans/Create
        public ActionResult Create()
        {
            ViewBag.UserID = new SelectList(db.EWPUsers, "UserID", "FirstName");
            return View();
        }

        // POST: Plans/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "PlanID,UserID,Microcycle,WorkoutTime,PlanDate")] Plan plan)
        {
            if (ModelState.IsValid)
            {
                //If user still hasn't filled up his personal info
                string myUserID = User.Identity.GetUserId();
                if (db.EWPUsers.Where(p => p.UserID == myUserID).Count() == 0)
                {
                    ViewBag.Color = "Red";
                    ViewBag.Message = "Please fill the User Info page first.";
                    return View();
                }
                plan.UserID = User.Identity.GetUserId();
                if (plan.PlanDate == null)
                    plan.PlanDate = DateTime.Now;
                db.Plans.Add(plan);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.UserID = new SelectList(db.EWPUsers, "UserID", "FirstName", plan.UserID);
            return View(plan);
        }

        // GET: Plans/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Plan plan = db.Plans.Find(id);
            if (plan == null)
            {
                return HttpNotFound();
            }
            ViewBag.UserID = new SelectList(db.EWPUsers, "UserID", "FirstName", plan.UserID);
            return View(plan);
        }

        // POST: Plans/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "PlanID,UserID,Microcycle,WorkoutTime,PlanDate")] Plan plan)
        {
            if (ModelState.IsValid)
            {
                plan.UserID = User.Identity.GetUserId();
                db.Entry(plan).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.UserID = new SelectList(db.EWPUsers, "UserID", "FirstName", plan.UserID);
            return View(plan);
        }

        // GET: Plans/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Plan plan = db.Plans.Find(id);
            if (plan == null)
            {
                return HttpNotFound();
            }
            return View(plan);
        }

        // POST: Plans/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Plan plan = db.Plans.Find(id);
            db.Plans.Remove(plan);
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
