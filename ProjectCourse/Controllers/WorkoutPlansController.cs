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
    public class WorkoutPlansController : Controller
    {
        private aspnetEntities db = new aspnetEntities();

        // GET: WorkoutPlans
        public ActionResult Index()
        {
            var workoutPlans = db.WorkoutPlans.Include(w => w.Plan).Include(w => w.Workout);
            if (workoutPlans.ToList().Count() == 0)
            {
                var currentUserID = User.Identity.GetUserId();//We need to get the last C1RM here=======================================
                if (db.C1RM.Where(c => c.UserID == currentUserID).Count() > 0)//To see if this user has any current 1RM running available.
                {
                    var rmID = db.C1RM.SingleOrDefault(c => c.UserID == currentUserID).RMID;//============ I have to get the last RMID
                    if (db.C1RMWorkout.Where(c => c.RMID == rmID).Count() > 0)//To see if current user has any workouts to try for 1RM
                    {
                        if (db.C1RMWorkout.Where(c => c.RMID == rmID && c.Workout1RM == null).Count() > 0)//To see if user fill all the data for all the workouts
                        {
                            return RedirectToAction("../C1RM");
                        }
                        else
                        {
                            //if (!(workoutPlans.Count() > 0))
                            //{
                            //    var rmWorkouts = db.C1RMWorkout.Where(c => c.RMID == rmID).ToList();
                            //    foreach(var v in rmWorkouts)
                            //    {
                            //        var rate = Utilities.RelativeCalculator(db.C1RM.SingleOrDefault(c => c.UserID == currentUserID).UserWeight, Convert.ToSingle(v.Workout1RM));
                            //        if (db.RelativeStrengthTs.SingleOrDefault(r => r.WorkoutID == v.WorkoutID && r.RelativeStrengthValue == rate).RelativeStrengthPoint < 5)
                            //        {
                            //            WorkoutPlan wp = new WorkoutPlan();
                            //            wp.WorkoutID = v.WorkoutID;
                            //            wp.PlanID = (db.Plans.Where(p => p.UserID == currentUserID).OrderByDescending(p => p.PlanID)).ToList()[0].PlanID;

                            //        }
                            //        //v.WorkoutID
                            //    }
                            //}
                            return View(workoutPlans.ToList());
                        }
                    }
                }
            }
            return View(workoutPlans.ToList());
        }

        // GET: WorkoutPlans/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            WorkoutPlan workoutPlan = db.WorkoutPlans.Find(id);
            if (workoutPlan == null)
            {
                return HttpNotFound();
            }
            return View(workoutPlan);
        }

        // GET: WorkoutPlans/Create
        public ActionResult Create()
        {
            ViewBag.PlanID = new SelectList(db.Plans, "PlanID", "UserID");
            ViewBag.WorkoutID = new SelectList(db.Workouts, "WorkoutID", "Name");
            return View();
        }

        // POST: WorkoutPlans/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "WorkoutPlanID,WorkoutID,PlanID,Repetition,WorkoutPlanSet,Rest,WorkoutPlanWeight")] WorkoutPlan workoutPlan)
        {
            if (ModelState.IsValid)
            {
                db.WorkoutPlans.Add(workoutPlan);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.PlanID = new SelectList(db.Plans, "PlanID", "UserID", workoutPlan.PlanID);
            ViewBag.WorkoutID = new SelectList(db.Workouts, "WorkoutID", "Name", workoutPlan.WorkoutID);
            return View(workoutPlan);
        }

        // GET: WorkoutPlans/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            WorkoutPlan workoutPlan = db.WorkoutPlans.Find(id);
            if (workoutPlan == null)
            {
                return HttpNotFound();
            }
            ViewBag.PlanID = new SelectList(db.Plans, "PlanID", "UserID", workoutPlan.PlanID);
            ViewBag.WorkoutID = new SelectList(db.Workouts, "WorkoutID", "Name", workoutPlan.WorkoutID);
            return View(workoutPlan);
        }

        // POST: WorkoutPlans/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "WorkoutPlanID,WorkoutID,PlanID,Repetition,WorkoutPlanSet,Rest,WorkoutPlanWeight")] WorkoutPlan workoutPlan)
        {
            if (ModelState.IsValid)
            {
                db.Entry(workoutPlan).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.PlanID = new SelectList(db.Plans, "PlanID", "UserID", workoutPlan.PlanID);
            ViewBag.WorkoutID = new SelectList(db.Workouts, "WorkoutID", "Name", workoutPlan.WorkoutID);
            return View(workoutPlan);
        }

        // GET: WorkoutPlans/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            WorkoutPlan workoutPlan = db.WorkoutPlans.Find(id);
            if (workoutPlan == null)
            {
                return HttpNotFound();
            }
            return View(workoutPlan);
        }

        // POST: WorkoutPlans/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            WorkoutPlan workoutPlan = db.WorkoutPlans.Find(id);
            db.WorkoutPlans.Remove(workoutPlan);
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
