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
    [CheckAuthorize(Roles = "Admin")]
    public class JointsController : Controller
    {
        private aspnetEntities db = new aspnetEntities();

        // GET: Joints
        public ActionResult Index()
        {
            return View(db.Joints.ToList());
        }

        // GET: Joints/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Joint joint = db.Joints.Find(id);
            if (joint == null)
            {
                return HttpNotFound();
            }
            return View(joint);
        }

        // GET: Joints/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Joints/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "JointID,Name")] Joint joint)
        {
            if (ModelState.IsValid)
            {
                db.Joints.Add(joint);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(joint);
        }

        // GET: Joints/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Joint joint = db.Joints.Find(id);
            if (joint == null)
            {
                return HttpNotFound();
            }
            return View(joint);
        }

        // POST: Joints/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "JointID,Name")] Joint joint)
        {
            if (ModelState.IsValid)
            {
                db.Entry(joint).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(joint);
        }

        // GET: Joints/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Joint joint = db.Joints.Find(id);
            if (joint == null)
            {
                return HttpNotFound();
            }
            return View(joint);
        }

        // POST: Joints/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Joint joint = db.Joints.Find(id);
            db.Joints.Remove(joint);
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
