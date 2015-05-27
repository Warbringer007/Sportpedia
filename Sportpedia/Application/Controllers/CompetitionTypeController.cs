using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Application.Services;
using EFDatabase;
using Newtonsoft.Json;

namespace Application.Controllers
{
    public class CompetitionTypeController : Controller
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
        public ActionResult Add(string greska)
        {
            if (Request.Cookies["user"] == null)
            {
                return RedirectToAction("Login", "User");
            }
            string username = Logged_username();
            Set_TempData(username);
            TempData["Greska"] = greska;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add(Type_of_competition model)
        {
            if (Request.Cookies["user"] == null)
            {
                return RedirectToAction("Login", "User");
            }
            string username = Logged_username();
            var dal = new CompetitionType_services();
            if (model.Type_name == null)
            {
                return RedirectToAction("Add", new { greska = "Unesite ime tipa natjecanja!" });
            }
            if (dal.Check_existing(model.Type_name) != null)
            {
                return RedirectToAction("Add", new { greska = "Event već postoji!" });
            }
            dal.New_event(model);
            return RedirectToAction("Add", "Competition");
        }
    }
}