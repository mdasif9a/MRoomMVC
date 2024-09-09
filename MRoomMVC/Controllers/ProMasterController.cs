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
    public class ProMasterController : Controller
    {
        private readonly MRoomDbContext db = new MRoomDbContext();



        // Property Type Crud Operations
        public ActionResult PropertyTypeList()
        {
            List<PropertyType> properties = db.PropertyTypes.ToList();
            return View(properties);
        }


        public ActionResult PropertyTypeCreate()
        {
            return View();
        }

        [HttpPost]
        public ActionResult PropertyTypeCreate(PropertyType property)
        {
            if (ModelState.IsValid)
            {
                db.PropertyTypes.Add(property);
                db.SaveChanges();
                TempData["datachange"] = "Property Type is Successfully Saved.";
                return RedirectToAction("PropertyTypeList");
            }
            else
            {
                TempData["datachange"] = "Property Type is Not Saved.";
            }
            return View(property);
        }

        public ActionResult PropertyTypeEdit(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }
            PropertyType property = db.PropertyTypes.Find(id);
            if (property == null)
            {
                return Content("Nothing Found");
            }
            return View(property);
        }

        [HttpPost]
        public ActionResult PropertyTypeEdit(PropertyType property)
        {
            if (ModelState.IsValid)
            {
                db.Entry(property).State = EntityState.Modified;
                db.SaveChanges();
                TempData["datachange"] = "Property Type is Successfully Updated.";
                return RedirectToAction("PropertyTypeList");
            }
            else
            {
                TempData["datachange"] = "Property Type is Not Updated.";
            }
            return View(property);
        }

        public ActionResult PropertyTypeDelete(int id)
        {
            PropertyType type = db.PropertyTypes.Find(id);
            if (type != null)
            {
                db.PropertyTypes.Remove(type);
                db.SaveChanges();
                TempData["datachange"] = "PropertyType Delete.";
            }
            else
            {
                TempData["datachange"] = "Data Not Delete.";
            }
            return RedirectToAction("PropertyTypeList");
        }

        // Property Variant Crud Operations
        public ActionResult PropertyVariantList()
        {
            var properties = (from pv in db.PropertyVariants
                                                join pt in db.PropertyTypes on pv.PropertyTypeId equals pt.Id
                                                select new PropertyVariantView
                                                {
                                                    Id = pv.Id,
                                                    PropertyTypeId = pv.PropertyTypeId,
                                                    PropertyTypeName = pt.PropertyTypeName,
                                                    PropertyVariantName = pv.PropertyVariantName,
                                                    Status = pv.Status
                                                }).ToList();
            return View(properties);
        }


        public ActionResult PropertyVariantCreate()
        {
            ViewBag.LPropertyType = new SelectList(db.PropertyTypes
                .Where(x => x.Status == "Active")
                .AsNoTracking()
                .ToList(), "Id", "PropertyTypeName");
            return View();
        }

        [HttpPost]
        public ActionResult PropertyVariantCreate(PropertyVariant property)
        {
            if (ModelState.IsValid)
            {
                db.PropertyVariants.Add(property);
                db.SaveChanges();
                TempData["datachange"] = "Property Variant is Successfully Saved.";
                return RedirectToAction("PropertyVariantList");
            }
            else
            {
                ViewBag.LPropertyType = new SelectList(db.PropertyTypes
                .Where(x => x.Status == "Active")
                .AsNoTracking()
                .ToList(), "Id", "PropertyTypeName");
                TempData["datachange"] = "Property Variant is Not Saved.";
            }
            return View(property);
        }

        public ActionResult PropertyVariantEdit(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }
            PropertyVariant property = db.PropertyVariants.Find(id);
            if (property == null)
            {
                return Content("Nothing Found");
            }
            ViewBag.LPropertyType = new SelectList(db.PropertyTypes
                .Where(x => x.Status == "Active")
                .AsNoTracking()
                .ToList(), "Id", "PropertyTypeName");
            return View(property);
        }

        [HttpPost]
        public ActionResult PropertyVariantEdit(PropertyVariant property)
        {
            if (ModelState.IsValid)
            {
                db.Entry(property).State = EntityState.Modified;
                db.SaveChanges();
                TempData["datachange"] = "Property Variant is Successfully Updated.";
                return RedirectToAction("PropertyVariantList");
            }
            else
            {
                ViewBag.LPropertyType = new SelectList(db.PropertyTypes
                .Where(x => x.Status == "Active")
                .AsNoTracking()
                .ToList(), "Id", "PropertyTypeName");
                TempData["datachange"] = "Property Variant is Not Updated.";
            }
            return View(property);
        }

        public ActionResult PropertyVariantDelete(int id)
        {
            PropertyVariant type = db.PropertyVariants.Find(id);
            if (type != null)
            {
                db.PropertyVariants.Remove(type);
                db.SaveChanges();
                TempData["datachange"] = "PropertyVariant Delete.";
            }
            else
            {
                TempData["datachange"] = "Data Not Delete.";
            }
            return RedirectToAction("PropertyVariantList");
        }


        // BHK Type Crud Operations
        public ActionResult BHKTypeList()
        {
            List<BHKType> properties = db.BHKTypes.ToList();
            return View(properties);
        }


        public ActionResult BHKTypeCreate()
        {
            return View();
        }

        [HttpPost]
        public ActionResult BHKTypeCreate(BHKType property)
        {
            if (ModelState.IsValid)
            {
                db.BHKTypes.Add(property);
                db.SaveChanges();
                TempData["datachange"] = "BHK Type is Successfully Saved.";
                return RedirectToAction("BHKTypeList");
            }
            else
            {
                TempData["datachange"] = "BHK Type is Not Saved.";
            }
            return View(property);
        }

        public ActionResult BHKTypeEdit(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }
            BHKType property = db.BHKTypes.Find(id);
            if (property == null)
            {
                return Content("Nothing Found");
            }
            return View(property);
        }

        [HttpPost]
        public ActionResult BHKTypeEdit(BHKType property)
        {
            if (ModelState.IsValid)
            {
                db.Entry(property).State = EntityState.Modified;
                db.SaveChanges();
                TempData["datachange"] = "BHK Type is Successfully Updated.";
                return RedirectToAction("BHKTypeList");
            }
            else
            {
                TempData["datachange"] = "BHK Type is Not Updated.";
            }
            return View(property);
        }

        public ActionResult BHKTypeDelete(int id)
        {
            BHKType type = db.BHKTypes.Find(id);
            if (type != null)
            {
                db.BHKTypes.Remove(type);
                db.SaveChanges();
                TempData["datachange"] = "BHKType Delete.";
            }
            else
            {
                TempData["datachange"] = "Data Not Delete.";
            }
            return RedirectToAction("BHKTypeList");
        }




        // Parking Type Crud Operations
        public ActionResult ParkingTypeList()
        {
            List<ParkingType> properties = db.ParkingTypes.ToList();
            return View(properties);
        }


        public ActionResult ParkingTypeCreate()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ParkingTypeCreate(ParkingType property)
        {
            if (ModelState.IsValid)
            {
                db.ParkingTypes.Add(property);
                db.SaveChanges();
                TempData["datachange"] = "Parking Type is Successfully Saved.";
                return RedirectToAction("ParkingTypeList");
            }
            else
            {
                TempData["datachange"] = "Parking Type is Not Saved.";
            }
            return View(property);
        }

        public ActionResult ParkingTypeEdit(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }
            ParkingType property = db.ParkingTypes.Find(id);
            if (property == null)
            {
                return Content("Nothing Found");
            }
            return View(property);
        }

        [HttpPost]
        public ActionResult ParkingTypeEdit(ParkingType property)
        {
            if (ModelState.IsValid)
            {
                db.Entry(property).State = EntityState.Modified;
                db.SaveChanges();
                TempData["datachange"] = "Parking Type is Successfully Updated.";
                return RedirectToAction("ParkingTypeList");
            }
            else
            {
                TempData["datachange"] = "Parking Type is Not Updated.";
            }
            return View(property);
        }

        public ActionResult ParkingTypeDelete(int id)
        {
            ParkingType type = db.ParkingTypes.Find(id);
            if (type != null)
            {
                db.ParkingTypes.Remove(type);
                db.SaveChanges();
                TempData["datachange"] = "ParkingType Delete.";
            }
            else
            {
                TempData["datachange"] = "Data Not Delete.";
            }
            return RedirectToAction("ParkingTypeList");
        }


        // Floor Type Crud Operations
        public ActionResult FloorTypeList()
        {
            List<FloorType> properties = db.FloorTypes.ToList();
            return View(properties);
        }


        public ActionResult FloorTypeCreate()
        {
            return View();
        }

        [HttpPost]
        public ActionResult FloorTypeCreate(FloorType property)
        {
            if (ModelState.IsValid)
            {
                db.FloorTypes.Add(property);
                db.SaveChanges();
                TempData["datachange"] = "Floor Type is Successfully Saved.";
                return RedirectToAction("FloorTypeList");
            }
            else
            {
                TempData["datachange"] = "Floor Type is Not Saved.";
            }
            return View(property);
        }

        public ActionResult FloorTypeEdit(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }
            FloorType property = db.FloorTypes.Find(id);
            if (property == null)
            {
                return Content("Nothing Found");
            }
            return View(property);
        }

        [HttpPost]
        public ActionResult FloorTypeEdit(FloorType property)
        {
            if (ModelState.IsValid)
            {
                db.Entry(property).State = EntityState.Modified;
                db.SaveChanges();
                TempData["datachange"] = "Floor Type is Successfully Updated.";
                return RedirectToAction("FloorTypeList");
            }
            else
            {
                TempData["datachange"] = "Floor Type is Not Updated.";
            }
            return View(property);
        }

        public ActionResult FloorTypeDelete(int id)
        {
            FloorType type = db.FloorTypes.Find(id);
            if (type != null)
            {
                db.FloorTypes.Remove(type);
                db.SaveChanges();
                TempData["datachange"] = "FloorType Delete.";
            }
            else
            {
                TempData["datachange"] = "Data Not Delete.";
            }
            return RedirectToAction("FloorTypeList");
        }

        // Crud For Furnished Item
        public ActionResult FurnishedItemList()
        {
            List<FurnishedType> furnishedItems = db.FurnishedTypes.ToList();
            return View(furnishedItems);
        }

        public ActionResult FurnishedItemCreate()
        {
            return View();
        }

        [HttpPost]
        public ActionResult FurnishedItemCreate(FurnishedType furnishedItem)
        {
            if (ModelState.IsValid)
            {
                db.FurnishedTypes.Add(furnishedItem);
                db.SaveChanges();
                TempData["datachange"] = "Furnished Item is Successfully Saved.";
                return RedirectToAction("FurnishedItemList");
            }
            else
            {
                TempData["datachange"] = "Furnished Item is Not Saved.";
            }
            return View(furnishedItem);
        }

        public ActionResult FurnishedItemEdit(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }
            FurnishedType furnishedItem = db.FurnishedTypes.Find(id);
            if (furnishedItem == null)
            {
                return HttpNotFound();
            }
            return View(furnishedItem);
        }

        [HttpPost]
        public ActionResult FurnishedItemEdit(FurnishedType furnishedItem)
        {
            if (ModelState.IsValid)
            {
                db.Entry(furnishedItem).State = EntityState.Modified;
                db.SaveChanges();
                TempData["datachange"] = "Furnished Item is Successfully Updated.";
                return RedirectToAction("FurnishedItemList");
            }
            else
            {
                TempData["datachange"] = "Furnished Item is Not Updated.";
            }
            return View(furnishedItem);
        }

        public ActionResult FurnishedItemDelete(int id)
        {
            FurnishedType furnishedItem = db.FurnishedTypes.Find(id);
            if (furnishedItem != null)
            {
                db.FurnishedTypes.Remove(furnishedItem);
                db.SaveChanges();
                TempData["datachange"] = "Furnished Item Deleted.";
            }
            else
            {
                TempData["datachange"] = "Data Not Deleted.";
            }
            return RedirectToAction("FurnishedItemList");
        }


        //stair crud
        public ActionResult StairList()
        {
            List<Stair> stairs = db.Stairs.ToList();
            return View(stairs);
        }

        public ActionResult StairCreate()
        {
            return View();
        }

        [HttpPost]
        public ActionResult StairCreate(Stair stair)
        {
            if (ModelState.IsValid)
            {
                db.Stairs.Add(stair);
                db.SaveChanges();
                TempData["datachange"] = "Stair is Successfully Saved.";
                return RedirectToAction("StairList");
            }
            else
            {
                TempData["datachange"] = "Stair is Not Saved.";
            }
            return View(stair);
        }

        public ActionResult StairEdit(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }
            Stair stair = db.Stairs.Find(id);
            if (stair == null)
            {
                return HttpNotFound();
            }
            return View(stair);
        }

        [HttpPost]
        public ActionResult StairEdit(Stair stair)
        {
            if (ModelState.IsValid)
            {
                db.Entry(stair).State = EntityState.Modified;
                db.SaveChanges();
                TempData["datachange"] = "Stair is Successfully Updated.";
                return RedirectToAction("StairList");
            }
            else
            {
                TempData["datachange"] = "Stair is Not Updated.";
            }
            return View(stair);
        }

        public ActionResult StairDelete(int id)
        {
            Stair stair = db.Stairs.Find(id);
            if (stair != null)
            {
                db.Stairs.Remove(stair);
                db.SaveChanges();
                TempData["datachange"] = "Stair Deleted.";
            }
            else
            {
                TempData["datachange"] = "Data Not Deleted.";
            }
            return RedirectToAction("StairList");
        }

        //Roof Crud
        public ActionResult RoofList()
        {
            List<Roof> roofs = db.Roofs.ToList();
            return View(roofs);
        }

        public ActionResult RoofCreate()
        {
            return View();
        }

        [HttpPost]
        public ActionResult RoofCreate(Roof roof)
        {
            if (ModelState.IsValid)
            {
                db.Roofs.Add(roof);
                db.SaveChanges();
                TempData["datachange"] = "Roof is Successfully Saved.";
                return RedirectToAction("RoofList");
            }
            else
            {
                TempData["datachange"] = "Roof is Not Saved.";
            }
            return View(roof);
        }

        public ActionResult RoofEdit(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }
            Roof roof = db.Roofs.Find(id);
            if (roof == null)
            {
                return HttpNotFound();
            }
            return View(roof);
        }

        [HttpPost]
        public ActionResult RoofEdit(Roof roof)
        {
            if (ModelState.IsValid)
            {
                db.Entry(roof).State = EntityState.Modified;
                db.SaveChanges();
                TempData["datachange"] = "Roof is Successfully Updated.";
                return RedirectToAction("RoofList");
            }
            else
            {
                TempData["datachange"] = "Roof is Not Updated.";
            }
            return View(roof);
        }

        public ActionResult RoofDelete(int id)
        {
            Roof roof = db.Roofs.Find(id);
            if (roof != null)
            {
                db.Roofs.Remove(roof);
                db.SaveChanges();
                TempData["datachange"] = "Roof Deleted.";
            }
            else
            {
                TempData["datachange"] = "Data Not Deleted.";
            }
            return RedirectToAction("RoofList");
        }

        //first Priority crud
        public ActionResult FirstPriorityList()
        {
            List<FirstPriority> firstPriorities = db.FirstPriorities.ToList();
            return View(firstPriorities);
        }

        public ActionResult FirstPriorityCreate()
        {
            return View();
        }

        [HttpPost]
        public ActionResult FirstPriorityCreate(FirstPriority firstPriority)
        {
            if (ModelState.IsValid)
            {
                db.FirstPriorities.Add(firstPriority);
                db.SaveChanges();
                TempData["datachange"] = "First Priority is Successfully Saved.";
                return RedirectToAction("FirstPriorityList");
            }
            else
            {
                TempData["datachange"] = "First Priority is Not Saved.";
            }
            return View(firstPriority);
        }

        public ActionResult FirstPriorityEdit(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }
            FirstPriority firstPriority = db.FirstPriorities.Find(id);
            if (firstPriority == null)
            {
                return HttpNotFound();
            }
            return View(firstPriority);
        }

        [HttpPost]
        public ActionResult FirstPriorityEdit(FirstPriority firstPriority)
        {
            if (ModelState.IsValid)
            {
                db.Entry(firstPriority).State = EntityState.Modified;
                db.SaveChanges();
                TempData["datachange"] = "First Priority is Successfully Updated.";
                return RedirectToAction("FirstPriorityList");
            }
            else
            {
                TempData["datachange"] = "First Priority is Not Updated.";
            }
            return View(firstPriority);
        }

        public ActionResult FirstPriorityDelete(int id)
        {
            FirstPriority firstPriority = db.FirstPriorities.Find(id);
            if (firstPriority != null)
            {
                db.FirstPriorities.Remove(firstPriority);
                db.SaveChanges();
                TempData["datachange"] = "First Priority Deleted.";
            }
            else
            {
                TempData["datachange"] = "Data Not Deleted.";
            }
            return RedirectToAction("FirstPriorityList");
        }

        //Religion Crud

        public ActionResult ReligionList()
        {
            List<Religion> religions = db.Religions.ToList();
            return View(religions);
        }

        public ActionResult ReligionCreate()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ReligionCreate(Religion religion)
        {
            if (ModelState.IsValid)
            {
                db.Religions.Add(religion);
                db.SaveChanges();
                TempData["datachange"] = "Religion is Successfully Saved.";
                return RedirectToAction("ReligionList");
            }
            else
            {
                TempData["datachange"] = "Religion is Not Saved.";
            }
            return View(religion);
        }

        public ActionResult ReligionEdit(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }
            Religion religion = db.Religions.Find(id);
            if (religion == null)
            {
                return HttpNotFound();
            }
            return View(religion);
        }

        [HttpPost]
        public ActionResult ReligionEdit(Religion religion)
        {
            if (ModelState.IsValid)
            {
                db.Entry(religion).State = EntityState.Modified;
                db.SaveChanges();
                TempData["datachange"] = "Religion is Successfully Updated.";
                return RedirectToAction("ReligionList");
            }
            else
            {
                TempData["datachange"] = "Religion is Not Updated.";
            }
            return View(religion);
        }

        public ActionResult ReligionDelete(int id)
        {
            Religion religion = db.Religions.Find(id);
            if (religion != null)
            {
                db.Religions.Remove(religion);
                db.SaveChanges();
                TempData["datachange"] = "Religion Deleted.";
            }
            else
            {
                TempData["datachange"] = "Data Not Deleted.";
            }
            return RedirectToAction("ReligionList");
        }

        //Cooking Items Crud

        public ActionResult CookingItemList()
        {
            List<CookingItem> cookingItems = db.CookingItems.ToList();
            return View(cookingItems);
        }

        public ActionResult CookingItemCreate()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CookingItemCreate(CookingItem cookingItem)
        {
            if (ModelState.IsValid)
            {
                db.CookingItems.Add(cookingItem);
                db.SaveChanges();
                TempData["datachange"] = "Cooking Item is Successfully Saved.";
                return RedirectToAction("CookingItemList");
            }
            else
            {
                TempData["datachange"] = "Cooking Item is Not Saved.";
            }
            return View(cookingItem);
        }

        public ActionResult CookingItemEdit(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }
            CookingItem cookingItem = db.CookingItems.Find(id);
            if (cookingItem == null)
            {
                return HttpNotFound();
            }
            return View(cookingItem);
        }

        [HttpPost]
        public ActionResult CookingItemEdit(CookingItem cookingItem)
        {
            if (ModelState.IsValid)
            {
                db.Entry(cookingItem).State = EntityState.Modified;
                db.SaveChanges();
                TempData["datachange"] = "Cooking Item is Successfully Updated.";
                return RedirectToAction("CookingItemList");
            }
            else
            {
                TempData["datachange"] = "Cooking Item is Not Updated.";
            }
            return View(cookingItem);
        }

        public ActionResult CookingItemDelete(int id)
        {
            CookingItem cookingItem = db.CookingItems.Find(id);
            if (cookingItem != null)
            {
                db.CookingItems.Remove(cookingItem);
                db.SaveChanges();
                TempData["datachange"] = "Cooking Item Deleted.";
            }
            else
            {
                TempData["datachange"] = "Data Not Deleted.";
            }
            return RedirectToAction("CookingItemList");
        }

        //Water Supply Crud
        public ActionResult WaterSupplyList()
        {
            List<WaterSupply> waterSupplies = db.WaterSupplies.ToList();
            return View(waterSupplies);
        }

        public ActionResult WaterSupplyCreate()
        {
            return View();
        }

        [HttpPost]
        public ActionResult WaterSupplyCreate(WaterSupply waterSupply)
        {
            if (ModelState.IsValid)
            {
                db.WaterSupplies.Add(waterSupply);
                db.SaveChanges();
                TempData["datachange"] = "Water Supply is Successfully Saved.";
                return RedirectToAction("WaterSupplyList");
            }
            else
            {
                TempData["datachange"] = "Water Supply is Not Saved.";
            }
            return View(waterSupply);
        }

        public ActionResult WaterSupplyEdit(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }
            WaterSupply waterSupply = db.WaterSupplies.Find(id);
            if (waterSupply == null)
            {
                return HttpNotFound();
            }
            return View(waterSupply);
        }

        [HttpPost]
        public ActionResult WaterSupplyEdit(WaterSupply waterSupply)
        {
            if (ModelState.IsValid)
            {
                db.Entry(waterSupply).State = EntityState.Modified;
                db.SaveChanges();
                TempData["datachange"] = "Water Supply is Successfully Updated.";
                return RedirectToAction("WaterSupplyList");
            }
            else
            {
                TempData["datachange"] = "Water Supply is Not Updated.";
            }
            return View(waterSupply);
        }

        public ActionResult WaterSupplyDelete(int id)
        {
            WaterSupply waterSupply = db.WaterSupplies.Find(id);
            if (waterSupply != null)
            {
                db.WaterSupplies.Remove(waterSupply);
                db.SaveChanges();
                TempData["datachange"] = "Water Supply Deleted.";
            }
            else
            {
                TempData["datachange"] = "Data Not Deleted.";
            }
            return RedirectToAction("WaterSupplyList");
        }

        //Lpgs Crud
        public ActionResult LpgList()
        {
            List<Lpg> lpgs = db.Lpgs.ToList();
            return View(lpgs);
        }

        public ActionResult LpgCreate()
        {
            return View();
        }

        [HttpPost]
        public ActionResult LpgCreate(Lpg lpg)
        {
            if (ModelState.IsValid)
            {
                db.Lpgs.Add(lpg);
                db.SaveChanges();
                TempData["datachange"] = "LPG is Successfully Saved.";
                return RedirectToAction("LpgList");
            }
            else
            {
                TempData["datachange"] = "LPG is Not Saved.";
            }
            return View(lpg);
        }

        public ActionResult LpgEdit(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }
            Lpg lpg = db.Lpgs.Find(id);
            if (lpg == null)
            {
                return HttpNotFound();
            }
            return View(lpg);
        }

        [HttpPost]
        public ActionResult LpgEdit(Lpg lpg)
        {
            if (ModelState.IsValid)
            {
                db.Entry(lpg).State = EntityState.Modified;
                db.SaveChanges();
                TempData["datachange"] = "LPG is Successfully Updated.";
                return RedirectToAction("LpgList");
            }
            else
            {
                TempData["datachange"] = "LPG is Not Updated.";
            }
            return View(lpg);
        }

        public ActionResult LpgDelete(int id)
        {
            Lpg lpg = db.Lpgs.Find(id);
            if (lpg != null)
            {
                db.Lpgs.Remove(lpg);
                db.SaveChanges();
                TempData["datachange"] = "LPG Deleted.";
            }
            else
            {
                TempData["datachange"] = "Data Not Deleted.";
            }
            return RedirectToAction("LpgList");
        }

        //Electricy Crud

        public ActionResult ElectricityList()
        {
            List<Electricity> electricities = db.Electricities.ToList();
            return View(electricities);
        }

        public ActionResult ElectricityCreate()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ElectricityCreate(Electricity electricity)
        {
            if (ModelState.IsValid)
            {
                db.Electricities.Add(electricity);
                db.SaveChanges();
                TempData["datachange"] = "Electricity is Successfully Saved.";
                return RedirectToAction("ElectricityList");
            }
            else
            {
                TempData["datachange"] = "Electricity is Not Saved.";
            }
            return View(electricity);
        }

        public ActionResult ElectricityEdit(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }
            Electricity electricity = db.Electricities.Find(id);
            if (electricity == null)
            {
                return HttpNotFound();
            }
            return View(electricity);
        }

        [HttpPost]
        public ActionResult ElectricityEdit(Electricity electricity)
        {
            if (ModelState.IsValid)
            {
                db.Entry(electricity).State = EntityState.Modified;
                db.SaveChanges();
                TempData["datachange"] = "Electricity is Successfully Updated.";
                return RedirectToAction("ElectricityList");
            }
            else
            {
                TempData["datachange"] = "Electricity is Not Updated.";
            }
            return View(electricity);
        }

        public ActionResult ElectricityDelete(int id)
        {
            Electricity electricity = db.Electricities.Find(id);
            if (electricity != null)
            {
                db.Electricities.Remove(electricity);
                db.SaveChanges();
                TempData["datachange"] = "Electricity Deleted.";
            }
            else
            {
                TempData["datachange"] = "Data Not Deleted.";
            }
            return RedirectToAction("ElectricityList");
        }

        // Parking Visitor Type Crud Operations
        public ActionResult ParkingVisitorList()
        {
            List<ParkingVisitor> parkingVisitors = db.ParkingVisitors.ToList();
            return View(parkingVisitors);
        }


        public ActionResult ParkingVisitorCreate()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ParkingVisitorCreate(ParkingVisitor parking)
        {
            if (ModelState.IsValid)
            {
                db.ParkingVisitors.Add(parking);
                db.SaveChanges();
                TempData["datachange"] = "ParkingVisitor is Successfully Saved.";
                return RedirectToAction("ParkingVisitorList");
            }
            else
            {
                TempData["datachange"] = "ParkingVisitor is Not Saved.";
            }
            return View(parking);
        }

        public ActionResult ParkingVisitorEdit(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }
            ParkingVisitor parking = db.ParkingVisitors.Find(id);
            if (parking == null)
            {
                return Content("Nothing Found");
            }
            return View(parking);
        }

        [HttpPost]
        public ActionResult ParkingVisitorEdit(ParkingVisitor parking)
        {
            if (ModelState.IsValid)
            {
                db.Entry(parking).State = EntityState.Modified;
                db.SaveChanges();
                TempData["datachange"] = "ParkingVisitor is Successfully Updated.";
                return RedirectToAction("ParkingVisitorList");
            }
            else
            {
                TempData["datachange"] = "ParkingVisitor is Not Updated.";
            }
            return View(parking);
        }

        public ActionResult ParkingVisitorDelete(int id)
        {
            ParkingVisitor type = db.ParkingVisitors.Find(id);
            if (type != null)
            {
                db.ParkingVisitors.Remove(type);
                db.SaveChanges();
                TempData["datachange"] = "ParkingVisitor Delete.";
            }
            else
            {
                TempData["datachange"] = "Data Not Delete.";
            }
            return RedirectToAction("ParkingVisitorList");
        }


        // Toilet Type Crud Operations
        public ActionResult ToiletTypeList()
        {
            List<ToiletType> toiletTypes = db.ToiletTypes.ToList();
            return View(toiletTypes);
        }


        public ActionResult ToiletTypeCreate()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ToiletTypeCreate(ToiletType toilet)
        {
            if (ModelState.IsValid)
            {
                db.ToiletTypes.Add(toilet);
                db.SaveChanges();
                TempData["datachange"] = "ToiletType is Successfully Saved.";
                return RedirectToAction("ToiletTypeList");
            }
            else
            {
                TempData["datachange"] = "ToiletType is Not Saved.";
            }
            return View(toilet);
        }

        public ActionResult ToiletTypeEdit(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }
            ToiletType toilet = db.ToiletTypes.Find(id);
            if (toilet == null)
            {
                return Content("Nothing Found");
            }
            return View(toilet);
        }

        [HttpPost]
        public ActionResult ToiletTypeEdit(ToiletType toilet)
        {
            if (ModelState.IsValid)
            {
                db.Entry(toilet).State = EntityState.Modified;
                db.SaveChanges();
                TempData["datachange"] = "ToiletType is Successfully Updated.";
                return RedirectToAction("ToiletTypeList");
            }
            else
            {
                TempData["datachange"] = "ToiletType is Not Updated.";
            }
            return View(toilet);
        }

        public ActionResult ToiletTypeDelete(int id)
        {
            ToiletType toilet = db.ToiletTypes.Find(id);
            if (toilet != null)
            {
                db.ToiletTypes.Remove(toilet);
                db.SaveChanges();
                TempData["datachange"] = "ToiletType Delete.";
            }
            else
            {
                TempData["datachange"] = "Data Not Delete.";
            }
            return RedirectToAction("ToiletTypeList");
        }


        // Elevator Type Crud Operations
        public ActionResult ElevatorTypeList()
        {
            List<ElevatorType> elevatorTypes = db.ElevatorTypes.ToList();
            return View(elevatorTypes);
        }


        public ActionResult ElevatorTypeCreate()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ElevatorTypeCreate(ElevatorType elevator)
        {
            if (ModelState.IsValid)
            {
                db.ElevatorTypes.Add(elevator);
                db.SaveChanges();
                TempData["datachange"] = "ElevatorType is Successfully Saved.";
                return RedirectToAction("ElevatorTypeList");
            }
            else
            {
                TempData["datachange"] = "ElevatorType is Not Saved.";
            }
            return View(elevator);
        }

        public ActionResult ElevatorTypeEdit(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }
            ElevatorType elevator = db.ElevatorTypes.Find(id);
            if (elevator == null)
            {
                return Content("Nothing Found");
            }
            return View(elevator);
        }

        [HttpPost]
        public ActionResult ElevatorTypeEdit(ElevatorType elevator)
        {
            if (ModelState.IsValid)
            {
                db.Entry(elevator).State = EntityState.Modified;
                db.SaveChanges();
                TempData["datachange"] = "ElevatorType is Successfully Updated.";
                return RedirectToAction("ElevatorTypeList");
            }
            else
            {
                TempData["datachange"] = "ElevatorType is Not Updated.";
            }
            return View(elevator);
        }

        public ActionResult ElevatorTypeDelete(int id)
        {
            ElevatorType elevator = db.ElevatorTypes.Find(id);
            if (elevator != null)
            {
                db.ElevatorTypes.Remove(elevator);
                db.SaveChanges();
                TempData["datachange"] = "ElevatorType Delete.";
            }
            else
            {
                TempData["datachange"] = "Data Not Delete.";
            }
            return RedirectToAction("ElevatorTypeList");
        }
    }
}
