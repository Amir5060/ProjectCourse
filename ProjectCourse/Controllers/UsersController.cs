using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ProjectCourse.Models;

namespace EWP.Controllers
{
    public class UsersController : Controller
    {
        private aspnetEntities db = new aspnetEntities();

        // GET: Users
        [CheckAuthorize(Roles = "Admin")]
        public ActionResult Index()
        {
            var users = db.EWPUsers.Include(u => u.Sport);
            return View(users.ToList());
        }

        // GET: Users/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EWPUser user = db.EWPUsers.Find(id);
            if (user == null)
            {
                return RedirectToAction("Create");
                //return HttpNotFound();
            }
            return View(user);
        }

        // GET: Users/Create
        public ActionResult Create()
        {
            ViewBag.SportID = new SelectList(db.Sports, "SportID", "SportName");
            return View();
        }

        // POST: Users/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "UserID,FirstName,LastName,Gender,DateOfBirth,Email,Username,Height,Experience,SportID,PhoneNumber,Address,EmailConfirmation")] EWPUser user)
        {
            if (ModelState.IsValid)
            {
                user.UserID = Guid.NewGuid().ToString();
                db.EWPUsers.Add(user);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.SportID = new SelectList(db.Sports, "SportID", "SportName", user.SportID);
            return View(user);
        }

        // GET: Users/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EWPUser user = db.EWPUsers.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            ViewBag.SportID = new SelectList(db.Sports, "SportID", "SportName", user.SportID);
            return View(user);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "UserID,FirstName,LastName,Gender,DateOfBirth,Email,Username,Height,Experience,SportID,PhoneNumber,Address,EmailConfirmation")] EWPUser user)
        {
            if (ModelState.IsValid)
            {
                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.SportID = new SelectList(db.Sports, "SportID", "SportName", user.SportID);
            return View(user);
        }

        [CheckAuthorize(Roles = "Admin")]
        // GET: Users/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EWPUser user = db.EWPUsers.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: Users/Delete/5
        [CheckAuthorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            EWPUser user = db.EWPUsers.Find(id);
            db.EWPUsers.Remove(user);
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
