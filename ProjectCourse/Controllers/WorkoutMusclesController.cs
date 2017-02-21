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
    public class WorkoutMusclesController : Controller
    {
        private aspnetEntities db = new aspnetEntities();

        // GET: WorkoutMuscles
        public ActionResult Index()
        {
            var workoutMuscles = db.WorkoutMuscles.Include(w => w.Workout).Include(w => w.Muscle);
            return View(workoutMuscles.ToList());
        }

        // GET: WorkoutMuscles/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            WorkoutMuscle workoutMuscle = db.WorkoutMuscles.Find(id);
            if (workoutMuscle == null)
            {
                return HttpNotFound();
            }
            return View(workoutMuscle);
        }

        // GET: WorkoutMuscles/Create
        public ActionResult Create()
        {
            ViewBag.WorkoutID = new SelectList(db.Workouts, "WorkoutID", "Name");
            ViewBag.MuscleID = new SelectList(db.Muscles, "MuscleID", "Name");
            return View();
        }

        // POST: WorkoutMuscles/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "WorkoutMuscleID,WorkoutID,MuscleID,PrimMover,Synergist,Stabilizer,Lengthening")] WorkoutMuscle workoutMuscle)
        {
            if (ModelState.IsValid)
            {
                db.WorkoutMuscles.Add(workoutMuscle);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.WorkoutID = new SelectList(db.Workouts, "WorkoutID", "Name", workoutMuscle.WorkoutID);
            ViewBag.MuscleID = new SelectList(db.Muscles, "MuscleID", "Name", workoutMuscle.MuscleID);
            return View(workoutMuscle);
        }

        // GET: WorkoutMuscles/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            WorkoutMuscle workoutMuscle = db.WorkoutMuscles.Find(id);
            if (workoutMuscle == null)
            {
                return HttpNotFound();
            }
            ViewBag.WorkoutID = new SelectList(db.Workouts, "WorkoutID", "Name", workoutMuscle.WorkoutID);
            ViewBag.MuscleID = new SelectList(db.Muscles, "MuscleID", "Name", workoutMuscle.MuscleID);
            return View(workoutMuscle);
        }

        // POST: WorkoutMuscles/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "WorkoutMuscleID,WorkoutID,MuscleID,PrimMover,Synergist,Stabilizer,Lengthening")] WorkoutMuscle workoutMuscle)
        {
            if (ModelState.IsValid)
            {
                db.Entry(workoutMuscle).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.WorkoutID = new SelectList(db.Workouts, "WorkoutID", "Name", workoutMuscle.WorkoutID);
            ViewBag.MuscleID = new SelectList(db.Muscles, "MuscleID", "Name", workoutMuscle.MuscleID);
            return View(workoutMuscle);
        }

        // GET: WorkoutMuscles/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            WorkoutMuscle workoutMuscle = db.WorkoutMuscles.Find(id);
            if (workoutMuscle == null)
            {
                return HttpNotFound();
            }
            return View(workoutMuscle);
        }

        // POST: WorkoutMuscles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            WorkoutMuscle workoutMuscle = db.WorkoutMuscles.Find(id);
            db.WorkoutMuscles.Remove(workoutMuscle);
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
