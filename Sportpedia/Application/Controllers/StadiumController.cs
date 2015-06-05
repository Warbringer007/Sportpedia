using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Application.Models;
using Application.Services;
using EFDatabase;
using Newtonsoft.Json;

namespace Application.Controllers
{
    public class StadiumController : Controller
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
            Set_TempData(username);
            TempData["Greska"] = greska;
            return View();
        }
        [HttpPost]
        public ActionResult Add(StadiumModel model)
        {
            if (Request.Cookies["user"] == null)
            {
                return RedirectToAction("Login", "User");
            }
            string username = Logged_username();
            var dal = new Stadium_services();
            MemoryStream target = new MemoryStream();
            model.Image.InputStream.CopyTo(target);
            model.Stadium.Picture = target.ToArray();
            dal.New_stadium(model.Stadium, username);
            return RedirectToAction("Add", "Team");
        }
        /*public ActionResult Index()
        {
            return View();
        }*/
    }
}