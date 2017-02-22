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
using System.Web.Security;

namespace ProjectCourse.Controllers
{
    public class InjuriesController : Controller
    {
        private aspnetEntities db = new aspnetEntities();

        // GET: Injuries
        public ActionResult Index()
        {
            var injuries = db.Injuries.Include(i => i.Bone).Include(i => i.EWPUser).Include(i => i.Joint).Include(i => i.Muscle);
            return View(injuries.ToList());
        }

        // GET: Injuries/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Injury injury = db.Injuries.Find(id);
            if (injury == null)
            {
                return HttpNotFound();
            }
            return View(injury);
        }

        // GET: Injuries/Create
        public ActionResult Create()
        {
            ViewBag.BoneID = new SelectList(db.Bones, "BoneID", "Name");
            ViewBag.UserID = new SelectList(db.EWPUsers, "UserID", "FirstName");
            ViewBag.JointID = new SelectList(db.Joints, "JointID", "Name");
            ViewBag.MuscleID = new SelectList(db.Muscles, "MuscleID", "Name");
            return View();
        }

        // POST: Injuries/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "InjuryID,UserID,MuscleID,JointID,InjuryDescription,BoneID")] Injury injury)
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
                injury.UserID = User.Identity.GetUserId();

                db.Injuries.Add(injury);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.BoneID = new SelectList(db.Bones, "BoneID", "Name", injury.BoneID);
            ViewBag.UserID = new SelectList(db.EWPUsers, "UserID", "FirstName", injury.UserID);
            ViewBag.JointID = new SelectList(db.Joints, "JointID", "Name", injury.JointID);
            ViewBag.MuscleID = new SelectList(db.Muscles, "MuscleID", "Name", injury.MuscleID);
            return View(injury);
        }

        // GET: Injuries/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Injury injury = db.Injuries.Find(id);
            if (injury == null)
            {
                return HttpNotFound();
            }
            ViewBag.BoneID = new SelectList(db.Bones, "BoneID", "Name", injury.BoneID);
            ViewBag.UserID = new SelectList(db.EWPUsers, "UserID", "FirstName", injury.UserID);
            ViewBag.JointID = new SelectList(db.Joints, "JointID", "Name", injury.JointID);
            ViewBag.MuscleID = new SelectList(db.Muscles, "MuscleID", "Name", injury.MuscleID);
            return View(injury);
        }

        // POST: Injuries/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "InjuryID,UserID,MuscleID,JointID,InjuryDescription,BoneID")] Injury injury)
        {
            if (ModelState.IsValid)
            {
                db.Entry(injury).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.BoneID = new SelectList(db.Bones, "BoneID", "Name", injury.BoneID);
            ViewBag.UserID = new SelectList(db.EWPUsers, "UserID", "FirstName", injury.UserID);
            ViewBag.JointID = new SelectList(db.Joints, "JointID", "Name", injury.JointID);
            ViewBag.MuscleID = new SelectList(db.Muscles, "MuscleID", "Name", injury.MuscleID);
            return View(injury);
        }

        // GET: Injuries/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Injury injury = db.Injuries.Find(id);
            if (injury == null)
            {
                return HttpNotFound();
            }
            return View(injury);
        }

        // POST: Injuries/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Injury injury = db.Injuries.Find(id);
            db.Injuries.Remove(injury);
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
