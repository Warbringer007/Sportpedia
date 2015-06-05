using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EFDatabase;
using Application.Services;
using System.ComponentModel;
using System.IO;
using Application.Models;

namespace Application.Controllers
{
    public class TeamController : Controller
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
            if (Set_TempData(username) == "user")
            {
                return RedirectToAction("Racun", "User");
            }
            var dal = new Team_services();
            BindingList<Sport> sportovi = dal.Sportovi();
            ViewBag.Sportovi = sportovi;
            BindingList<Stadium> stadioni = dal.Stadioni();
            ViewBag.Stadioni = stadioni;
            TempData["Greska"] = greska;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add(SingleTeamModel model)
        {
            if (Request.Cookies["user"] == null)
            {
                return RedirectToAction("Login", "User");
            }
            string username = Logged_username();
            if (Set_TempData(username) == "user")
            {
                return RedirectToAction("Racun", "User");
            }
            var dal = new Team_services();
            if (model.Team.Name == null || model.Team.Country == null || model.Image == null || model.Team.Fans_Name == null || /*model.Team.Stadium == null ||*/ model.Team.Webpage == null)
            {
                return RedirectToAction("Add", new { greska = "Popunite sve podatke o klubu!" });
            }
            if (dal.Check_existing(model.Team.Name) != null)
            {
                return RedirectToAction("Add", new { greska = "Klub već postoji!" });
            }
            MemoryStream target = new MemoryStream();
            model.Image.InputStream.CopyTo(target);
            model.Team.Emblem = target.ToArray();
            dal.New_Team(model, username);
            return RedirectToAction("Index", new { id = dal.Check_existing(model.Team.Name).ID });
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
            var dal = new Team_services();
            BindingList<Player> players = dal.Igraci(dal.Check_existing(id).Sport);
            ViewBag.Players = players;
            SingleTeamModel model = new SingleTeamModel {Team = dal.Check_existing(id)};
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(SingleTeamModel model)
        {
            if (Request.Cookies["user"] == null)
            {
                return RedirectToAction("Login", "User");
            }
            string username = Logged_username();
            var dal = new Team_services();
            if ( model.Comment != null ) dal.Add_comment(model, username);
            return RedirectToAction("Index", new { teamname = model.Team.ID });
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
            var dal = new Team_services();
            SingleTeamModel model = new SingleTeamModel { Team = dal.Check_existing(id) };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(SingleTeamModel model)
        {
            if (Request.Cookies["user"] == null)
            {
                return RedirectToAction("Login", "User");
            }
            string username = Logged_username();
            if (Set_TempData(username) == "user")
            {
                return RedirectToAction("Racun", "User");
            }
            var dal = new Team_services();
            if (model.Team.Name == null || model.Team.Country == null  || model.Team.Fans_Name == null || /*model.Team.Stadium == null ||*/ model.Team.Webpage == null)
            {
                return RedirectToAction("Edit", new { greska = "Popunite sve podatke o klubu!", id = model.Team.ID });
            }
            MemoryStream target = new MemoryStream();
            if (model.Image != null)
            {
                model.Image.InputStream.CopyTo(target);
                model.Team.Emblem = target.ToArray();
            }
            dal.Edit_Team(model);
            return RedirectToAction("Index", new { id = model.Team.ID });
        }

        [HttpGet]
        public ActionResult Teams()
        {
            if (Request.Cookies["user"] == null)
            {
                return RedirectToAction("Login", "User");
            }
            string username = Logged_username();
            Set_TempData(username);
            var dal = new Team_services();
            TeamListView view = new TeamListView
            {
                All_teams = dal.Team_list()
            };
            return View(view);
        }

        [HttpPost]
        public ActionResult NewPlayer(SingleTeamModel model)
        {
            if (Request.Cookies["user"] == null)
            {
                return RedirectToAction("Login", "User");
            }
            var dal = new Team_services();
            dal.Add_Player(model);
            return RedirectToAction("Index", new { id = model.Team.ID });
        }
    }
}