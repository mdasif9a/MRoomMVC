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
    public class DistanceMasterController : Controller
    {
        private readonly MRoomDbContext db = new MRoomDbContext();


        [NonAction]
        private void MyCountries()
        {
            ViewBag.LCountryName = new SelectList(db.CountryMasters.Where(x => x.Status == "Active").AsNoTracking().ToList(), "Id", "Name");
        }

        [NonAction]
        private void MyStates(int CountryId)
        {
            ViewBag.LStateName = new SelectList(db.StateMasters.Where(x => x.Status == "Active" && x.CountryId == CountryId).AsNoTracking().ToList(), "Id", "Name");
        }

        [NonAction]
        private void MyCities(int CountryId, int StateId)
        {
            ViewBag.LCityName = new SelectList(db.CityMasters.Where(x => x.Status == "Active" && x.CountryId == CountryId && x.StateId == StateId).AsNoTracking().ToList(), "Id", "Name");
        }

        // Railway Crud Operations

        public ActionResult RailwayList()
        {
            var railways = (from nb in db.RailwayStations
                                             join st in db.StateMasters on nb.StateId equals st.Id
                                             join cy in db.CityMasters on nb.CityId equals cy.Id
                                             join ct in db.CountryMasters on nb.CountryId equals ct.Id
                                             select new RailwayStationView
                                             {
                                                 Id = nb.Id,
                                                 CountryId = nb.CountryId,
                                                 StateId = nb.StateId,
                                                 CityId = nb.CityId,
                                                 Name = nb.Name,
                                                 Status = nb.Status,
                                                 CountryName = ct.Name,
                                                 StateName = st.Name,
                                                 CityName = cy.Name
                                             }).ToList();
            return View(railways);
        }


        public ActionResult RailwayCreate()
        {
            MyCountries();
            return View();
        }

        [HttpPost]
        public ActionResult RailwayCreate(RailwayStation railway)
        {
            if (ModelState.IsValid)
            {
                db.RailwayStations.Add(railway);
                db.SaveChanges();
                TempData["datachange"] = "Railway Station is Successfully Saved.";
                return RedirectToAction("RailwayList");
            }
            else
            {
                MyCountries();
                TempData["datachange"] = "Railway Staion is Not Saved.";
            }
            return View(railway);
        }

        public ActionResult RailwayEdit(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }
            RailwayStation railway = db.RailwayStations.Find(id);
            if (railway == null)
            {
                return Content("Nothing Found");
            }
            MyCountries();
            MyStates(railway.CountryId);
            MyCities(railway.CountryId, railway.StateId);
            return View(railway);
        }

        [HttpPost]
        public ActionResult RailwayEdit(RailwayStation railway)
        {
            if (ModelState.IsValid)
            {
                db.Entry(railway).State = EntityState.Modified;
                db.SaveChanges();
                TempData["datachange"] = "Railway Station is Successfully Updated.";
                return RedirectToAction("RailwayList");
            }
            else
            {
                MyCountries();
                MyStates(railway.CountryId);
                MyCities(railway.CountryId, railway.StateId);
                TempData["datachange"] = "Railway Station is Not Updated.";
            }
            return View(railway);
        }

        public ActionResult RailwayDelete(int id)
        {
            RailwayStation railway = db.RailwayStations.Find(id);
            if (railway != null)
            {
                db.RailwayStations.Remove(railway);
                db.SaveChanges();
                TempData["datachange"] = "Railway Station Delete.";
            }
            else
            {
                TempData["datachange"] = "Data Not Delete.";
            }
            return RedirectToAction("RailwayList");
        }

        // Bus Crud Operation
        public ActionResult BusList()
        {
            var buses = (from nb in db.BusStands
                                    join st in db.StateMasters on nb.StateId equals st.Id
                                    join cy in db.CityMasters on nb.CityId equals cy.Id
                                    join ct in db.CountryMasters on nb.CountryId equals ct.Id
                                    select new BusStandView
                                    {
                                        Id = nb.Id,
                                        CountryId = nb.CountryId,
                                        StateId = nb.StateId,
                                        CityId = nb.CityId,
                                        Name = nb.Name,
                                        Status = nb.Status,
                                        CountryName = ct.Name,
                                        StateName = st.Name,
                                        CityName = cy.Name
                                    }).ToList();
            return View(buses);
        }


        public ActionResult BusCreate()
        {
            MyCountries();
            return View();
        }

        [HttpPost]
        public ActionResult BusCreate(BusStand bus)
        {
            if (ModelState.IsValid)
            {
                db.BusStands.Add(bus);
                db.SaveChanges();
                TempData["datachange"] = "Bus Stand is Successfully Saved.";
                return RedirectToAction("BusList");
            }
            else
            {
                MyCountries();
                TempData["datachange"] = "Bus Stand is Not Saved.";
            }
            return View(bus);
        }

        public ActionResult BusEdit(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }
            BusStand bus = db.BusStands.Find(id);
            if (bus == null)
            {
                return Content("Nothing Found");
            }
            MyCountries();
            MyStates(bus.CountryId);
            MyCities(bus.CountryId, bus.StateId);
            return View(bus);
        }

        [HttpPost]
        public ActionResult BusEdit(BusStand bus)
        {
            if (ModelState.IsValid)
            {
                db.Entry(bus).State = EntityState.Modified;
                db.SaveChanges();
                TempData["datachange"] = "Bus Stand is Successfully Updated.";
                return RedirectToAction("BusList");
            }
            else
            {
                MyCountries();
                MyStates(bus.CountryId);
                MyCities(bus.CountryId, bus.StateId);
                TempData["datachange"] = "Bus Stand is Not Updated.";
            }
            return View(bus);
        }

        public ActionResult BusDelete(int id)
        {
            BusStand bus = db.BusStands.Find(id);
            if (bus != null)
            {
                db.BusStands.Remove(bus);
                db.SaveChanges();
                TempData["datachange"] = "Bus Stand Delete.";
            }
            else
            {
                TempData["datachange"] = "Data Not Delete.";
            }
            return RedirectToAction("BusList");
        }

        // School Gov Crud Operation
        public ActionResult SchoolGovList()
        {
            var buses = (from nb in db.SchoolGovs
                                     join st in db.StateMasters on nb.StateId equals st.Id
                                     join cy in db.CityMasters on nb.CityId equals cy.Id
                                     join ct in db.CountryMasters on nb.CountryId equals ct.Id
                                     select new SchoolGovView
                                     {
                                         Id = nb.Id,
                                         CountryId = nb.CountryId,
                                         StateId = nb.StateId,
                                         CityId = nb.CityId,
                                         Name = nb.Name,
                                         Status = nb.Status,
                                         CountryName = ct.Name,
                                         StateName = st.Name,
                                         CityName = cy.Name
                                     }).ToList();
            return View(buses);
        }


        public ActionResult SchoolGovCreate()
        {
            MyCountries();
            return View();
        }

        [HttpPost]
        public ActionResult SchoolGovCreate(SchoolGov bus)
        {
            if (ModelState.IsValid)
            {
                db.SchoolGovs.Add(bus);
                db.SaveChanges();
                TempData["datachange"] = "SchoolGov is Successfully Saved.";
                return RedirectToAction("SchoolGovList");
            }
            else
            {
                MyCountries();
                TempData["datachange"] = "SchoolGov is Not Saved.";
            }
            return View(bus);
        }

        public ActionResult SchoolGovEdit(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }
            SchoolGov bus = db.SchoolGovs.Find(id);
            if (bus == null)
            {
                return Content("Nothing Found");
            }
            MyCountries();
            MyStates(bus.CountryId);
            MyCities(bus.CountryId, bus.StateId);
            return View(bus);
        }

        [HttpPost]
        public ActionResult SchoolGovEdit(SchoolGov bus)
        {
            if (ModelState.IsValid)
            {
                db.Entry(bus).State = EntityState.Modified;
                db.SaveChanges();
                TempData["datachange"] = "SchoolGov is Successfully Updated.";
                return RedirectToAction("SchoolGovList");
            }
            else
            {
                MyCountries();
                MyStates(bus.CountryId);
                MyCities(bus.CountryId, bus.StateId);
                TempData["datachange"] = "SchoolGov is Not Updated.";
            }
            return View(bus);
        }

        public ActionResult SchoolGovDelete(int id)
        {
            SchoolGov bus = db.SchoolGovs.Find(id);
            if (bus != null)
            {
                db.SchoolGovs.Remove(bus);
                db.SaveChanges();
                TempData["datachange"] = "SchoolGov Delete.";
            }
            else
            {
                TempData["datachange"] = "Data Not Delete.";
            }
            return RedirectToAction("SchoolGovList");
        }

        // School Pvt Crud Operation
        public ActionResult SchoolPvtList()
        {
            var buses = (from nb in db.SchoolPvts
                                     join st in db.StateMasters on nb.StateId equals st.Id
                                     join cy in db.CityMasters on nb.CityId equals cy.Id
                                     join ct in db.CountryMasters on nb.CountryId equals ct.Id
                                     select new SchoolPvtView
                                     {
                                         Id = nb.Id,
                                         CountryId = nb.CountryId,
                                         StateId = nb.StateId,
                                         CityId = nb.CityId,
                                         Name = nb.Name,
                                         Status = nb.Status,
                                         CountryName = ct.Name,
                                         StateName = st.Name,
                                         CityName = cy.Name
                                     }).ToList();
            return View(buses);
        }


        public ActionResult SchoolPvtCreate()
        {
            MyCountries();
            return View();
        }

        [HttpPost]
        public ActionResult SchoolPvtCreate(SchoolPvt bus)
        {
            if (ModelState.IsValid)
            {
                db.SchoolPvts.Add(bus);
                db.SaveChanges();
                TempData["datachange"] = "SchoolPvt is Successfully Saved.";
                return RedirectToAction("SchoolPvtList");
            }
            else
            {
                MyCountries();
                TempData["datachange"] = "SchoolPvt is Not Saved.";
            }
            return View(bus);
        }

        public ActionResult SchoolPvtEdit(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }
            SchoolPvt bus = db.SchoolPvts.Find(id);
            if (bus == null)
            {
                return Content("Nothing Found");
            }
            MyCountries();
            MyStates(bus.CountryId);
            MyCities(bus.CountryId, bus.StateId);
            return View(bus);
        }

        [HttpPost]
        public ActionResult SchoolPvtEdit(SchoolPvt bus)
        {
            if (ModelState.IsValid)
            {
                db.Entry(bus).State = EntityState.Modified;
                db.SaveChanges();
                TempData["datachange"] = "SchoolPvt is Successfully Updated.";
                return RedirectToAction("SchoolPvtList");
            }
            else
            {
                MyCountries();
                MyStates(bus.CountryId);
                MyCities(bus.CountryId, bus.StateId);
                TempData["datachange"] = "SchoolPvt is Not Updated.";
            }
            return View(bus);
        }

        public ActionResult SchoolPvtDelete(int id)
        {
            SchoolPvt bus = db.SchoolPvts.Find(id);
            if (bus != null)
            {
                db.SchoolPvts.Remove(bus);
                db.SaveChanges();
                TempData["datachange"] = "SchoolPvt Delete.";
            }
            else
            {
                TempData["datachange"] = "Data Not Delete.";
            }
            return RedirectToAction("SchoolPvtList");
        }

        //Bank Gov Crud

        public ActionResult BankGovList()
        {
            var banks = (from nb in db.BankGovs
                                   join st in db.StateMasters on nb.StateId equals st.Id
                                   join cy in db.CityMasters on nb.CityId equals cy.Id
                                   join ct in db.CountryMasters on nb.CountryId equals ct.Id
                                   select new BankGovView
                                   {
                                       Id = nb.Id,
                                       CountryId = nb.CountryId,
                                       StateId = nb.StateId,
                                       CityId = nb.CityId,
                                       Name = nb.Name,
                                       Status = nb.Status,
                                       CountryName = ct.Name,
                                       StateName = st.Name,
                                       CityName = cy.Name
                                   }).ToList();
            return View(banks);
        }

        public ActionResult BankGovCreate()
        {
            MyCountries();
            return View();
        }

        [HttpPost]
        public ActionResult BankGovCreate(BankGov bank)
        {
            if (ModelState.IsValid)
            {
                db.BankGovs.Add(bank);
                db.SaveChanges();
                TempData["datachange"] = "BankGov is Successfully Saved.";
                return RedirectToAction("BankGovList");
            }
            else
            {
                MyCountries();
                TempData["datachange"] = "BankGov is Not Saved.";
            }
            return View(bank);
        }

        public ActionResult BankGovEdit(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }
            BankGov bank = db.BankGovs.Find(id);
            if (bank == null)
            {
                return Content("Nothing Found");
            }
            MyCountries();
            MyStates(bank.CountryId);
            MyCities(bank.CountryId, bank.StateId);
            return View(bank);
        }

        [HttpPost]
        public ActionResult BankGovEdit(BankGov bank)
        {
            if (ModelState.IsValid)
            {
                db.Entry(bank).State = EntityState.Modified;
                db.SaveChanges();
                TempData["datachange"] = "BankGov is Successfully Updated.";
                return RedirectToAction("BankGovList");
            }
            else
            {
                MyCountries();
                MyStates(bank.CountryId);
                MyCities(bank.CountryId, bank.StateId);
                TempData["datachange"] = "BankGov is Not Updated.";
            }
            return View(bank);
        }

        public ActionResult BankGovDelete(int id)
        {
            BankGov bank = db.BankGovs.Find(id);
            if (bank != null)
            {
                db.BankGovs.Remove(bank);
                db.SaveChanges();
                TempData["datachange"] = "BankGov Deleted.";
            }
            else
            {
                TempData["datachange"] = "Data Not Deleted.";
            }
            return RedirectToAction("BankGovList");
        }

        //Bank Pvt Crud

        public ActionResult BankPvtList()
        {
            var banks = (from nb in db.BankPvts
                                   join st in db.StateMasters on nb.StateId equals st.Id
                                   join cy in db.CityMasters on nb.CityId equals cy.Id
                                   join ct in db.CountryMasters on nb.CountryId equals ct.Id
                                   select new BankPvtView
                                   {
                                       Id = nb.Id,
                                       CountryId = nb.CountryId,
                                       StateId = nb.StateId,
                                       CityId = nb.CityId,
                                       Name = nb.Name,
                                       Status = nb.Status,
                                       CountryName = ct.Name,
                                       StateName = st.Name,
                                       CityName = cy.Name
                                   }).ToList();
            return View(banks);
        }

        public ActionResult BankPvtCreate()
        {
            MyCountries();
            return View();
        }

        [HttpPost]
        public ActionResult BankPvtCreate(BankPvt bank)
        {
            if (ModelState.IsValid)
            {
                db.BankPvts.Add(bank);
                db.SaveChanges();
                TempData["datachange"] = "BankPvt is Successfully Saved.";
                return RedirectToAction("BankPvtList");
            }
            else
            {
                MyCountries();
                TempData["datachange"] = "BankPvt is Not Saved.";
            }
            return View(bank);
        }

        public ActionResult BankPvtEdit(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }
            BankPvt bank = db.BankPvts.Find(id);
            if (bank == null)
            {
                return Content("Nothing Found");
            }
            MyCountries();
            MyStates(bank.CountryId);
            MyCities(bank.CountryId, bank.StateId);
            return View(bank);
        }

        [HttpPost]
        public ActionResult BankPvtEdit(BankPvt bank)
        {
            if (ModelState.IsValid)
            {
                db.Entry(bank).State = EntityState.Modified;
                db.SaveChanges();
                TempData["datachange"] = "BankPvt is Successfully Updated.";
                return RedirectToAction("BankPvtList");
            }
            else
            {
                MyCountries();
                MyStates(bank.CountryId);
                MyCities(bank.CountryId, bank.StateId);
                TempData["datachange"] = "BankPvt is Not Updated.";
            }
            return View(bank);
        }

        public ActionResult BankPvtDelete(int id)
        {
            BankPvt bank = db.BankPvts.Find(id);
            if (bank != null)
            {
                db.BankPvts.Remove(bank);
                db.SaveChanges();
                TempData["datachange"] = "BankPvt Deleted.";
            }
            else
            {
                TempData["datachange"] = "Data Not Deleted.";
            }
            return RedirectToAction("BankPvtList");
        }

        //Hospital Gov Crud
        public ActionResult HospitalGovList()
        {
            var hospitals = (from nb in db.HospitalGovs
                                           join st in db.StateMasters on nb.StateId equals st.Id
                                           join cy in db.CityMasters on nb.CityId equals cy.Id
                                           join ct in db.CountryMasters on nb.CountryId equals ct.Id
                                           select new HospitalGovView
                                           {
                                               Id = nb.Id,
                                               CountryId = nb.CountryId,
                                               StateId = nb.StateId,
                                               CityId = nb.CityId,
                                               Name = nb.Name,
                                               Status = nb.Status,
                                               CountryName = ct.Name,
                                               StateName = st.Name,
                                               CityName = cy.Name
                                           }).ToList();
            return View(hospitals);
        }

        public ActionResult HospitalGovCreate()
        {
            MyCountries();
            return View();
        }

        [HttpPost]
        public ActionResult HospitalGovCreate(HospitalGov hospital)
        {
            if (ModelState.IsValid)
            {
                db.HospitalGovs.Add(hospital);
                db.SaveChanges();
                TempData["datachange"] = "HospitalGov is Successfully Saved.";
                return RedirectToAction("HospitalGovList");
            }
            else
            {
                MyCountries();
                TempData["datachange"] = "HospitalGov is Not Saved.";
            }
            return View(hospital);
        }

        public ActionResult HospitalGovEdit(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }
            HospitalGov hospital = db.HospitalGovs.Find(id);
            if (hospital == null)
            {
                return Content("Nothing Found");
            }
            MyCountries();
            MyStates(hospital.CountryId);
            MyCities(hospital.CountryId, hospital.StateId);
            return View(hospital);
        }

        [HttpPost]
        public ActionResult HospitalGovEdit(HospitalGov hospital)
        {
            if (ModelState.IsValid)
            {
                db.Entry(hospital).State = EntityState.Modified;
                db.SaveChanges();
                TempData["datachange"] = "HospitalGov is Successfully Updated.";
                return RedirectToAction("HospitalGovList");
            }
            else
            {
                MyCountries();
                MyStates(hospital.CountryId);
                MyCities(hospital.CountryId, hospital.StateId);
                TempData["datachange"] = "HospitalGov is Not Updated.";
            }
            return View(hospital);
        }

        public ActionResult HospitalGovDelete(int id)
        {
            HospitalGov hospital = db.HospitalGovs.Find(id);
            if (hospital != null)
            {
                db.HospitalGovs.Remove(hospital);
                db.SaveChanges();
                TempData["datachange"] = "HospitalGov Deleted.";
            }
            else
            {
                TempData["datachange"] = "Data Not Deleted.";
            }
            return RedirectToAction("HospitalGovList");
        }

        //Hospital Pvt Crud
        public ActionResult HospitalPvtList()
        {
            var hospitals = (from nb in db.HospitalPvts
                                           join st in db.StateMasters on nb.StateId equals st.Id
                                           join cy in db.CityMasters on nb.CityId equals cy.Id
                                           join ct in db.CountryMasters on nb.CountryId equals ct.Id
                                           select new HospitalPvtView
                                           {
                                               Id = nb.Id,
                                               CountryId = nb.CountryId,
                                               StateId = nb.StateId,
                                               CityId = nb.CityId,
                                               Name = nb.Name,
                                               Status = nb.Status,
                                               CountryName = ct.Name,
                                               StateName = st.Name,
                                               CityName = cy.Name
                                           }).ToList();
            return View(hospitals);
        }

        public ActionResult HospitalPvtCreate()
        {
            MyCountries();
            return View();
        }

        [HttpPost]
        public ActionResult HospitalPvtCreate(HospitalPvt hospital)
        {
            if (ModelState.IsValid)
            {
                db.HospitalPvts.Add(hospital);
                db.SaveChanges();
                TempData["datachange"] = "HospitalPvt is Successfully Saved.";
                return RedirectToAction("HospitalPvtList");
            }
            else
            {
                MyCountries();
                TempData["datachange"] = "HospitalPvt is Not Saved.";
            }
            return View(hospital);
        }

        public ActionResult HospitalPvtEdit(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }
            HospitalPvt hospital = db.HospitalPvts.Find(id);
            if (hospital == null)
            {
                return Content("Nothing Found");
            }
            MyCountries();
            MyStates(hospital.CountryId);
            MyCities(hospital.CountryId, hospital.StateId);
            return View(hospital);
        }

        [HttpPost]
        public ActionResult HospitalPvtEdit(HospitalPvt hospital)
        {
            if (ModelState.IsValid)
            {
                db.Entry(hospital).State = EntityState.Modified;
                db.SaveChanges();
                TempData["datachange"] = "HospitalPvt is Successfully Updated.";
                return RedirectToAction("HospitalPvtList");
            }
            else
            {
                MyCountries();
                MyStates(hospital.CountryId);
                MyCities(hospital.CountryId, hospital.StateId);
                TempData["datachange"] = "HospitalPvt is Not Updated.";
            }
            return View(hospital);
        }

        public ActionResult HospitalPvtDelete(int id)
        {
            HospitalPvt hospital = db.HospitalPvts.Find(id);
            if (hospital != null)
            {
                db.HospitalPvts.Remove(hospital);
                db.SaveChanges();
                TempData["datachange"] = "HospitalPvt Deleted.";
            }
            else
            {
                TempData["datachange"] = "Data Not Deleted.";
            }
            return RedirectToAction("HospitalPvtList");
        }

        //Market Crud Operation
        public ActionResult MarketList()
        {
            var markets = (from nb in db.Markets
                                    join st in db.StateMasters on nb.StateId equals st.Id
                                    join cy in db.CityMasters on nb.CityId equals cy.Id
                                    join ct in db.CountryMasters on nb.CountryId equals ct.Id
                                    select new MarketView
                                    {
                                        Id = nb.Id,
                                        CountryId = nb.CountryId,
                                        StateId = nb.StateId,
                                        CityId = nb.CityId,
                                        Name = nb.Name,
                                        Status = nb.Status,
                                        CountryName = ct.Name,
                                        StateName = st.Name,
                                        CityName = cy.Name
                                    }).ToList();
            return View(markets);
        }

        public ActionResult MarketCreate()
        {
            MyCountries();
            return View();
        }

        [HttpPost]
        public ActionResult MarketCreate(Market market)
        {
            if (ModelState.IsValid)
            {
                db.Markets.Add(market);
                db.SaveChanges();
                TempData["datachange"] = "Market is Successfully Saved.";
                return RedirectToAction("MarketList");
            }
            else
            {
                MyCountries();
                TempData["datachange"] = "Market is Not Saved.";
            }
            return View(market);
        }

        public ActionResult MarketEdit(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }
            Market market = db.Markets.Find(id);
            if (market == null)
            {
                return Content("Nothing Found");
            }
            MyCountries();
            MyStates(market.CountryId);
            MyCities(market.CountryId, market.StateId);
            return View(market);
        }

        [HttpPost]
        public ActionResult MarketEdit(Market market)
        {
            if (ModelState.IsValid)
            {
                db.Entry(market).State = EntityState.Modified;
                db.SaveChanges();
                TempData["datachange"] = "Market is Successfully Updated.";
                return RedirectToAction("MarketList");
            }
            else
            {
                MyCountries();
                MyStates(market.CountryId);
                MyCities(market.CountryId, market.StateId);
                TempData["datachange"] = "Market is Not Updated.";
            }
            return View(market);
        }

        public ActionResult MarketDelete(int id)
        {
            Market market = db.Markets.Find(id);
            if (market != null)
            {
                db.Markets.Remove(market);
                db.SaveChanges();
                TempData["datachange"] = "Market Deleted.";
            }
            else
            {
                TempData["datachange"] = "Data Not Deleted.";
            }
            return RedirectToAction("MarketList");
        }

        //DmOffice Crud Operation
        public ActionResult DmOfficeList()
        {
            var offices = (from nb in db.DmOffices
                                      join st in db.StateMasters on nb.StateId equals st.Id
                                      join cy in db.CityMasters on nb.CityId equals cy.Id
                                      join ct in db.CountryMasters on nb.CountryId equals ct.Id
                                      select new DmOfficeView
                                      {
                                          Id = nb.Id,
                                          CountryId = nb.CountryId,
                                          StateId = nb.StateId,
                                          CityId = nb.CityId,
                                          Name = nb.Name,
                                          Status = nb.Status,
                                          CountryName = ct.Name,
                                          StateName = st.Name,
                                          CityName = cy.Name
                                      }).ToList();
            return View(offices);
        }

        public ActionResult DmOfficeCreate()
        {
            MyCountries();
            return View();
        }

        [HttpPost]
        public ActionResult DmOfficeCreate(DmOffice office)
        {
            if (ModelState.IsValid)
            {
                db.DmOffices.Add(office);
                db.SaveChanges();
                TempData["datachange"] = "DmOffice is Successfully Saved.";
                return RedirectToAction("DmOfficeList");
            }
            else
            {
                MyCountries();
                TempData["datachange"] = "DmOffice is Not Saved.";
            }
            return View(office);
        }

        public ActionResult DmOfficeEdit(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }
            DmOffice office = db.DmOffices.Find(id);
            if (office == null)
            {
                return Content("Nothing Found");
            }
            MyCountries();
            MyStates(office.CountryId);
            MyCities(office.CountryId, office.StateId);
            return View(office);
        }

        [HttpPost]
        public ActionResult DmOfficeEdit(DmOffice office)
        {
            if (ModelState.IsValid)
            {
                db.Entry(office).State = EntityState.Modified;
                db.SaveChanges();
                TempData["datachange"] = "DmOffice is Successfully Updated.";
                return RedirectToAction("DmOfficeList");
            }
            else
            {
                MyCountries();
                MyStates(office.CountryId);
                MyCities(office.CountryId, office.StateId);
                TempData["datachange"] = "DmOffice is Not Updated.";
            }
            return View(office);
        }

        public ActionResult DmOfficeDelete(int id)
        {
            DmOffice office = db.DmOffices.Find(id);
            if (office != null)
            {
                db.DmOffices.Remove(office);
                db.SaveChanges();
                TempData["datachange"] = "DmOffice Deleted.";
            }
            else
            {
                TempData["datachange"] = "Data Not Deleted.";
            }
            return RedirectToAction("DmOfficeList");
        }

        //public tpt Crud Opt.

        public ActionResult PublicTptList()
        {
            var transports = (from nb in db.PublicTpts
                                          join st in db.StateMasters on nb.StateId equals st.Id
                                          join cy in db.CityMasters on nb.CityId equals cy.Id
                                          join ct in db.CountryMasters on nb.CountryId equals ct.Id
                                          select new PublicTptView
                                          {
                                              Id = nb.Id,
                                              CountryId = nb.CountryId,
                                              StateId = nb.StateId,
                                              CityId = nb.CityId,
                                              Name = nb.Name,
                                              Status = nb.Status,
                                              CountryName = ct.Name,
                                              StateName = st.Name,
                                              CityName = cy.Name
                                          }).ToList();
            return View(transports);
        }

        public ActionResult PublicTptCreate()
        {
            MyCountries();
            return View();
        }

        [HttpPost]
        public ActionResult PublicTptCreate(PublicTpt transport)
        {
            if (ModelState.IsValid)
            {
                db.PublicTpts.Add(transport);
                db.SaveChanges();
                TempData["datachange"] = "PublicTpt is Successfully Saved.";
                return RedirectToAction("PublicTptList");
            }
            else
            {
                MyCountries();
                TempData["datachange"] = "PublicTpt is Not Saved.";
            }
            return View(transport);
        }

        public ActionResult PublicTptEdit(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }
            PublicTpt transport = db.PublicTpts.Find(id);
            if (transport == null)
            {
                return Content("Nothing Found");
            }
            MyCountries();
            MyStates(transport.CountryId);
            MyCities(transport.CountryId, transport.StateId);
            return View(transport);
        }

        [HttpPost]
        public ActionResult PublicTptEdit(PublicTpt transport)
        {
            if (ModelState.IsValid)
            {
                db.Entry(transport).State = EntityState.Modified;
                db.SaveChanges();
                TempData["datachange"] = "PublicTpt is Successfully Updated.";
                return RedirectToAction("PublicTptList");
            }
            else
            {
                MyCountries();
                MyStates(transport.CountryId);
                MyCities(transport.CountryId, transport.StateId);
                TempData["datachange"] = "PublicTpt is Not Updated.";
            }
            return View(transport);
        }

        public ActionResult PublicTptDelete(int id)
        {
            PublicTpt transport = db.PublicTpts.Find(id);
            if (transport != null)
            {
                db.PublicTpts.Remove(transport);
                db.SaveChanges();
                TempData["datachange"] = "PublicTpt Deleted.";
            }
            else
            {
                TempData["datachange"] = "Data Not Deleted.";
            }
            return RedirectToAction("PublicTptList");
        }

    }
}
