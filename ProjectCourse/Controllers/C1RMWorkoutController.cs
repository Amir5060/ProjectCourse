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
    [Authorize]
    public class C1RMWorkoutController : Controller
    {
        private aspnetEntities db = new aspnetEntities();

        // GET: C1RMWorkout
        public ActionResult Index()
        {
            var userId = User.Identity.GetUserId();
            var c1RMWorkout = db.C1RMWorkout.Include(c => c.C1RM).Where(c => c.Plan.UserID == userId && c.Plan.FinishDate == null).Include(c => c.Workout);
            return View(c1RMWorkout.ToList());
        }

        // GET: C1RMWorkout/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            C1RMWorkout c1RMWorkout = db.C1RMWorkout.Find(id);
            if (c1RMWorkout == null)
            {
                return HttpNotFound();
            }
            return View(c1RMWorkout);
        }

        // GET: C1RMWorkout/Create
        public ActionResult Create()
        {
            var currentUserID = User.Identity.GetUserId();
            if (db.C1RM.Where(c => c.UserID == currentUserID).Count() > 0)
            {
                var rmID = db.C1RM.SingleOrDefault(c => c.UserID == currentUserID).RMID;
                if (db.C1RMWorkout.Where(c => c.RMID == rmID).Count() > 0)
                {
                    var retVal = db.C1RMWorkout.Where(c => c.RMID == rmID);
                    return View(retVal);
                    //ViewBag.RMID = new SelectList(db.C1RM, "RMID", "UserID");
                    //ViewBag.WorkoutID = new SelectList(db.Workouts, "WorkoutID", "Name");
                    //return View();
                }
                else
                {
                    //Just for the first time, and for the test how the users physical status is.
                    int[] workoutID = new int[] { 1, 5, 6, 7, 8, 9 };//List of all workout's IDs for the first period.
                    C1RMWorkout c1RMWorkout;
                    var userPlan = db.Plans.FirstOrDefault(x => x.UserID == currentUserID);
                    for (int i = 0; i < 6; i++)
                    {
                        c1RMWorkout = new Models.C1RMWorkout();
                        c1RMWorkout.RMID = rmID;
                        c1RMWorkout.RMWorkoutDate = DateTime.Now.Date;
                        c1RMWorkout.WorkoutID = workoutID[i];
                        c1RMWorkout.RMPlanId = userPlan.PlanID;
                        db.C1RMWorkout.Add(c1RMWorkout);
                        db.SaveChanges();
                    }
                    return RedirectToAction("Index");
                }
            }
            return View();
        }

        // POST: C1RMWorkout/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "RMWorkoutID,RMID,WorkoutID,WorkoutWeight,Repetition,RMWorkoutDate,C1RM")] C1RMWorkout c1RMWorkout)
        {
            if (ModelState.IsValid)
            {
                db.C1RMWorkout.Add(c1RMWorkout);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.RMID = new SelectList(db.C1RM, "RMID", "UserID", c1RMWorkout.RMID);
            ViewBag.WorkoutID = new SelectList(db.Workouts, "WorkoutID", "Name", c1RMWorkout.WorkoutID);
            return View(c1RMWorkout);
        }

        // GET: C1RMWorkout/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            C1RMWorkout c1RMWorkout = db.C1RMWorkout.Find(id);
            if (c1RMWorkout == null)
            {
                return HttpNotFound();
            }
            ViewBag.RMID = new SelectList(db.C1RM, "RMID", "UserID", c1RMWorkout.RMID);
            ViewBag.WorkoutID = new SelectList(db.Workouts, "WorkoutID", "Name", c1RMWorkout.WorkoutID);
            return View(c1RMWorkout);
        }

        // POST: C1RMWorkout/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "RMWorkoutID,RMID,WorkoutID,WorkoutWeight,Repetition,RMWorkoutDate,C1RM,RMPlanID")] C1RMWorkout c1RMWorkout)
        {
            if (c1RMWorkout.WorkoutWeight == null && c1RMWorkout.Repetition == null)
            {
                c1RMWorkout = db.C1RMWorkout.Find(Convert.ToInt32(RouteData.Values["id"]));
                ViewBag.Message = "You gotta fill in all the fields!";
                return View(c1RMWorkout);
            }
            if (ModelState.IsValid)
            {
                db.Entry(c1RMWorkout).State = EntityState.Modified;
                if ((Convert.ToSingle(Request["WorkoutWeight"]) > 0) && (Convert.ToInt32(Request["Repetition"]) > 0))
                    c1RMWorkout.Workout1RM = Utilities.OneRMCalculator(Convert.ToSingle(Request["WorkoutWeight"]), Convert.ToInt32(Request["Repetition"]));
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.RMID = new SelectList(db.C1RM, "RMID", "UserID", c1RMWorkout.RMID);
            ViewBag.WorkoutID = new SelectList(db.Workouts, "WorkoutID", "Name", c1RMWorkout.WorkoutID);
            return View(c1RMWorkout);
        }

        // GET: C1RMWorkout/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            C1RMWorkout c1RMWorkout = db.C1RMWorkout.Find(id);
            if (c1RMWorkout == null)
            {
                return HttpNotFound();
            }
            return View(c1RMWorkout);
        }

        // POST: C1RMWorkout/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            C1RMWorkout c1RMWorkout = db.C1RMWorkout.Find(id);
            db.C1RMWorkout.Remove(c1RMWorkout);
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

        /// <summary>
        /// Description:
        ///     Send the user his workouts for this plan.
        /// History:
        ///     Amir Naji   02/12/2016
        /// </summary>
        /// <returns></returns>
        public ActionResult WorkoutPlan()
        {
            var currentUserID = User.Identity.GetUserId();
            if (db.C1RM.Where(c => c.UserID == currentUserID).Count() > 0)//To see if this user has any current 1RM running available.
            {
                var rmID = db.C1RM.SingleOrDefault(c => c.UserID == currentUserID).RMID;
                if (db.C1RMWorkout.Where(c => c.RMID == rmID).Count() > 0)//To see if current user has any workouts to try for 1RM
                {
                    if (db.C1RMWorkout.Where(c => c.RMID == rmID && c.Workout1RM == null).Count() > 0)//To see if user fill all the data for all the workouts
                    {
                        //ViewBag.Color = "Red";
                        //ViewBag.Message = "You haven't finished all of your Workouts yet.";
                        return RedirectToAction("../C1RMWorkout");
                    }                    
                    else
                        return RedirectToAction("../WorkoutPlans");
                }
            }
            return RedirectToAction("Index");
        }
    }
}
