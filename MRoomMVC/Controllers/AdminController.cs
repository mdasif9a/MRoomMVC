﻿using System;
using System.Web.Mvc;
using System.Data.Entity;
using System.Collections.Generic;
using System.Linq;
using MRoomMVC.Data;
using MRoomMVC.Models;
using System.IO;
using System.Web;


namespace MRoomMVC.Controllers
{
    public class AdminController : Controller
    {
        private readonly MRoomDbContext db = new MRoomDbContext();

        [NonAction]
        private string SaveFile(HttpPostedFileBase file, string subfolder)
        {
            if (file != null && file.ContentLength > 0)
            {
                string imgurl = "/Content/" + subfolder + "/";
                string filename = DateTime.UtcNow.Ticks.ToString() + Path.GetExtension(file.FileName);
                string filePath = Server.MapPath(imgurl);
                if (!Directory.Exists(filePath))
                {
                    Directory.CreateDirectory(filePath);
                }
                string fullFilePath = Path.Combine(filePath, filename);
                file.SaveAs(fullFilePath);
                return imgurl + filename;
            }
            return null;
        }
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult AcSliders()
        {
            List<Slider> sliders = db.Sliders.AsNoTracking().ToList();
            return View(sliders);
        }

        public ActionResult AcSlidersCreate()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AcSlidersCreate(Slider slider, HttpPostedFileBase formFile)
        {
            if (ModelState.IsValid)
            {
                if (formFile.ContentLength > 0)
                {
                    slider.FilePath = SaveFile(formFile, "Sliders");
                }
                db.Sliders.Add(slider);
                db.SaveChanges();
                TempData["datachange"] = "Slider is Successfully Saved.";
                return RedirectToAction("AcSliders");
            }
            else
            {
                TempData["datachange"] = "Slider is Not Saved.";
            }
            return View(slider);
        }

        public ActionResult AcSlidersEdit(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }
            Slider slider = db.Sliders.Find(id);
            if (slider == null)
            {
                return Content("Nothing Found");
            }
            return View(slider);
        }

        [HttpPost]
        public ActionResult AcSlidersEdit(Slider slider, HttpPostedFileBase formFile)
        {
            if (ModelState.IsValid)
            {
                if (formFile != null && formFile.ContentLength > 0)
                {
                    slider.FilePath = SaveFile(formFile, "Sliders");
                }
                db.Entry(slider).State = EntityState.Modified;
                db.SaveChanges();
                TempData["datachange"] = "Slider is Successfully Updated.";
                return RedirectToAction("AcSliders");
            }
            else
            {
                TempData["datachange"] = "Slider is Not Updated.";
            }
            return View(slider);
        }

        public ActionResult AcSlidersDelete(int id)
        {
            Slider slider = db.Sliders.Find(id);
            if (slider != null)
            {
                db.Sliders.Remove(slider);
                db.SaveChanges();
                TempData["datachange"] = "Slider Delete.";
            }
            else
            {
                TempData["datachange"] = "Data Not Delete.";
            }
            return RedirectToAction("AcSliders");
        }

        public ActionResult AcTestimonials()
        {
            List<Testimonial> testimonials = db.Testimonials.AsNoTracking().ToList();
            return View(testimonials);
        }

        public ActionResult AcTestimonialsCreate()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AcTestimonialsCreate(Testimonial testimonial, HttpPostedFileBase formFile)
        {
            if (ModelState.IsValid)
            {
                if (formFile.ContentLength > 0)
                {
                    testimonial.ImgPath = SaveFile(formFile, "Testimonials");
                }
                db.Testimonials.Add(testimonial);
                db.SaveChanges();
                TempData["datachange"] = "Testimonial is Successfully Saved.";
                return RedirectToAction("AcTestimonials");
            }
            else
            {
                TempData["datachange"] = "Testimonial is Not Saved.";
            }
            return View(testimonial);
        }

        public ActionResult AcTestimonialsEdit(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }
            Testimonial testimonial = db.Testimonials.Find(id);
            if (testimonial == null)
            {
                return Content("Nothing Found");
            }
            return View(testimonial);
        }

        [HttpPost]
        public ActionResult AcTestimonialsEdit(Testimonial testimonial, HttpPostedFileBase formFile)
        {
            if (ModelState.IsValid)
            {
                if (formFile != null && formFile.ContentLength > 0)
                {
                    testimonial.ImgPath = SaveFile(formFile, "Testimonials");
                }
                db.Entry(testimonial).State = EntityState.Modified;
                db.SaveChanges();
                TempData["datachange"] = "Testimonial is Successfully Updated.";
                return RedirectToAction("AcTestimonials");
            }
            else
            {
                TempData["datachange"] = "Testimonial is Not Updated.";
            }
            return View(testimonial);
        }

        public ActionResult AcTestimonialsDelete(int id)
        {
            Testimonial testimonial = db.Testimonials.Find(id);
            if (testimonial != null)
            {
                db.Testimonials.Remove(testimonial);
                db.SaveChanges();
                TempData["datachange"] = "Testimonial Delete.";
            }
            else
            {
                TempData["datachange"] = "Data Not Delete.";
            }
            return RedirectToAction("AcTestimonials");
        }

        public ActionResult Settings()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Settings(string OldPassword, string NewPassword)
        {
            UserLogin login = db.UserLogins.Where(ur => ur.Role == "Admin").FirstOrDefault();
            if (login != null && login.Password == OldPassword)
            {
                login.Password = NewPassword;
                db.Entry(login).State = EntityState.Modified;
                db.SaveChanges();
                TempData["datachange"] = "Your Password has Sucessfully Change";
            }
            else if (String.IsNullOrEmpty(OldPassword))
            {
                TempData["datachange"] = "Please Enter Your Old Password";
            }
            else
            {
                TempData["datachange"] = "Incorrect Old Passsword";
            }
            return RedirectToAction("Settings");
        }
    }
}
