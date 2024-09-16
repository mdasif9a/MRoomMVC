using System;
using System.Web.Mvc;
using System.Data.Entity;
using System.Collections.Generic;
using System.Linq;
using MRoomMVC.Data;
using MRoomMVC.Models;
using System.IO;
using System.Web;
using MRoomMVC.ViewModels;

namespace MRoomMVC.Controllers
{
    [Authorize(Roles = "Admin, LandLords, LandLords")]
    public class LandLordsController : Controller
    {
        private readonly MRoomDbContext db = new MRoomDbContext();

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
        public ActionResult Index()
        {
            var usercurrent = db.UserLogins.Where(x => x.Username == User.Identity.Name).FirstOrDefault();
            int[] myints = new int[]
            {
                db.PropertyDetails.Count(x => x.UserId == usercurrent.Id),
                db.PropertyDetails.Count(x => x.UserId == usercurrent.Id && x.IsActive),
            };
            return View(myints);
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
            ViewBag.LMyUsers = new SelectList(db.UserLogins.AsNoTracking().Where(x => x.Role != "Admin" && x.Username == User.Identity.Name).OrderBy(x => x.Username).ToList(), "Id", "Username");
            ViewBag.LPropertyType = new SelectList(db.PropertyTypes.AsNoTracking().OrderBy(x => x.PropertyTypeName).ToList(), "Id", "PropertyTypeName");
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

        public ActionResult StatusListing()
        {
            var usercurrent = db.UserLogins.Where(x => x.Username == User.Identity.Name).FirstOrDefault();
            List<ApprovePro> approves = (from pd in db.PropertyDetails
                                         where pd.UserId == usercurrent.Id
                                         select new ApprovePro
                                         {
                                             PropertyId = pd.PropertyId,
                                             Name = pd.Name,
                                             CreatedDate = pd.CreatedDate,
                                             IsActive = pd.IsActive,
                                             ApprovedBy = pd.ApprovedBy,
                                             UniqueName = pd.UniqueName,
                                             VerifiedBy = pd.VerifiedBy
                                         }).ToList();
            return View(approves);
        }


        public ActionResult Settings()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Settings(string OldPassword, string Password)
        {
            var login = db.UserLogins.Where(x => x.Username == User.Identity.Name).FirstOrDefault();
            if (login != null && login.Password == OldPassword)
            {
                login.Password = Password;
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

    }
}