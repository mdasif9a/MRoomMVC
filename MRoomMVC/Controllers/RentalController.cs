using System;
using System.Web.Mvc;
using System.Data.Entity;
using System.Collections.Generic;
using System.Linq;
using MRoomMVC.Data;
using MRoomMVC.Models;
using System.IO;
using System.Web;
using System.Security.Cryptography;

namespace MRoomMVC.Controllers
{
    [Authorize(Roles = "Admin, Rental, LandLords")]
    public class RentalController : Controller
    {
        private readonly MRoomDbContext db = new MRoomDbContext();
        public ActionResult Index()
        {
            var usercurrent = db.UserLogins.Where(x => x.Username == User.Identity.Name).FirstOrDefault();
            int propertiesct = db.PropertyDetails.Where(x => x.UserId == usercurrent.Id).AsNoTracking().Count();
            ViewBag.ProCount = propertiesct;
            return View();
        }

        public ActionResult SearchProperty()
        {
            ViewBag.LMyUsers = new SelectList(db.UserLogins.AsNoTracking().Where(x => x.Role != "Admin").ToList(), "Id", "Username");
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
            return View();
        }

        public ActionResult PropertySearch(PropertyDetail property)
        {
            IQueryable<PropertyDetail> propertyDetails = db.PropertyDetails.Where(x => x.IsActive);

            // Apply filters based on the optional fields
            if (property.PropertyTypeId.HasValue)
            {
                propertyDetails = propertyDetails.Where(x => x.PropertyTypeId == property.PropertyTypeId);
            }

            if (!string.IsNullOrEmpty(property.Size))
            {
                propertyDetails = propertyDetails.Where(x => x.Size == property.Size);
            }

            if (!string.IsNullOrEmpty(property.PropertyVariantName))
            {
                propertyDetails = propertyDetails.Where(x => x.PropertyVariantName == property.PropertyVariantName);
            }

            if (!string.IsNullOrEmpty(property.BHKTypeName))
            {
                propertyDetails = propertyDetails.Where(x => x.BHKTypeName == property.BHKTypeName);
            }

            if (!string.IsNullOrEmpty(property.ToiletTypeName))
            {
                propertyDetails = propertyDetails.Where(x => x.ToiletTypeName == property.ToiletTypeName);
            }

            if (!string.IsNullOrEmpty(property.ParkingTypeName))
            {
                propertyDetails = propertyDetails.Where(x => x.ParkingTypeName == property.ParkingTypeName);
            }

            if (!string.IsNullOrEmpty(property.ParkingVisitorName))
            {
                propertyDetails = propertyDetails.Where(x => x.ParkingVisitorName == property.ParkingVisitorName);
            }

            if (!string.IsNullOrEmpty(property.FloorTypeName))
            {
                propertyDetails = propertyDetails.Where(x => x.FloorTypeName == property.FloorTypeName);
            }

            if (!string.IsNullOrEmpty(property.FirstPrioName))
            {
                propertyDetails = propertyDetails.Where(x => x.FirstPrioName == property.FirstPrioName);
            }

            if (property.NoOfMembers != null)
            {
                propertyDetails = propertyDetails.Where(x => x.NoOfMembers == property.NoOfMembers);
            }

            if (!string.IsNullOrEmpty(property.CC_TV))
            {
                propertyDetails = propertyDetails.Where(x => x.CC_TV == property.CC_TV);
            }

            if (!string.IsNullOrEmpty(property.CountryName))
            {
                propertyDetails = propertyDetails.Where(x => x.CountryName == property.CountryName);
            }

            if (!string.IsNullOrEmpty(property.StateName))
            {
                propertyDetails = propertyDetails.Where(x => x.StateName == property.StateName);
            }

            if (!string.IsNullOrEmpty(property.CityName))
            {
                propertyDetails = propertyDetails.Where(x => x.CityName == property.CityName);
            }

            if (!string.IsNullOrEmpty(property.ColonyName))
            {
                propertyDetails = propertyDetails.Where(x => x.ColonyName == property.ColonyName);
            }

            if (!string.IsNullOrEmpty(property.ReligionName))
            {
                propertyDetails = propertyDetails.Where(x => x.ReligionName == property.ReligionName);
            }

            if (!string.IsNullOrEmpty(property.FurnishedName))
            {
                propertyDetails = propertyDetails.Where(x => x.FurnishedName == property.FurnishedName);
            }

            if (!string.IsNullOrEmpty(property.WaterName))
            {
                propertyDetails = propertyDetails.Where(x => x.WaterName == property.WaterName);
            }

            if (!string.IsNullOrEmpty(property.LpgName))
            {
                propertyDetails = propertyDetails.Where(x => x.LpgName == property.LpgName);
            }

            if (!string.IsNullOrEmpty(property.ElectricityName))
            {
                propertyDetails = propertyDetails.Where(x => x.ElectricityName == property.ElectricityName);
            }

            if (!string.IsNullOrEmpty(property.StairName))
            {
                propertyDetails = propertyDetails.Where(x => x.StairName == property.StairName);
            }

            if (!string.IsNullOrEmpty(property.RoofName))
            {
                propertyDetails = propertyDetails.Where(x => x.RoofName == property.RoofName);
            }

            if (!string.IsNullOrEmpty(property.CookingName))
            {
                propertyDetails = propertyDetails.Where(x => x.CookingName == property.CookingName);
            }

            // Return the filtered list of properties to the view
            return View("PropertyList", propertyDetails.ToList());
        }


        public ActionResult AddWishList(string Pid = "")
        {
            if (string.IsNullOrEmpty(Pid))
            {
                TempData["datachange"] = "Property Id Not Given";
            }
            PropertyDetail property = db.PropertyDetails.Where(x => x.PropertyId == Pid).FirstOrDefault();
            if (property == null)
            {
                TempData["datachange"] = "Property Not Found";
            }
            UserLogin user = db.UserLogins.Where(x => x.Username == User.Identity.Name).FirstOrDefault();
            WishList wish = db.WishLists.Where(x => x.UserId == user.Id && x.PropertyId == Pid).FirstOrDefault();
            if (wish != null)
            {
                TempData["datachange"] = "Property Already Exist in WishList.";
            }
            else
            {
                WishList wishList = new WishList
                {
                    UserId = user.Id,
                    PropertyId = Pid
                };
                db.WishLists.Add(wishList);
                db.SaveChanges();
                TempData["datachange"] = "Property Successfully Added in WishList.";
            }
            return Redirect(Request.UrlReferrer.AbsoluteUri);
        }

        public ActionResult MyWishList()
        {
            var login = db.UserLogins.Where(x => x.Username == User.Identity.Name).FirstOrDefault();
            var wish = db.WishLists.Where(x => x.UserId == login.Id).Select(x => x.PropertyId).ToList();
            var properties = db.PropertyDetails.Where(x => wish.Contains(x.PropertyId)).ToList();
            return View(properties.ToList());
        }

        public ActionResult BookVisitList()
        {
            var login = db.UserLogins.Where(x => x.Username == User.Identity.Name).FirstOrDefault();
            var wish = db.WishLists.Where(x => x.UserId == login.Id).Select(x => x.PropertyId).ToList();
            var properties = db.PropertyDetails.Where(x => wish.Contains(x.PropertyId)).ToList();
            return View(properties.ToList());
        }
        public ActionResult BookVisit(string Pid = "")
        {
            if (string.IsNullOrEmpty(Pid))
            {
                TempData["datachange"] = "Property Id Not Given";
                return RedirectToAction("BookVisitList");
            }
            var login = db.UserLogins.Where(x => x.Username == User.Identity.Name).FirstOrDefault();
            BookingVisit booking = new BookingVisit
            {
                Id = 0,
                UserId = login.Id,
                PropertyId = Pid,
                CreatedDate = DateTime.Now,
                IsActive = false
            };
            return View(booking);
        }

        [HttpPost]
        public ActionResult BookingCreate(BookingVisit booking)
        {
            booking.IsActive = false;
            booking.CreatedDate = DateTime.Today;
            db.BookingVisits.Add(booking);
            db.SaveChanges();
            TempData["datachange"] = "Booking Is Succefully Created. Let's We Approve Them.";
            return RedirectToAction("BookVisitList");
        }

        public ActionResult StatusConfirm()
        {
            var login = db.UserLogins.Where(x => x.Username == User.Identity.Name).FirstOrDefault();
            var bookings = db.BookingVisits.Where(x => x.UserId == login.Id);
            return View(bookings);
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