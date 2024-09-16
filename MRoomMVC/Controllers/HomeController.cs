using System;
using System.Web.Mvc;
using System.Data.Entity;
using System.Collections.Generic;
using System.Linq;
using MRoomMVC.Data;
using MRoomMVC.Models;
using System.Web.Security;
using MRoomMVC.ViewModels;
using System.Web;

namespace MRoomMVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly MRoomDbContext db = new MRoomDbContext();

        public ActionResult Index()
        {
            ViewBag.Slider = db.Sliders.AsNoTracking().OrderByDescending(x => x.Id).Take(8).ToList();
            ViewBag.Testimonial = db.Testimonials.AsNoTracking().OrderByDescending(x => x.Id).Take(8).ToList();
            ViewBag.CommercialShop = db.PropertyDetails.AsNoTracking().Where(x => x.PropertyVariantName == "SHOP" && x.IsActive).Take(8).ToList();
            ViewBag.BHK1 = db.PropertyDetails.AsNoTracking().Where(x => x.BHKTypeName == "1 BHK" && x.IsActive).Take(8).ToList();
            ViewBag.BHK2 = db.PropertyDetails.AsNoTracking().Where(x => x.BHKTypeName == "2 BHK" && x.IsActive).Take(8).ToList();
            ViewBag.BHK3 = db.PropertyDetails.AsNoTracking().Where(x => x.BHKTypeName == "3 BHK" && x.IsActive).Take(8).ToList();
            ViewBag.BHKType = new SelectList(db.BHKTypes.AsNoTracking().OrderBy(x => x.BHKName), "BHKName", "BHKName"); ;
            ViewBag.City = new SelectList(db.CityMasters.AsNoTracking().OrderBy(x => x.Name), "Name", "Name");
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
            IQueryable<PropertyDetail> query = db.PropertyDetails.AsNoTracking().Where(x => x.IsActive);
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
            ViewBag.City = new SelectList(db.CityMasters.AsNoTracking().OrderBy(x => x.Name), "Name", "Name");
            //ViewBag.Colony = new SelectList(db.ColonyMuhallas.AsNoTracking(), "ColonyName", "ColonyName");
            ViewBag.Floor = new SelectList(db.FloorTypes.AsNoTracking().OrderBy(x => x.FloorTypeName), "FloorTypeName", "FloorTypeName");
            return View(result);
        }

        public ActionResult PropertyListFilter(string Colony, string City, string Budget, string Floor)
        {
            IQueryable<PropertyDetail> query = db.PropertyDetails.AsNoTracking().Where(x => x.IsActive); ;
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
            ViewBag.City = new SelectList(db.CityMasters.AsNoTracking().OrderBy(x => x.Name), "Name", "Name");
            //ViewBag.Colony = new SelectList(db.ColonyMuhallas.AsNoTracking(), "ColonyName", "ColonyName");
            ViewBag.Floor = new SelectList(db.FloorTypes.AsNoTracking().OrderBy(x => x.FloorTypeName), "FloorTypeName", "FloorTypeName");
            return View("PropertyList", result);
        }

        public ActionResult PropertySearch(string BHKType, string City, string NearBy)
        {
            IQueryable<PropertyDetail> query = db.PropertyDetails.AsNoTracking().Where(x => x.IsActive); ;
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
            ViewBag.City = new SelectList(db.CityMasters.AsNoTracking().OrderBy(x => x.Name), "Name", "Name");
            //ViewBag.Colony = new SelectList(db.ColonyMuhallas.AsNoTracking(), "ColonyName", "ColonyName");
            ViewBag.Floor = new SelectList(db.FloorTypes.AsNoTracking().OrderBy(x => x.FloorTypeName), "FloorTypeName", "FloorTypeName");
            return View("PropertyList", result);
        }

        public ActionResult PropertyDetails(string Pid = "")
        {
            PropertyDetail detail = db.PropertyDetails.FirstOrDefault(x => x.PropertyId == Pid);
            var nearsid = db.PD_Near.Where(x => x.PropertyId == detail.Id).Select(x => x.NearById).ToList();
            List<string> nears = db.NearBies.Where(x => nearsid.Contains(x.Id)).Select(x => x.NearByName).ToList();
            ViewBag.Nears = string.Join(", ", nears);
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

        public ActionResult Login(string Mobile = "")
        {
            UserLogin login = new UserLogin();
            if (!string.IsNullOrEmpty(Mobile))
            {
                login.Username = Mobile;
            }
            return View(login);
        }

        [HttpPost]
        public ActionResult Login(UserLogin log, string returnUrl)
        {
            UserLogin user1 = db.UserLogins.Where(x => x.Username == log.Username && x.Password == log.Password).FirstOrDefault();

            if (user1 != null)
            {
                var authTicket = new FormsAuthenticationTicket(1, user1.Username, db.ConvertUtcToIst(), db.ConvertUtcToIst().AddMinutes(20), user1.IsRemember, user1.Role);
                string encryptedTicket = FormsAuthentication.Encrypt(authTicket);
                HttpCookie authCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);
                authCookie.Expires = db.ConvertUtcToIst().AddMinutes(20);
                Response.Cookies.Add(authCookie);
                if (!String.IsNullOrEmpty(returnUrl))
                {
                    return Redirect(returnUrl);
                }
                if (user1.Role == "Admin")
                {
                    return Redirect("/Admin");
                }
                else if (user1.Role == "Rental")
                {
                    return Redirect("/Rental");
                }
                else if (user1.Role == "LandLords")
                {
                    return Redirect("/LandLords");
                }
            }

            TempData["datachange"] = "Invalid Username or Password.";
            return View(log);
        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return Redirect("/Home/Login");
        }

        public ActionResult UserRegistration(string Mobile = "")
        {
            UserDetailsView login = new UserDetailsView();
            if (!string.IsNullOrEmpty(Mobile))
            {
                login.Mobile = Mobile;
            }
            return View(login);
        }

        [HttpPost]
        public ActionResult UserRegistration(UserDetailsView detailsView)
        {
            bool emailexist = db.UserDetails.Any(x => x.Email == detailsView.Email || x.Mobile == detailsView.Mobile);
            bool unameexist = db.UserLogins.Any(x => x.Username == detailsView.Mobile);
            if (emailexist)
            {
                TempData["datachange"] = "Email/Mobile No. is alraedy Exist";
                return View(detailsView);
            }

            if (unameexist)
            {
                TempData["datachange"] = "Mobile No. is alraedy Exist";
                return View(detailsView);
            }
            if (ModelState.IsValid)
            {
                UserDetails user = new UserDetails
                {
                    Name = detailsView.Name,
                    Dob = detailsView.Dob,
                    Email = detailsView.Email,
                    Mobile = detailsView.Mobile,
                    Address = detailsView.Address,
                    IsActive = true,
                    CreatedDate = DateTime.Today
                };

                UserLogin login = new UserLogin
                {
                    Username = detailsView.Mobile,
                    Password = detailsView.Password,
                    Role = detailsView.Role
                };
                db.UserLogins.Add(login);
                db.SaveChanges();
                user.LoginId = login.Id;
                db.UserDetails.Add(user);
                db.SaveChanges();
                TempData["datachange"] = "Regisration is sucessfully Added plz login with mobile no and password.";
                return RedirectToAction("Login");
            }
            TempData["datachange"] = "Invalid Data";
            return View(detailsView);
        }

        public ActionResult ForgotPassword()
        {
            return View();
        }

        public ActionResult LoginCheckPage()
        {
            return View();
        }

        public ActionResult LoginCheck(string Mobile)
        {
            bool existsuser = db.UserDetails.Any(x => x.Mobile == Mobile);
            if (existsuser)
            {
                return RedirectToAction("Login", new { Mobile = Mobile });
            }
            else
            {
                return RedirectToAction("UserRegistration", new { Mobile = Mobile });
            }
        }

        [HttpPost]
        public ActionResult ForgotPassword(string Mobile, DateTime Dob)
        {
            UserDetails userde = db.UserDetails.Where(x => x.Mobile == Mobile && x.Dob == Dob).FirstOrDefault();
            if (userde == null)
            {
                TempData["datachange"] = "Incorrect mobileno or dob. Plz Check Again";
                return View();
            }
            UserLogin user = db.UserLogins.Find(userde.LoginId);
            ViewBag.LoginId = user.Id;
            return View("ResetPassword");
        }

        [HttpPost]
        public ActionResult ResetPassword(int LoginId, string Password)
        {
            UserLogin user = db.UserLogins.Find(LoginId);
            if (!string.IsNullOrEmpty(Password))
            {
                user.Password = Password;
            }
            db.Entry(user).State = EntityState.Modified;
            db.SaveChanges();
            TempData["datachange"] = "Your Password Is suceesfully Changed.";
            return RedirectToAction("Login");
        }
    }
}
