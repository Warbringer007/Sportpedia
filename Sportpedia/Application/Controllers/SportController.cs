using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Application.Models;
using Application.Services;
using EFDatabase;
using System.ComponentModel;

namespace Application.Controllers
{
    public class SportController : Controller
    {
        public string Logged_username()
        {
            return JsonConvert.DeserializeObject<Dictionary<string, dynamic>>(Request.Cookies["user"].Value)["Username"];
        }

        public string Set_TempData(string username)
        {
            TempData["Username"] = username;
            var dal = new User_services();
            User user = dal.Check_existing(username);
            TempData["Permission"] = user.Permissions.Name;
            return user.Permissions.Name;
        }

        [HttpGet]
        public ActionResult Add(string greska = "")
        {
            if (Request.Cookies["user"] == null)
            {
                return RedirectToAction("Login", "User");
            }
            string username = Logged_username();
            if (Set_TempData(username) != "admin")
            {
                return RedirectToAction("Racun", "User");
            }
            var dal = new Sport_services();
            var eventdal = new Event_services();
            SingleSportModel model = new SingleSportModel();
            List<All_Events> events = eventdal.Event_list().Select(one => new All_Events
            {
                Event = one.Name, Checked = false
            }).ToList();
            model.Eventi = events;
            BindingList<Sport_type> vrste_sportova = dal.Sportski_tipovi();
            ViewBag.Tipovi = vrste_sportova;
            TempData["Greska"] = greska;
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add(SingleSportModel model)
        {
            if (Request.Cookies["user"] == null)
            {
                return RedirectToAction("Login", "User");
            }
            string username = Logged_username();
            if (Set_TempData(username) != "admin")
            {
                return RedirectToAction("Racun", "User");
            }
            var dal = new Sport_services();
            if (model.Sport.Name == null)
            {
                return RedirectToAction("Add", new { greska = "Unesite ime sporta!" });
            }
            if (dal.Check_existing(model.Sport.Name) != null)
            {
                return RedirectToAction("Add", new { greska = "Sport već postoji!" });
            }
            dal.New_sport(model,Logged_username());
            return RedirectToAction("Index", new { sportname = model.Sport.Name});
        }

        public ActionResult Index(string sportname)
        {
            string username = Logged_username();
            Set_TempData(username);
            var dal = new Sport_services();
            return View(dal.Sport_information(sportname));
        }

        public ActionResult Sports()
        {
            if (Request.Cookies["user"] == null)
            {
                return RedirectToAction("Login", "User");
            }
            string username = Logged_username();
            Set_TempData(username);
            var dal = new Sport_services();
            SportListView view = new SportListView
            {
                All_sports = dal.Sport_list()
            };
            return View(view);
        }
	}
}