using System;
using System.Web.Mvc;
using System.Data.Entity;
using System.Collections.Generic;
using System.Linq;
using MRoomMVC.Data;
using MRoomMVC.Models;

namespace MRoomMVC.Controllers
{
    public class MyUserController : Controller
    {
        private readonly MRoomDbContext db = new MRoomDbContext();
        


        // Users Login Crud Operations
        public ActionResult Index()
        {
            List<UserLogin> users = db.UserLogins.ToList();
            return View(users);
        }


        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(UserLogin myuser)
        {
            if (ModelState.IsValid)
            {
                db.UserLogins.Add(myuser);
                db.SaveChanges();
                TempData["datachange"] = "User is Successfully Saved.";
                return RedirectToAction("Index");
            }
            else
            {
                TempData["datachange"] = "User is Not Saved.";
            }
            return View(myuser);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }
            UserLogin myuser = db.UserLogins.Find(id);
            if (myuser == null)
            {
                return Content("Nothing Found");
            }
            return View(myuser);
        }

        [HttpPost]
        public ActionResult Edit(UserLogin myuser)
        {
            if (ModelState.IsValid)
            {
                db.Entry(myuser).State = EntityState.Modified;
                db.SaveChanges();
                TempData["datachange"] = "User is Successfully Updated.";
                return RedirectToAction("Index");
            }
            else
            {
                TempData["datachange"] = "User is Not Updated.";
            }
            return View(myuser);
        }

        public ActionResult Delete(int id)
        {
            UserLogin type = db.UserLogins.Find(id);
            if (type != null)
            {
                db.UserLogins.Remove(type);
                db.SaveChanges();
                TempData["datachange"] = "UserLogin Delete.";
            }
            else
            {
                TempData["datachange"] = "Data Not Delete.";
            }
            return RedirectToAction("Index");
        }
    }
}
