using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ProjectCourse.Models;
using System.Web.Security;
using System.Net;

namespace EWP.Controllers
{
    public class UserInfoController : Controller
    {
        aspnetEntities db = new aspnetEntities();
        public UserInfoController()
        {
            ViewBag.SportID = new SelectList(db.Sports, "SportID", "SportName");
            ViewBag.GenderList = GenderList();
        }

        // GET: UserInfo
        public ActionResult Index(string userID)
        {
            var userInfo = db.GetUserByUserID((string)Membership.GetUser(User.Identity.Name).ProviderUserKey);
            if (userInfo != null)
            {
                return View(userInfo);
            }
            return Create();
        }

        // GET: UserInfo/Details/5
        public ActionResult Details(int id)
        {
            var userInfo = db.GetUserByUserID((string)Membership.GetUser(User.Identity.Name).ProviderUserKey);
            if (userInfo != null)
            {
                return View(userInfo);
            }
            return Create();
        }

        // GET: UserInfo/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: UserInfo/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: UserInfo/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EWPUser user = db.EWPUsers.Find(id);
            //if (user == null)
            //{
            //    return HttpNotFound();
            //}
            if (user != null)
            {
                GetUserByUserID_Result currentUser = db.GetUserByUserID(id.ToString()).ToList()[0];
                ViewBag.SportID = new SelectList(db.Sports, "SportID", "SportName", currentUser.SportID);//new SelectList(db.Sports, "SportID", "SportName", userInfo.SportID);
                ViewBag.GenderID = new SelectList(GenderList(), "GenderName", currentUser.Gender);
                return View(currentUser);
            }
            else
            {
                ViewBag.SportID = new SelectList(db.Sports, "SportID", "SportName");
                ViewBag.GenderList = GenderList();
                
            }

            return View(user);
        }

        // POST: UserInfo/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(string id, FormCollection collection)
        {
            try
            {
               
                // TODO: Add update logic here
                EWPUser newUser = db.EWPUsers.Find(id);
                if (newUser == null)
                {
                    newUser = new EWPUser();
                    newUser.UserID = id.ToString();
                    newUser.FirstName = collection["FirstName"];
                    newUser.LastName = collection["LastName"];
                    newUser.Gender = collection["Gender"];
                    if (collection["DateOfBirth"] != null)
                        if (collection["DateOfBirth"].ToString().Trim() != "")
                            newUser.DateOfBirth = Convert.ToDateTime(collection["DateOfBirth"]);
                    if (collection["Username"].ToString().Trim().Length > 0)
                        newUser.Username = collection["Username"];
                    if (collection["Height"].ToString().Trim().Length > 0)
                        newUser.Height = Convert.ToInt32(collection["Height"]);
                    if (collection["Experience"].ToString().Trim().Length > 0)
                        newUser.Experience = Convert.ToInt32(collection["Experience"]);
                    newUser.SportID = Convert.ToInt32(collection["SportID"]);
                    newUser.PhoneNumber = collection["PhoneNumber"];
                    newUser.Address = collection["Address"];

                    db.EWPUsers.Add(newUser);
                    db.SaveChanges();
                    ViewBag.Color = "Green";
                    ViewBag.Message = "Your info added into the database.";
                }
                else
                {
                    
                    //UpdateModel(currentUser, "currentUser");
                    //newUser = new EWPUser();
                    newUser.UserID = id.ToString();
                    newUser.FirstName = collection["FirstName"];
                    newUser.LastName = collection["LastName"];
                    newUser.Gender = collection["Gender"];
                    if (collection["DateOfBirth"] != null)
                        if (collection["DateOfBirth"].ToString().Trim() != "")
                            newUser.DateOfBirth = Convert.ToDateTime(collection["DateOfBirth"]);
                    if (collection["Username"].ToString().Trim().Length > 0)
                        newUser.Username = collection["Username"];
                    if (collection["Height"].ToString().Trim().Length > 0)
                        newUser.Height = Convert.ToInt32(collection["Height"]);
                    if (collection["Experience"].ToString().Trim().Length > 0)
                        newUser.Experience = Convert.ToInt32(collection["Experience"]);
                    newUser.SportID = Convert.ToInt32(collection["SportID"]);
                    newUser.PhoneNumber = collection["PhoneNumber"];
                    newUser.Address = collection["Address"];

                    //db.EWPUsers.(newUser);
                    db.SaveChanges();
                    ViewBag.Color = "Green";
                    ViewBag.Message = "Your info has been edited successfully.";
                    return View(db.GetUserByUserID(id.ToString()).ToList()[0]);
                }
                //ViewBag.SportID = new SelectList(db.Sports, "SportID", "SportName");
                //ViewBag.GenderList = GenderList();
                //myUser.save();
                //return RedirectToAction("Index");
                return View();
            }
            catch(Exception ex)
            {
                ViewBag.Color = "Red";
                ViewBag.Message = "There was an error.";
                return View();
            }
        }

        // GET: UserInfo/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: UserInfo/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }


        /// <summary>
        /// Description:
        ///     Populating the gender dropdown
        /// History:
        ///     Amir Naji   19-10-2016
        /// </summary>
        /// <returns></returns>
        public List<SelectListItem> GenderList()
        {
            List<SelectListItem> items = new List<SelectListItem>();
            items.Add(new SelectListItem() { Value = "Male", Text = "Male" });
            items.Add(new SelectListItem() { Value = "Female", Text = "Female" });
            return items;
        }

        public SelectList GetAllSports()
        {
            return new SelectList(db.GetAllSports(), "SportID", "SportName");
        }
    }
}
