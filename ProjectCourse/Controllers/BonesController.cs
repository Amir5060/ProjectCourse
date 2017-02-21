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
    [Authorize(Roles = "Admin")]
    public class BonesController : Controller
    {
        private aspnetEntities db = new aspnetEntities();

        // GET: Bones
        public ActionResult Index()
        {
            return View(db.Bones.ToList());
        }

        // GET: Bones/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Bone bone = db.Bones.Find(id);
            if (bone == null)
            {
                return HttpNotFound();
            }
            return View(bone);
        }

        // GET: Bones/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Bones/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "BoneID,Name")] Bone bone)
        {
            if (ModelState.IsValid)
            {
                db.Bones.Add(bone);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(bone);
        }

        // GET: Bones/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Bone bone = db.Bones.Find(id);
            if (bone == null)
            {
                return HttpNotFound();
            }
            return View(bone);
        }

        // POST: Bones/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "BoneID,Name")] Bone bone)
        {
            if (ModelState.IsValid)
            {
                db.Entry(bone).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(bone);
        }

        // GET: Bones/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Bone bone = db.Bones.Find(id);
            if (bone == null)
            {
                return HttpNotFound();
            }
            return View(bone);
        }

        // POST: Bones/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Bone bone = db.Bones.Find(id);
            db.Bones.Remove(bone);
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
