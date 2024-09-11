using System;
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
    [Authorize(Roles = "Admin, Rental, LandLords")]
    public class LandLordsController : Controller
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
            var usercurrent = db.UserLogins.Where(x => x.Username == User.Identity.Name).FirstOrDefault();
            int propertiesct = db.PropertyDetails.Where(x => x.UserId == usercurrent.Id).AsNoTracking().Count();
            ViewBag.ProCount = propertiesct;
            return View();
        }

        public ActionResult PropertyList()
        {
            var usercurrent = db.UserLogins.Where(x => x.Username == User.Identity.Name).FirstOrDefault();
            List<PropertyDetail> properties = db.PropertyDetails.Where(x => x.UserId == usercurrent.Id).AsNoTracking().ToList();
            return View(properties);
        }


        public ActionResult PropertyCreate()
        {
            PropertyDetail property = new PropertyDetail();
            var usercurrent = db.UserLogins.Where(x => x.Username == User.Identity.Name).FirstOrDefault();
            ViewBag.LMyUsers = new SelectList(db.UserLogins.AsNoTracking().Where(x => x.Role != "Admin" && x.Username == User.Identity.Name).ToList(), "Id", "Username");
            ViewBag.LPropertyType = new SelectList(db.PropertyTypes.AsNoTracking().ToList(), "Id", "PropertyTypeName");
            ViewBag.LBHK = new SelectList(db.BHKTypes.Where(x => x.Status == "Active").AsNoTracking().ToList(), "BHKName", "BHKName");
            ViewBag.LToiletType = new SelectList(db.ToiletTypes.Where(x => x.Status == "Active").AsNoTracking().ToList(), "Name", "Name");
            ViewBag.LParkingType = new SelectList(db.ParkingTypes.Where(x => x.Status == "Active").AsNoTracking().ToList(), "Name", "Name");
            ViewBag.LParkingVisitors = new SelectList(db.ParkingVisitors.Where(x => x.Status == "Active").AsNoTracking().ToList(), "Name", "Name");
            ViewBag.LFloor = new SelectList(db.FloorTypes.Where(x => x.Status == "Active").AsNoTracking().ToList(), "FloorTypeName", "FloorTypeName");
            ViewBag.LFirstPriority = new SelectList(db.FirstPriorities.Where(x => x.Status == "Active").AsNoTracking().ToList(), "Name", "Name");
            ViewBag.LCountry = new SelectList(db.CountryMasters.Where(x => x.Status == "Active").AsNoTracking().ToList(), "Name", "Name");
            ViewBag.LReligion = new SelectList(db.Religions.Where(x => x.Status == "Active").AsNoTracking().ToList(), "Name", "Name");
            ViewBag.LFurnished = new SelectList(db.FurnishedTypes.Where(x => x.Status == "Active").AsNoTracking().ToList(), "Name", "Name");
            ViewBag.LWater = new SelectList(db.WaterSupplies.Where(x => x.Status == "Active").AsNoTracking().ToList(), "Name", "Name");
            ViewBag.LLpg = new SelectList(db.Lpgs.Where(x => x.Status == "Active").AsNoTracking().ToList(), "Name", "Name");
            ViewBag.LElectricity = new SelectList(db.Electricities.Where(x => x.Status == "Active").AsNoTracking().ToList(), "Name", "Name");
            ViewBag.LStair = new SelectList(db.Stairs.Where(x => x.Status == "Active").AsNoTracking().ToList(), "Name", "Name");
            ViewBag.LRoof = new SelectList(db.Roofs.Where(x => x.Status == "Active").AsNoTracking().ToList(), "Name", "Name");
            ViewBag.LCooking = new SelectList(db.CookingItems.Where(x => x.Status == "Active").AsNoTracking().ToList(), "Name", "Name");
            ViewBag.LNearBies = new SelectList(db.NearBies.Where(x => x.Status == "Active").AsNoTracking().ToList(), "Id", "NearByName");

            property.PropertyFor = "Rent";
            property.UserId = usercurrent.Id;

            return View(property);
        }

        [HttpPost]
        public ActionResult PropertyCreate(PropertyDetail property, HttpPostedFileBase imageInput1, HttpPostedFileBase imageInput2, HttpPostedFileBase imageInput3, HttpPostedFileBase imageInput4, HttpPostedFileBase imageInput5, HttpPostedFileBase imageInput6)
        {
            List<PD_Near> pD_Nears = new List<PD_Near>();
            if (ModelState.IsValid)
            {
                property.Image1 = SaveFile(imageInput1, "PropertyImages");
                property.Image2 = SaveFile(imageInput2, "PropertyImages");
                property.Image3 = SaveFile(imageInput3, "PropertyImages");
                property.Image4 = SaveFile(imageInput4, "PropertyImages");
                property.Image5 = SaveFile(imageInput5, "PropertyImages");
                property.Image6 = SaveFile(imageInput6, "PropertyImages");
                property.CreatedDate = DateTime.Today;
                db.PropertyDetails.Add(property);
                db.SaveChanges();
                property.PropertyId = "MROOM" + property.BHKTypeName.Replace(" ", "") + property.Id.ToString("0000");
                db.SaveChanges();
                pD_Nears.AddRange(from item1 in property.NearBies
                                  let d_Near = new PD_Near { NearById = item1, PropertyId = property.Id }
                                  select d_Near);
                db.PD_Near.AddRange(pD_Nears);
                db.SaveChanges();
                TempData["datachange"] = "Property is Successfully Saved.";
                return RedirectToAction("PropertyList");
            }
            else
            {
                TempData["datachange"] = "Property Type is Not Saved.";
            }
            return View(property);
        }


        public ActionResult Settings()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Settings(string OldPassword, string NewPassword)
        {
            var login = db.UserLogins.Where(x => x.Username == User.Identity.Name).FirstOrDefault();
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