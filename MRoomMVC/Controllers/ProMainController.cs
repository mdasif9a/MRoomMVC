﻿using System;
using System.Web.Mvc;
using System.Data.Entity;
using System.Collections.Generic;
using System.Linq;
using MRoomMVC.Data;
using MRoomMVC.Models;
using MRoomMVC.ViewModels;
using System.IO;
using System.Web;

namespace MRoomMVC.Controllers
{
    public class ProMainController : Controller
    {
        private readonly MRoomDbContext db = new MRoomDbContext();

        [Authorize(Roles = "Admin")]
        public ActionResult ApproveProperty()
        {
            List<ApprovePro> approves = (from pd in db.PropertyDetails
                                         join ur in db.UserLogins on pd.UserId equals ur.Id
                                         select new ApprovePro
                                         {
                                             PropertyId = pd.PropertyId,
                                             UserName = ur.Username,
                                             Name = pd.Name,
                                             CreatedDate = pd.CreatedDate,
                                             IsActive = pd.IsActive,
                                             ApprovedBy = pd.ApprovedBy,
                                             UniqueName = pd.UniqueName,
                                             VerifiedBy = pd.VerifiedBy
                                         }).ToList();
            return View(approves);
        }

        [Authorize(Roles = "Admin")]
        public ActionResult ApproveCreate(string Pid)
        {
            if (string.IsNullOrEmpty(Pid))
            {
                return HttpNotFound();
            }
            PropertyDetail property = db.PropertyDetails.Where(x => x.PropertyId == Pid).FirstOrDefault();
            ApprovePro approve = new ApprovePro
            {
                PropertyId = property.PropertyId,
                ApprovedBy = property.ApprovedBy,
                UniqueName = property.UniqueName,
                VerifiedBy = property.VerifiedBy,
                IsActive = property.IsActive
            };
            if (property == null)
            {
                return HttpNotFound();
            }
            return View(approve);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public ActionResult ApproveCreate(ApprovePro pro)
        {
            PropertyDetail property = db.PropertyDetails.Where(x => x.PropertyId == pro.PropertyId).FirstOrDefault();
            property.ApprovedBy = pro.ApprovedBy;
            property.UniqueName = pro.UniqueName;
            property.VerifiedBy = pro.VerifiedBy;
            property.IsActive = pro.IsActive;
            db.Entry(property).State = EntityState.Modified;
            db.SaveChanges();
            TempData["datachange"] = "Property Successfully Verified.";
            return RedirectToAction("ApproveCreate", new { Pid = pro.PropertyId });
        }

        [Authorize(Roles = "Admin")]
        public ActionResult ProAllList()
        {
            var result = db.PropertyDetails
                           .AsNoTracking()
                           .ToList();
            return View(result);
        }


        [Authorize(Roles = "Admin")]
        public ActionResult ProSaleList()
        {
            List<PropertyDetail> properties = db.PropertyDetails.Where(x => x.PropertyFor == "Sell").AsNoTracking().ToList();
            return View(properties);
        }

        [Authorize(Roles = "Admin")]
        public ActionResult ProRentList()
        {
            List<PropertyDetail> properties = db.PropertyDetails.Where(x => x.PropertyFor == "Rent").AsNoTracking().ToList();
            return View(properties);
        }

        [Authorize(Roles = "Admin")]
        public ActionResult PropertyCreate(string TypeFor = "")
        {
            PropertyDetail property = new PropertyDetail();
            ViewBag.LMyUsers = new SelectList(db.UserLogins.AsNoTracking().Where(x => x.Role == "LandLords").OrderBy(x => x.Username).ToList(), "Id", "Username");
            ViewBag.LPropertyType = new SelectList(db.PropertyTypes.OrderBy(x => x.PropertyTypeName).AsNoTracking().ToList(), "Id", "PropertyTypeName");
            ViewBag.LBHK = new SelectList(db.BHKTypes.Where(x => x.Status == "Active").OrderBy(x => x.BHKName).AsNoTracking().ToList(), "BHKName", "BHKName");
            ViewBag.LToiletType = new SelectList(db.ToiletTypes.Where(x => x.Status == "Active").OrderBy(x => x.Name).AsNoTracking().ToList(), "Name", "Name");
            ViewBag.LParkingType = new SelectList(db.ParkingTypes.Where(x => x.Status == "Active").OrderBy(x => x.Name).AsNoTracking().ToList(), "Name", "Name");
            ViewBag.LParkingVisitors = new SelectList(db.ParkingVisitors.Where(x => x.Status == "Active").AsNoTracking().ToList(), "Name", "Name");
            ViewBag.LFloor = new SelectList(db.FloorTypes.Where(x => x.Status == "Active").OrderBy(x => x.FloorTypeName).AsNoTracking().ToList(), "FloorTypeName", "FloorTypeName");
            ViewBag.LFirstPriority = new SelectList(db.FirstPriorities.Where(x => x.Status == "Active").OrderBy(x => x.Name).AsNoTracking().ToList(), "Name", "Name");
            ViewBag.LCountry = new SelectList(db.CountryMasters.Where(x => x.Status == "Active").OrderBy(x => x.Name).AsNoTracking().ToList(), "Name", "Name");
            ViewBag.LReligion = new SelectList(db.Religions.Where(x => x.Status == "Active").OrderBy(x => x.Name).AsNoTracking().ToList(), "Name", "Name");
            ViewBag.LFurnished = new SelectList(db.FurnishedTypes.Where(x => x.Status == "Active").OrderBy(x => x.Name).AsNoTracking().ToList(), "Name", "Name");
            ViewBag.LWater = new SelectList(db.WaterSupplies.Where(x => x.Status == "Active").OrderBy(x => x.Name).AsNoTracking().ToList(), "Name", "Name");
            ViewBag.LLpg = new SelectList(db.Lpgs.Where(x => x.Status == "Active").OrderBy(x => x.Name).AsNoTracking().ToList(), "Name", "Name");
            ViewBag.LElectricity = new SelectList(db.Electricities.Where(x => x.Status == "Active").OrderBy(x => x.Name).AsNoTracking().ToList(), "Name", "Name");
            ViewBag.LStair = new SelectList(db.Stairs.Where(x => x.Status == "Active").OrderBy(x => x.Name).AsNoTracking().ToList(), "Name", "Name");
            ViewBag.LRoof = new SelectList(db.Roofs.Where(x => x.Status == "Active").OrderBy(x => x.Name).AsNoTracking().ToList(), "Name", "Name");
            ViewBag.LCooking = new SelectList(db.CookingItems.Where(x => x.Status == "Active").OrderBy(x => x.Name).AsNoTracking().ToList(), "Name", "Name");
            ViewBag.LNearBies = new SelectList(db.NearBies.Where(x => x.Status == "Active").OrderBy(x => x.NearByName).AsNoTracking().ToList(), "Id", "NearByName");

            if (!string.IsNullOrEmpty(TypeFor))
            {
                property.PropertyFor = TypeFor;
            }
            return View(property);
        }


        [NonAction]
        private string SaveFile(HttpPostedFileBase file, string subfolder)
        {
            if (file != null && file.ContentLength > 0)
            {
                string imgurl = "/Content/" + subfolder + "/";
                string filename = Guid.NewGuid() + DateTime.UtcNow.Ticks.ToString() + Path.GetExtension(file.FileName);
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

        [NonAction]
        private string EditSaveFile(string OldUrl, HttpPostedFileBase file, string subfolder)
        {
            if (file != null && file.ContentLength > 0)
            {
                string imgurl = "/Content/" + subfolder + "/";
                string filename = Guid.NewGuid() + DateTime.UtcNow.Ticks.ToString() + Path.GetExtension(file.FileName);
                string filePath = Server.MapPath(imgurl);
                if (!Directory.Exists(filePath))
                {
                    Directory.CreateDirectory(filePath);
                }
                string fullFilePath = Path.Combine(filePath, filename);
                file.SaveAs(fullFilePath);
                string oldfilePath = Server.MapPath(OldUrl);
                if (System.IO.File.Exists(oldfilePath))
                {
                    System.IO.File.Delete(oldfilePath);
                }
                return imgurl + filename;
            }
            return OldUrl;
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
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
                property.IsActive = true;
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
                return RedirectToAction("ProAllList");
            }
            else
            {
                TempData["datachange"] = "Property Type is Not Saved.";
            }
            return View(property);
        }

        [Authorize(Roles = "Admin")]
        public ActionResult PropertyEdit(int id)
        {
            PropertyDetail property = db.PropertyDetails.Find(id);
            ViewBag.LMyUsers = new SelectList(db.UserLogins.AsNoTracking().Where(x => x.Role == "LandLords").OrderBy(x => x.Username).ToList(), "Id", "Username");
            ViewBag.LPropertyType = new SelectList(db.PropertyTypes.OrderBy(x => x.PropertyTypeName).AsNoTracking().ToList(), "Id", "PropertyTypeName");
            ViewBag.LBHK = new SelectList(db.BHKTypes.Where(x => x.Status == "Active").OrderBy(x => x.BHKName).AsNoTracking().ToList(), "BHKName", "BHKName");
            ViewBag.LToiletType = new SelectList(db.ToiletTypes.Where(x => x.Status == "Active").OrderBy(x => x.Name).AsNoTracking().ToList(), "Name", "Name");
            ViewBag.LParkingType = new SelectList(db.ParkingTypes.Where(x => x.Status == "Active").OrderBy(x => x.Name).AsNoTracking().ToList(), "Name", "Name");
            ViewBag.LParkingVisitors = new SelectList(db.ParkingVisitors.Where(x => x.Status == "Active").AsNoTracking().ToList(), "Name", "Name");
            ViewBag.LFloor = new SelectList(db.FloorTypes.Where(x => x.Status == "Active").OrderBy(x => x.FloorTypeName).AsNoTracking().ToList(), "FloorTypeName", "FloorTypeName");
            ViewBag.LFirstPriority = new SelectList(db.FirstPriorities.Where(x => x.Status == "Active").OrderBy(x => x.Name).AsNoTracking().ToList(), "Name", "Name");
            ViewBag.LCountry = new SelectList(db.CountryMasters.Where(x => x.Status == "Active").OrderBy(x => x.Name).AsNoTracking().ToList(), "Name", "Name");
            ViewBag.LReligion = new SelectList(db.Religions.Where(x => x.Status == "Active").OrderBy(x => x.Name).AsNoTracking().ToList(), "Name", "Name");
            ViewBag.LFurnished = new SelectList(db.FurnishedTypes.Where(x => x.Status == "Active").OrderBy(x => x.Name).AsNoTracking().ToList(), "Name", "Name");
            ViewBag.LWater = new SelectList(db.WaterSupplies.Where(x => x.Status == "Active").OrderBy(x => x.Name).AsNoTracking().ToList(), "Name", "Name");
            ViewBag.LLpg = new SelectList(db.Lpgs.Where(x => x.Status == "Active").OrderBy(x => x.Name).AsNoTracking().ToList(), "Name", "Name");
            ViewBag.LElectricity = new SelectList(db.Electricities.Where(x => x.Status == "Active").OrderBy(x => x.Name).AsNoTracking().ToList(), "Name", "Name");
            ViewBag.LStair = new SelectList(db.Stairs.Where(x => x.Status == "Active").OrderBy(x => x.Name).AsNoTracking().ToList(), "Name", "Name");
            ViewBag.LRoof = new SelectList(db.Roofs.Where(x => x.Status == "Active").OrderBy(x => x.Name).AsNoTracking().ToList(), "Name", "Name");
            ViewBag.LCooking = new SelectList(db.CookingItems.Where(x => x.Status == "Active").OrderBy(x => x.Name).AsNoTracking().ToList(), "Name", "Name");

            //For Edit Sections
            ViewBag.LPropertyVariant = new SelectList(db.PropertyVariants.Where(x => x.Status == "Active" && x.PropertyTypeId == property.PropertyTypeId)
                .OrderBy(x => x.PropertyVariantName).AsNoTracking().ToList(), "PropertyVariantName", "PropertyVariantName");
            var mycontry = db.CountryMasters.FirstOrDefault(x => x.Name == property.CountryName);
            ViewBag.LStateName = new SelectList(db.StateMasters.Where(x => x.Status == "Active" && x.CountryId == mycontry.Id)
                .OrderBy(x => x.Name).AsNoTracking().ToList(), "Name", "Name");
            var mystate = db.StateMasters.FirstOrDefault(x => x.Name == property.StateName);
            ViewBag.LCityName = new SelectList(db.CityMasters.Where(x => x.Status == "Active" && x.StateId == mystate.Id)
                .OrderBy(x => x.Name).AsNoTracking().ToList(), "Name", "Name");
            var mycity = db.CityMasters.FirstOrDefault(x => x.Name == property.CityName);
            ViewBag.LColonyName = new SelectList(db.ColonyMuhallas.Where(x => x.Status == "Active" && x.CityId == mycity.Id)
                .OrderBy(x => x.ColonyName).AsNoTracking().ToList(), "ColonyName", "ColonyName");

            List<int> pD_s = db.PD_Near.Where(x => x.PropertyId == property.Id).Select(x => x.NearById).ToList();
            property.NearBies = pD_s;
            ViewBag.LNearBies = new SelectList(db.NearBies.Where(x => x.Status == "Active" && x.CityId == mycity.Id)
                .OrderBy(x => x.NearByName).AsNoTracking().ToList(), "Id", "NearByName");

            ViewBag.LRailway = new SelectList(db.RailwayStations.Where(x => x.Status == "Active" && x.CityId == mycity.Id)
                .OrderBy(x => x.Name).AsNoTracking().ToList(), "Name", "Name");

            ViewBag.LBusStand = new SelectList(db.BusStands.Where(x => x.Status == "Active" && x.CityId == mycity.Id)
                .OrderBy(x => x.Name).AsNoTracking().ToList(), "Name", "Name");

            ViewBag.LSchoolGov = new SelectList(db.SchoolGovs.Where(x => x.Status == "Active" && x.CityId == mycity.Id)
                .OrderBy(x => x.Name).AsNoTracking().ToList(), "Name", "Name");

            ViewBag.LSchoolPvt = new SelectList(db.SchoolPvts.Where(x => x.Status == "Active" && x.CityId == mycity.Id)
                .OrderBy(x => x.Name).AsNoTracking().ToList(), "Name", "Name");

            ViewBag.LBankGov = new SelectList(db.BankGovs.Where(x => x.Status == "Active" && x.CityId == mycity.Id)
                .OrderBy(x => x.Name).AsNoTracking().ToList(), "Name", "Name");

            ViewBag.LBankPvt = new SelectList(db.BankPvts.Where(x => x.Status == "Active" && x.CityId == mycity.Id)
                .OrderBy(x => x.Name).AsNoTracking().ToList(), "Name", "Name");

            ViewBag.LHospitalGov = new SelectList(db.HospitalGovs.Where(x => x.Status == "Active" && x.CityId == mycity.Id)
                .OrderBy(x => x.Name).AsNoTracking().ToList(), "Name", "Name");

            ViewBag.LHospitalPvt = new SelectList(db.HospitalPvts.Where(x => x.Status == "Active" && x.CityId == mycity.Id)
                .OrderBy(x => x.Name).AsNoTracking().ToList(), "Name", "Name");

            ViewBag.LPublicTpt = new SelectList(db.PublicTpts.Where(x => x.Status == "Active" && x.CityId == mycity.Id)
                .OrderBy(x => x.Name).AsNoTracking().ToList(), "Name", "Name");

            ViewBag.LMarket = new SelectList(db.Markets.Where(x => x.Status == "Active" && x.CityId == mycity.Id)
                .OrderBy(x => x.Name).AsNoTracking().ToList(), "Name", "Name");

            ViewBag.LDMOffice = new SelectList(db.DmOffices.Where(x => x.Status == "Active" && x.CityId == mycity.Id)
                .OrderBy(x => x.Name).AsNoTracking().ToList(), "Name", "Name");
            return View(property);
        }


        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult PropertyEdit(PropertyDetail property, HttpPostedFileBase imageInput1, HttpPostedFileBase imageInput2, HttpPostedFileBase imageInput3, HttpPostedFileBase imageInput4, HttpPostedFileBase imageInput5, HttpPostedFileBase imageInput6)
        {
            List<PD_Near> pD_Nears = new List<PD_Near>();
            if (ModelState.IsValid)
            {
                property.Image1 = EditSaveFile(property.Image1, imageInput1, "PropertyImages");
                property.Image2 = EditSaveFile(property.Image2, imageInput2, "PropertyImages");
                property.Image3 = EditSaveFile(property.Image3, imageInput3, "PropertyImages");
                property.Image4 = EditSaveFile(property.Image4, imageInput4, "PropertyImages");
                property.Image5 = EditSaveFile(property.Image5, imageInput5, "PropertyImages");
                property.Image6 = EditSaveFile(property.Image6, imageInput6, "PropertyImages");
                property.UpdatedDate = DateTime.Today;
                db.Entry(property).State = EntityState.Modified;
                db.SaveChanges();
                pD_Nears = db.PD_Near.Where(x => x.PropertyId == property.Id).ToList();
                db.PD_Near.RemoveRange(pD_Nears);
                db.SaveChanges();
                pD_Nears.Clear();
                pD_Nears.AddRange(from item1 in property.NearBies
                                  let d_Near = new PD_Near { NearById = item1, PropertyId = property.Id }
                                  select d_Near);
                db.PD_Near.AddRange(pD_Nears);
                db.SaveChanges();
                TempData["datachange"] = "Property is Successfully Update.";
                return RedirectToAction("ProAllList");
            }
            else
            {
                TempData["datachange"] = "Property Type is Not Update.";
            }
            return View(property);
        }

        [NonAction]
        private void DeleteFile(string mainpath)
        {
            string oldfilepath = Server.MapPath(mainpath);
            if (System.IO.File.Exists(oldfilepath))
            {
                System.IO.File.Delete(oldfilepath);
            }
        }

        public ActionResult PropertyDelete(int id)
        {
            PropertyDetail property = db.PropertyDetails.Find(id);
            if (property != null)
            {
                List<PD_Near> pD_Nears = db.PD_Near.Where(x => x.PropertyId == property.Id).ToList();
                db.PropertyDetails.Remove(property);
                db.SaveChanges();
                db.PD_Near.RemoveRange(pD_Nears);
                db.SaveChanges();
                DeleteFile(property.Image1);
                DeleteFile(property.Image2);
                DeleteFile(property.Image3);
                DeleteFile(property.Image4);
                DeleteFile(property.Image5);
                DeleteFile(property.Image6);
                TempData["datachange"] = "Property Delete.";
            }
            else
            {
                TempData["datachange"] = "Data Not Delete.";
            }
            return Redirect(Request.UrlReferrer.AbsoluteUri);
        }

        public JsonResult GetPropertySub(int main)
        {
            List<PropertyVariant> variants = db.PropertyVariants.Where(x => x.PropertyTypeId == main).OrderBy(x => x.PropertyVariantName).ToList();
            return Json(variants, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetStates(string country)
        {
            var states = db.StateMasters
                   .Where(st => db.CountryMasters.Any(ct => ct.Id == st.CountryId && ct.Name == country)).OrderBy(x => x.Name)
                   .Select(st => st.Name)
                   .ToList();
            return Json(states, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetCities(string statename)
        {
            var cities = db.CityMasters
                   .Where(cty => db.StateMasters.Any(st => st.Id == cty.StateId && st.Name == statename)).OrderBy(x => x.Name)
                   .Select(cty => cty.Name)
                   .ToList();
            return Json(cities, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetColony(string cityname)
        {
            var colonies = db.ColonyMuhallas
                   .Where(cty => db.CityMasters.Any(st => st.Id == cty.CityId && st.Name == cityname)).OrderBy(x => x.ColonyName)
                   .Select(cty => cty.ColonyName)
                   .ToList();
            return Json(colonies, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetNearBy(string cityname)
        {
            var nears = db.NearBies
                   .Where(cty => db.CityMasters.Any(st => st.Id == cty.CityId && st.Name == cityname)).OrderBy(x => x.NearByName)
                   .Select(cty => new { cty.Id, cty.NearByName })
                   .ToList();
            return Json(nears, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetRailway(string cityname)
        {
            var results = db.RailwayStations
                   .Where(cty => db.CityMasters.Any(st => st.Id == cty.CityId && st.Name == cityname)).OrderBy(x => x.Name)
                   .Select(cty => cty.Name)
                   .ToList();
            return Json(results, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetBus(string cityname)
        {
            var results = db.BusStands
                   .Where(cty => db.CityMasters.Any(st => st.Id == cty.CityId && st.Name == cityname)).OrderBy(x => x.Name)
                   .Select(cty => cty.Name)
                   .ToList();
            return Json(results, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetSchoolGov(string cityname)
        {
            var results = db.SchoolGovs
                   .Where(cty => db.CityMasters.Any(st => st.Id == cty.CityId && st.Name == cityname)).OrderBy(x => x.Name)
                   .Select(cty => cty.Name)
                   .ToList();
            return Json(results, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetSchoolPvt(string cityname)
        {
            var results = db.SchoolPvts
                   .Where(cty => db.CityMasters.Any(st => st.Id == cty.CityId && st.Name == cityname)).OrderBy(x => x.Name)
                   .Select(cty => cty.Name)
                   .ToList();
            return Json(results, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetHospitalGov(string cityname)
        {
            var results = db.HospitalGovs
                   .Where(cty => db.CityMasters.Any(st => st.Id == cty.CityId && st.Name == cityname)).OrderBy(x => x.Name)
                   .Select(cty => cty.Name)
                   .ToList();
            return Json(results, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetHospitalPvt(string cityname)
        {
            var results = db.HospitalPvts
                   .Where(cty => db.CityMasters.Any(st => st.Id == cty.CityId && st.Name == cityname)).OrderBy(x => x.Name)
                   .Select(cty => cty.Name)
                   .ToList();
            return Json(results, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetBankPvt(string cityname)
        {
            var results = db.BankPvts
                   .Where(cty => db.CityMasters.Any(st => st.Id == cty.CityId && st.Name == cityname)).OrderBy(x => x.Name)
                   .Select(cty => cty.Name)
                   .ToList();
            return Json(results, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetBankGov(string cityname)
        {
            var results = db.BankGovs
                   .Where(cty => db.CityMasters.Any(st => st.Id == cty.CityId && st.Name == cityname)).OrderBy(x => x.Name)
                   .Select(cty => cty.Name)
                   .ToList();
            return Json(results, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetMarket(string cityname)
        {
            var results = db.Markets
                   .Where(cty => db.CityMasters.Any(st => st.Id == cty.CityId && st.Name == cityname)).OrderBy(x => x.Name)
                   .Select(cty => cty.Name)
                   .ToList();
            return Json(results, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetPublicTpt(string cityname)
        {
            var results = db.PublicTpts
                   .Where(cty => db.CityMasters.Any(st => st.Id == cty.CityId && st.Name == cityname)).OrderBy(x => x.Name)
                   .Select(cty => cty.Name)
                   .ToList();
            return Json(results, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetDmOffice(string cityname)
        {
            var results = db.DmOffices
                   .Where(cty => db.CityMasters.Any(st => st.Id == cty.CityId && st.Name == cityname)).OrderBy(x => x.Name)
                   .Select(cty => cty.Name)
                   .ToList();
            return Json(results, JsonRequestBehavior.AllowGet);
        }
    }
}
