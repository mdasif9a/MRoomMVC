using System;
using System.Web.Mvc;
using System.Data.Entity;
using System.Collections.Generic;
using System.Linq;
using MRoomMVC.Data;
using MRoomMVC.Models;
using MRoomMVC.ViewModels;

namespace MRoomMVC.Controllers
{
    [Authorize(Roles = "Admin")]
    public class LocationMasterController : Controller
    {
        private readonly MRoomDbContext db = new MRoomDbContext();

        [NonAction]
        private void MyCountries()
        {
            ViewBag.LCountryName = new SelectList(db.CountryMasters.Where(x => x.Status == "Active").AsNoTracking().ToList(), "Id", "Name");
        }

        [AllowAnonymous]
        public JsonResult GetStates(int countryId)
        {
            List<StateMaster> states = db.StateMasters.Where(x => x.CountryId == countryId).ToList();
            return Json(states, JsonRequestBehavior.AllowGet);
        }
        [AllowAnonymous]
        public JsonResult GetCities(int countryId, int stateId)
        {
            List<CityMaster> cities = db.CityMasters.Where(x => x.StateId == stateId && x.CountryId == countryId).ToList();
            return Json(cities, JsonRequestBehavior.AllowGet);
        }

        // Country Crud Operations
        public ActionResult CountryMasterList()
        {
            List<CountryMaster> cities = db.CountryMasters.ToList();
            return View(cities);
        }

        public ActionResult CountryMasterCreate()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CountryMasterCreate(CountryMaster country)
        {
            if (ModelState.IsValid)
            {
                db.CountryMasters.Add(country);
                db.SaveChanges();
                TempData["datachange"] = "Country is Successfully Saved.";
                return RedirectToAction("CountryMasterList");
            }
            else
            {
                TempData["datachange"] = "Country is Not Saved.";
            }
            return View(country);
        }

        public ActionResult CountryMasterEdit(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }
            CountryMaster country = db.CountryMasters.Find(id);
            if (country == null)
            {
                return Content("Nothing Found");
            }
            return View(country);
        }

        [HttpPost]
        public ActionResult CountryMasterEdit(CountryMaster country)
        {
            if (ModelState.IsValid)
            {
                db.Entry(country).State = EntityState.Modified;
                db.SaveChanges();
                TempData["datachange"] = "Country is Successfully Updated.";
                return RedirectToAction("CountryMasterList");
            }
            else
            {
                TempData["datachange"] = "Country is Not Updated.";
            }
            return View(country);
        }

        public ActionResult CountryMasterDelete(int id)
        {
            CountryMaster country = db.CountryMasters.Find(id);
            if (country != null)
            {
                db.CountryMasters.Remove(country);
                db.SaveChanges();
                TempData["datachange"] = "Country Delete.";
            }
            else
            {
                TempData["datachange"] = "Data Not Delete.";
            }
            return RedirectToAction("CountryMasterList");
        }

        // State Crud Operations
        public ActionResult StateMasterList()
        {
            var states = (from st in db.StateMasters
                          join ct in db.CountryMasters on st.CountryId equals ct.Id
                          select new StateMasterView
                          {
                              Id = st.Id,
                              CountryId = st.CountryId,
                              Name = st.Name,
                              Status = st.Status,
                              CountryName = ct.Name
                          }).ToList();
            return View(states);
        }

        public ActionResult StateMasterCreate()
        {
            MyCountries();
            return View();
        }

        [HttpPost]
        public ActionResult StateMasterCreate(StateMaster state1)
        {
            if (ModelState.IsValid)
            {
                db.StateMasters.Add(state1);
                db.SaveChanges();
                TempData["datachange"] = "State is Successfully Saved.";
                return RedirectToAction("StateMasterList");
            }
            else
            {
                MyCountries();
                TempData["datachange"] = "State is Not Saved.";
            }
            return View(state1);
        }

        public ActionResult StateMasterEdit(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }
            StateMaster country = db.StateMasters.Find(id);
            if (country == null)
            {
                return Content("Nothing Found");
            }
            MyCountries();
            return View(country);
        }

        [HttpPost]
        public ActionResult StateMasterEdit(StateMaster state1)
        {
            if (ModelState.IsValid)
            {
                db.Entry(state1).State = EntityState.Modified;
                db.SaveChanges();
                TempData["datachange"] = "State is Successfully Updated.";
                return RedirectToAction("StateMasterList");
            }
            else
            {
                MyCountries();
                TempData["datachange"] = "State is Not Updated.";
            }
            return View(state1);
        }

        public ActionResult StateMasterDelete(int id)
        {
            StateMaster country = db.StateMasters.Find(id);
            if (country != null)
            {
                db.StateMasters.Remove(country);
                db.SaveChanges();
                TempData["datachange"] = "State Delete.";
            }
            else
            {
                TempData["datachange"] = "Data Not Delete.";
            }
            return RedirectToAction("StateMasterList");
        }

        // City Crud Operations
        public ActionResult CityMasterList()
        {
            var cities = (from cy in db.CityMasters
                          join st in db.StateMasters on cy.StateId equals st.Id
                          join ct in db.CountryMasters on cy.CountryId equals ct.Id
                          select new CityMasterView
                          {
                              Id = cy.Id,
                              StateId = cy.StateId,
                              CountryId = cy.CountryId,
                              Name = cy.Name,
                              Status = cy.Status,
                              StateName = st.Name,
                              CountryName = ct.Name,
                          }).ToList();
            return View(cities);
        }

        public ActionResult CityMasterCreate()
        {
            MyCountries();
            return View();
        }

        [HttpPost]
        public ActionResult CityMasterCreate(CityMaster city)
        {
            if (ModelState.IsValid)
            {
                db.CityMasters.Add(city);
                db.SaveChanges();
                TempData["datachange"] = "City is Successfully Saved.";
                return RedirectToAction("CityMasterList");
            }
            else
            {
                MyCountries();
                TempData["datachange"] = "City is Not Saved.";
            }
            return View(city);
        }

        public ActionResult CityMasterEdit(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }
            CityMaster city = db.CityMasters.Find(id);
            if (city == null)
            {
                return Content("Nothing Found");
            }
            MyCountries();
            ViewBag.LStateName = new SelectList(db.StateMasters.Where(x => x.Status == "Active" && x.CountryId == city.CountryId).AsNoTracking().ToList(), "Id", "Name");
            return View(city);
        }

        [HttpPost]
        public ActionResult CityMasterEdit(CityMaster city)
        {
            if (ModelState.IsValid)
            {
                db.Entry(city).State = EntityState.Modified;
                db.SaveChanges();
                TempData["datachange"] = "City is Successfully Updated.";
                return RedirectToAction("CityMasterList");
            }
            else
            {
                MyCountries();
                ViewBag.LStateName = new SelectList(db.StateMasters.Where(x => x.Status == "Active" && x.CountryId == city.CountryId).AsNoTracking().ToList(), "Id", "Name");
                TempData["datachange"] = "City is Not Updated.";
            }
            return View(city);
        }

        public ActionResult CityMasterDelete(int id)
        {
            CityMaster city = db.CityMasters.Find(id);
            if (city != null)
            {
                db.CityMasters.Remove(city);
                db.SaveChanges();
                TempData["datachange"] = "City Delete.";
            }
            else
            {
                TempData["datachange"] = "Data Not Delete.";
            }
            return RedirectToAction("CityMasterList");
        }


        //Colony/Muhalla Crud
        public ActionResult ColonyMuhallaList()
        {
            var muhallas = (from nb in db.ColonyMuhallas
                            join st in db.StateMasters on nb.StateId equals st.Id
                            join cy in db.CityMasters on nb.CityId equals cy.Id
                            join ct in db.CountryMasters on nb.CountryId equals ct.Id
                            select new ColonyMuhallaView
                            {
                                Id = nb.Id,
                                CountryId = nb.CountryId,
                                StateId = nb.StateId,
                                CityId = nb.CityId,
                                ColonyName = nb.ColonyName,
                                Status = nb.Status,
                                CountryName = ct.Name,
                                StateName = st.Name,
                                CityName = cy.Name,
                                PinCode = nb.PinCode,
                                Zone = nb.Zone
                            }).ToList();
            return View(muhallas);
        }

        public ActionResult ColonyMuhallaCreate()
        {
            MyCountries();
            return View();
        }

        [HttpPost]
        public ActionResult ColonyMuhallaCreate(ColonyMuhalla muhalla)
        {
            if (ModelState.IsValid)
            {
                db.ColonyMuhallas.Add(muhalla);
                db.SaveChanges();
                TempData["datachange"] = "Colony-Muhalla is Successfully Saved.";
                return RedirectToAction("ColonyMuhallaList");
            }
            else
            {
                MyCountries();
                TempData["datachange"] = "Colony-Muhalla is Not Saved.";
            }
            return View(muhalla);
        }

        public ActionResult ColonyMuhallaEdit(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }
            ColonyMuhalla muhalla = db.ColonyMuhallas.Find(id);
            if (muhalla == null)
            {
                return Content("Nothing Found");
            }
            MyCountries();
            ViewBag.LStateName = new SelectList(db.StateMasters.Where(x => x.Status == "Active" && x.CountryId == muhalla.CountryId).AsNoTracking().ToList(), "Id", "Name");
            ViewBag.LCityName = new SelectList(db.CityMasters.Where(x => x.Status == "Active" && x.StateId == muhalla.StateId && x.CountryId == muhalla.CountryId).AsNoTracking().ToList(), "Id", "Name");
            return View(muhalla);
        }

        [HttpPost]
        public ActionResult ColonyMuhallaEdit(ColonyMuhalla muhalla)
        {
            if (ModelState.IsValid)
            {
                db.Entry(muhalla).State = EntityState.Modified;
                db.SaveChanges();
                TempData["datachange"] = "Colony-Muhalla Successfully Updated.";
                return RedirectToAction("ColonyMuhallaList");
            }
            else
            {
                MyCountries();
                ViewBag.LStateName = new SelectList(db.StateMasters.Where(x => x.Status == "Active" && x.CountryId == muhalla.CountryId).AsNoTracking().ToList(), "Id", "Name");
                ViewBag.LCityName = new SelectList(db.CityMasters.Where(x => x.Status == "Active" && x.StateId == muhalla.StateId && x.CountryId == muhalla.CountryId).AsNoTracking().ToList(), "Id", "Name");
                TempData["datachange"] = "Colony-Muhalla Not Updated.";
            }
            return View(muhalla);
        }

        public ActionResult ColonyMuhallaDelete(int id)
        {
            ColonyMuhalla colony = db.ColonyMuhallas.Find(id);
            if (colony != null)
            {
                db.ColonyMuhallas.Remove(colony);
                db.SaveChanges();
                TempData["datachange"] = "Colony-Muhalla Delete.";
            }
            else
            {
                TempData["datachange"] = "Data Not Delete.";
            }
            return RedirectToAction("ColonyMuhallaList");
        }

        //NearBy Crud
        public ActionResult NearByList()
        {
            var nearbies = (from nb in db.NearBies
                            join st in db.StateMasters on nb.StateId equals st.Id
                            join cy in db.CityMasters on nb.CityId equals cy.Id
                            join ct in db.CountryMasters on nb.CountryId equals ct.Id
                            select new NearByView
                            {
                                Id = nb.Id,
                                CountryId = nb.CountryId,
                                StateId = nb.StateId,
                                CityId = nb.CityId,
                                NearByName = nb.NearByName,
                                Status = nb.Status,
                                CountryName = ct.Name,
                                StateName = st.Name,
                                CityName = cy.Name
                            }).ToList();
            return View(nearbies);
        }

        public ActionResult NearByCreate()
        {
            MyCountries();
            return View();
        }

        [HttpPost]
        public ActionResult NearByCreate(NearBy nearby)
        {
            if (ModelState.IsValid)
            {
                db.NearBies.Add(nearby);
                db.SaveChanges();
                TempData["datachange"] = "Near By is Successfully Saved.";
                return RedirectToAction("NearByList");
            }
            else
            {
                MyCountries();
                TempData["datachange"] = "Near By is Not Saved.";
            }
            return View(nearby);
        }

        public ActionResult NearByEdit(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }
            NearBy nearby = db.NearBies.Find(id);
            if (nearby == null)
            {
                return Content("Nothing Found");
            }
            MyCountries();
            ViewBag.LStateName = new SelectList(db.StateMasters.Where(x => x.Status == "Active" && x.CountryId == nearby.CountryId).AsNoTracking().ToList(), "Id", "Name");
            ViewBag.LCityName = new SelectList(db.CityMasters.Where(x => x.Status == "Active" && x.StateId == nearby.StateId && x.CountryId == nearby.CountryId).AsNoTracking().ToList(), "Id", "Name");
            return View(nearby);
        }

        [HttpPost]
        public ActionResult NearByEdit(NearBy nearby)
        {
            if (ModelState.IsValid)
            {
                db.Entry(nearby).State = EntityState.Modified;
                db.SaveChanges();
                TempData["datachange"] = "Near By is Successfully Updated.";
                return RedirectToAction("NearByList");
            }
            else
            {
                MyCountries();
                ViewBag.LStateName = new SelectList(db.StateMasters.Where(x => x.Status == "Active" && x.CountryId == nearby.CountryId).AsNoTracking().ToList(), "Id", "Name");
                ViewBag.LCityName = new SelectList(db.CityMasters.Where(x => x.Status == "Active" && x.StateId == nearby.StateId && x.CountryId == nearby.CountryId).AsNoTracking().ToList(), "Id", "Name");
                TempData["datachange"] = "Near By is Not Updated.";
            }
            return View(nearby);
        }

        public ActionResult NearByDelete(int id)
        {
            NearBy type = db.NearBies.Find(id);
            if (type != null)
            {
                db.NearBies.Remove(type);
                db.SaveChanges();
                TempData["datachange"] = "Near By Delete.";
            }
            else
            {
                TempData["datachange"] = "Data Not Delete.";
            }
            return RedirectToAction("NearByList");
        }



    }
}
