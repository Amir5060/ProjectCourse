using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ProjectCourse.Models;

namespace ProjectCourse.Controllers
{
    public class RelativeStrengthTsController : Controller
    {
        private aspnetEntities db = new aspnetEntities();

        // GET: RelativeStrengthTs
        public ActionResult Index()
        {
            var relativeStrengthTs = db.RelativeStrengthTs.Include(r => r.Workout);
            return View(relativeStrengthTs.ToList());
        }

        // GET: RelativeStrengthTs/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RelativeStrengthT relativeStrengthT = db.RelativeStrengthTs.Find(id);
            if (relativeStrengthT == null)
            {
                return HttpNotFound();
            }
            return View(relativeStrengthT);
        }

        // GET: RelativeStrengthTs/Create
        public ActionResult Create()
        {
            ViewBag.WorkoutID = new SelectList(db.Workouts, "WorkoutID", "Name");
            return View();
        }

        // POST: RelativeStrengthTs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "RelativeStrengthID,WorkoutID,RelativeStrengthValue,RelativeStrengthPoint,Sex")] RelativeStrengthT relativeStrengthT)
        {
            if (ModelState.IsValid)
            {
                db.RelativeStrengthTs.Add(relativeStrengthT);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.WorkoutID = new SelectList(db.Workouts, "WorkoutID", "Name", relativeStrengthT.WorkoutID);
            return View(relativeStrengthT);
        }

        // GET: RelativeStrengthTs/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RelativeStrengthT relativeStrengthT = db.RelativeStrengthTs.Find(id);
            if (relativeStrengthT == null)
            {
                return HttpNotFound();
            }
            ViewBag.WorkoutID = new SelectList(db.Workouts, "WorkoutID", "Name", relativeStrengthT.WorkoutID);
            return View(relativeStrengthT);
        }

        // POST: RelativeStrengthTs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "RelativeStrengthID,WorkoutID,RelativeStrengthValue,RelativeStrengthPoint,Sex")] RelativeStrengthT relativeStrengthT)
        {
            if (ModelState.IsValid)
            {
                db.Entry(relativeStrengthT).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.WorkoutID = new SelectList(db.Workouts, "WorkoutID", "Name", relativeStrengthT.WorkoutID);
            return View(relativeStrengthT);
        }

        // GET: RelativeStrengthTs/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RelativeStrengthT relativeStrengthT = db.RelativeStrengthTs.Find(id);
            if (relativeStrengthT == null)
            {
                return HttpNotFound();
            }
            return View(relativeStrengthT);
        }

        // POST: RelativeStrengthTs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            RelativeStrengthT relativeStrengthT = db.RelativeStrengthTs.Find(id);
            db.RelativeStrengthTs.Remove(relativeStrengthT);
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
