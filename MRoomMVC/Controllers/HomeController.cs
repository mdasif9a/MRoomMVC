using System;
using System.Web.Mvc;
using System.Data.Entity;
using System.Collections.Generic;
using System.Linq;
using MRoomMVC.Data;
using MRoomMVC.Models;
using System.Web.Security;

namespace MRoomMVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly MRoomDbContext db = new MRoomDbContext();

        public ActionResult Index()
        {
            ViewBag.Slider = db.Sliders.AsNoTracking().OrderByDescending(x => x.Id).Take(8).ToList();
            ViewBag.Testimonial = db.Testimonials.AsNoTracking().OrderByDescending(x => x.Id).Take(8).ToList();
            ViewBag.CommercialShop = db.PropertyDetails.AsNoTracking().Where(x => x.PropertyVariantName == "SHOP").Take(8).ToList();
            ViewBag.BHK1 = db.PropertyDetails.AsNoTracking().Where(x => x.BHKTypeName == "1 BHK").Take(8).ToList();
            ViewBag.BHK2 = db.PropertyDetails.AsNoTracking().Where(x => x.BHKTypeName == "2 BHK").Take(8).ToList();
            ViewBag.BHK3 = db.PropertyDetails.AsNoTracking().Where(x => x.BHKTypeName == "3 BHK").Take(8).ToList();
            ViewBag.BHKType = new SelectList(db.BHKTypes.AsNoTracking(), "BHKName", "BHKName"); ;
            ViewBag.City = new SelectList(db.CityMasters.AsNoTracking(), "Name", "Name");
            return View();
        }

        public ActionResult About()
        {
            return View();
        }

        public ActionResult Privacy()
        {
            return View();
        }

        public ActionResult TermsOfUse()
        {
            return View();
        }

        public ActionResult Contact()
        {
            return View();
        }

        public ActionResult PropertyList(string Name = "")
        {
            IQueryable<PropertyDetail> query = db.PropertyDetails.AsNoTracking();
            if (Name == "CommercialShop")
            {
                query = query.Where(x => x.PropertyVariantName == "SHOP");
            }
            else if (Name == "BHK1")
            {
                query = query.Where(x => x.BHKTypeName == "1 BHK");
            }
            else if (Name == "BHK2")
            {
                query = query.Where(x => x.BHKTypeName == "2 BHK");
            }
            else if (Name == "BHK3")
            {
                query = query.Where(x => x.BHKTypeName == "3 BHK");
            }
            List<PropertyDetail> result = query.ToList();
            ViewBag.City = new SelectList(db.CityMasters.AsNoTracking(), "Name", "Name");
            //ViewBag.Colony = new SelectList(db.ColonyMuhallas.AsNoTracking(), "ColonyName", "ColonyName");
            ViewBag.Floor = new SelectList(db.FloorTypes.AsNoTracking(), "FloorTypeName", "FloorTypeName");
            return View(result);
        }

        public ActionResult PropertyListFilter(string Colony, string City, string Budget, string Floor)
        {
            IQueryable<PropertyDetail> query = db.PropertyDetails.AsNoTracking();
            if (!string.IsNullOrEmpty(Colony))
            {
                query = query.Where(x => x.ColonyName == Colony);
            }
            if (!string.IsNullOrEmpty(City))
            {
                query = query.Where(x => x.CityName == City);
            }
            var query2 = query.AsEnumerable();
            if (!string.IsNullOrEmpty(Budget))
            {
                int budgetval = Convert.ToInt32(Budget);
                query2 = query2.Where(x => Convert.ToInt32(x.MonthlyRent) >= budgetval);
            }
            if (!string.IsNullOrEmpty(Floor))
            {
                query2 = query2.Where(x => x.FloorTypeName == Floor);
            }
            List<PropertyDetail> result = query2.ToList();
            ViewBag.City = new SelectList(db.CityMasters.AsNoTracking(), "Name", "Name");
            //ViewBag.Colony = new SelectList(db.ColonyMuhallas.AsNoTracking(), "ColonyName", "ColonyName");
            ViewBag.Floor = new SelectList(db.FloorTypes.AsNoTracking(), "FloorTypeName", "FloorTypeName");
            return View("PropertyList", result);
        }

        public ActionResult PropertySearch(string BHKType, string City, string NearBy)
        {
            IQueryable<PropertyDetail> query = db.PropertyDetails.AsNoTracking();
            if (!string.IsNullOrEmpty(BHKType))
            {
                query = query.Where(x => x.BHKTypeName == BHKType);
            }
            if (!string.IsNullOrEmpty(City))
            {
                query = query.Where(x => x.CityName == City);
            }
            if (!string.IsNullOrEmpty(NearBy))
            {
                int nid = Convert.ToInt32(NearBy);
                var near = db.NearBies.FirstOrDefault(x => x.Id == nid);
                if (near != null)
                {
                    var d_Nears = db.PD_Near.Where(x => x.NearById == near.Id).Select(x => x.PropertyId);
                    query = query.Where(x => d_Nears.Contains(x.Id));
                }
            }

            List<PropertyDetail> result = query.ToList();
            ViewBag.City = new SelectList(db.CityMasters.AsNoTracking(), "Name", "Name");
            //ViewBag.Colony = new SelectList(db.ColonyMuhallas.AsNoTracking(), "ColonyName", "ColonyName");
            ViewBag.Floor = new SelectList(db.FloorTypes.AsNoTracking(), "FloorTypeName", "FloorTypeName");
            return View("PropertyList", result);
        }

        public ActionResult PropertyDetails(int Pid = 0)
        {
            PropertyDetail detail = db.PropertyDetails.Find(Pid);
            if (detail == null)
            {
                return HttpNotFound();
            }
            return View(detail);
        }

        public ActionResult Testimonials()
        {
            List<Testimonial> testimonials = db.Testimonials.AsNoTracking().ToList();
            return View(testimonials);
        }

        public ActionResult Tenats()
        {
            return View();
        }

        public ActionResult OwnerLandlords()
        {
            return View();
        }

        public ActionResult ListingWhatsapp()
        {
            return View();
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(UserLogin log, string returnUrl)
        {
            UserLogin user1 = db.UserLogins.Where(x => x.Username == log.Username && x.Password == log.Password).FirstOrDefault();
            if (user1 != null && user1.Role == "Admin")
            {
                FormsAuthentication.SetAuthCookie(user1.Username, user1.IsRemember);
                if (!String.IsNullOrEmpty(returnUrl))
                {
                    return Redirect(returnUrl);
                }
                return Redirect("/Admin");
            }
            TempData["datachange"] = "Invalid Username or Password.";
            return View(log);
        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return Redirect("/Home/Login");
        }
    }
}
