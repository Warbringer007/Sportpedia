using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    public class PlayerController : Controller
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
            var dal = new Team_services();
            BindingList<char> sex = new BindingList<char> {'M', 'Ž'};
            BindingList<Sport> sportovi = dal.Sportovi();
            ViewBag.Sportovi = sportovi;
            ViewBag.Sex = sex;
            TempData["Greska"] = greska;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add(SinglePlayerModel model)
        {
            if (Request.Cookies["user"] == null)
            {
                return RedirectToAction("Login", "User");
            }
            string username = Logged_username();
            var dal = new Player_services();
            if (model.Player.Name == null || model.Player.Country == null || model.Image == null)
            {
                return RedirectToAction("Add", new { greska = "Popunite sve podatke o igraču!" });
            }
            if (dal.Check_existing(model.Player.Name) != null)
            {
                return RedirectToAction("Add", new { greska = "Igrač već postoji!" });
            }
            MemoryStream target = new MemoryStream();
            model.Image.InputStream.CopyTo(target);
            model.Player.Picture = target.ToArray();
            dal.New_Player(model, username);
            return RedirectToAction("Index", new { id = dal.Check_existing(model.Player.Name).ID });
        }

        public ActionResult Players()
        {
            if (Request.Cookies["user"] == null)
            {
                return RedirectToAction("Login", "User");
            }
            string username = Logged_username();
            Set_TempData(username);
            var dal = new Player_services();
            return View(dal.Player_list());
        }

        [HttpGet]
        public ActionResult Index(int id)
        {
            if (Request.Cookies["user"] == null)
            {
                return RedirectToAction("Login", "User");
            }
            string username = Logged_username();
            Set_TempData(username);
            var dal = new Player_services();
            SinglePlayerModel model = new SinglePlayerModel { Player = dal.Check_existing(id) };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(SinglePlayerModel model)
        {
            if (Request.Cookies["user"] == null)
            {
                return RedirectToAction("Login", "User");
            }
            string username = Logged_username();
            var dal = new Player_services();
            if (model.Comment != null) dal.Add_comment(model, username);
            return RedirectToAction("Index", new { teamname = model.Player.ID });
        }

        [HttpGet]
        public ActionResult Edit(int id, string greska = "")
        {
            if (Request.Cookies["user"] == null)
            {
                return RedirectToAction("Login", "User");
            }
            string username = Logged_username();
            Set_TempData(username);
            TempData["Greska"] = greska;
            var dal = new Player_services();
            SinglePlayerModel model = new SinglePlayerModel { Player = dal.Check_existing(id) };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(SinglePlayerModel model)
        {
            if (Request.Cookies["user"] == null)
            {
                return RedirectToAction("Login", "User");
            }
            string username = Logged_username();
            var dal = new Player_services();
            if (model.Player.Name == null || model.Player.Country == null)
            {
                return RedirectToAction("Edit", new { greska = "Popunite sve podatke o igraču!", id = model.Player.ID });
            }
            MemoryStream target = new MemoryStream();
            if (model.Image != null)
            {
                model.Image.InputStream.CopyTo(target);
                model.Player.Picture = target.ToArray();
            }
            dal.Edit_Player(model);
            return RedirectToAction("Index", new { id = model.Player.ID });
        }
    }
}