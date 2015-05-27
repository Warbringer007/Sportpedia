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
    public class CompetitionController : Controller
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
        public ActionResult Index(int id)
        {
            if (Request.Cookies["user"] == null)
            {
                return RedirectToAction("Login", "User");
            }
            string username = Logged_username();
            Set_TempData(username);
            var dal = new Competition_services();
            var dal2 = new Team_services();
            var dal3 = new Player_services();
            CompetitionViewModel model = new CompetitionViewModel();
            model.Competition = dal.Check_existing(id);
            if (model.Competition.Sport.Sport_type.Type_description.Equals("1v1"))
            {
                List<Player> svi = new List<Player>();
                foreach (var one in model.Competition.Competition_contestants)
                {
                    svi.Add(dal3.Check_existing(one.Contestant.ID));
                }
                model.Players = svi;
            }
            else
            {
                List<Team> svi = new List<Team>();
                foreach (var one in model.Competition.Competition_contestants)
                {
                    svi.Add(dal2.Check_existing(one.Contestant.ID));
                }
                model.Teams = svi;
            }
            model.Rounds = (model.Competition.Competition_contestants.Count() - 1)*2;
            if (!model.Competition.Type_of_competition.DoubleQuadra) model.Rounds *= 2;
            return View(model);
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
            var dal = new Player_services();
            var dal2 = new Team_services();
            var dal3 = new CompetitionType_services();
            BindingList<Type_of_competition> competitionType = dal3.Competition_type_list();
            ViewBag.Types = competitionType;
            BindingList<Sport> sportovi = dal2.Sportovi();
            ViewBag.Sportovi = sportovi;
            TempData["Greska"] = greska;
            SingleCompetitionModel model = new SingleCompetitionModel();
            List<SelectListItem> svi = new List<SelectListItem>();
            foreach (var one in dal2.Team_list())
            {
                SelectListItem jedan = new SelectListItem();
                jedan.Text = one.Name;
                svi.Add(jedan);
            }
            foreach (var one in dal.Player_list())
            {
                SelectListItem jedan = new SelectListItem();
                jedan.Text = one.Name;
                svi.Add(jedan);
            }
            model.Sports = svi;
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add(SingleCompetitionModel model)
        {
            if (Request.Cookies["user"] == null)
            {
                return RedirectToAction("Login", "User");
            }
            string username = Logged_username();
            var dal = new Competition_services();
            if (model.Competition.Name == null || model.Competition.Country == null || model.Image == null)
            {
                return RedirectToAction("Add", new { greska = "Popunite sve podatke o natjecanju!" });
            }
            if (dal.Check_existing(model.Competition.Name) != null)
            {
                return RedirectToAction("Add", new { greska = "Igrač već postoji!" });
            }
            if (model.Competitors == null)
            {
                return RedirectToAction("Add", new { greska = "Dodajte natjecatelje!" });
            }
            if (model.Competitors.Count % 2 != 0)
            {
                return RedirectToAction("Add", new { greska = "Broj natjecatelja mora biti paran!" });
            }
            MemoryStream target = new MemoryStream();
            model.Image.InputStream.CopyTo(target);
            model.Competition.Emblem = target.ToArray();
            dal.New_competition(model, username);
            return View();
        }

        public ActionResult Competitions()
        {
            if (Request.Cookies["user"] == null)
            {
                return RedirectToAction("Login", "User");
            }
            string username = Logged_username();
            Set_TempData(username);
            var dal = new Competition_services();
            return View(dal.Competition_list());
        }

        public ActionResult Round(int competition, int kolo)
        {
            if (Request.Cookies["user"] == null)
            {
                return RedirectToAction("Login", "User");
            }
            string username = Logged_username();
            var dal = new Competition_services();
            var dal2 = new Team_services();
            var dal3 = new Player_services();
            RoundViewModel model = new RoundViewModel();
            Competition current = dal.Check_existing(competition);
            List<Match> matches = dal.RoundMatches(current, kolo);
            model.matches = new List<SingleMatchModel>();
            foreach (var one in matches)
            {
                SingleMatchModel match = new SingleMatchModel();
                match.HomePoints = dal.HomePoints(one);
                match.AwayPoints = dal.AwayPoints(one);
                match.Match = one;
                if (current.Sport.Sport_type.Type_name.Equals("1v1"))
                {
                    match.HomeContestant = dal3.Check_existing(one.Match_contestants.ElementAt(0).Contestant.ID).Name;
                    match.AwayContestant = dal3.Check_existing(one.Match_contestants.ElementAt(1).Contestant.ID).Name;
                }
                else
                {
                    match.HomeContestant = dal2.Check_existing(one.Match_contestants.ElementAt(0).Contestant.ID).Name;
                    match.AwayContestant = dal2.Check_existing(one.Match_contestants.ElementAt(1).Contestant.ID).Name;
                }
                model.matches.Add(match);
            }
            model.Round = kolo;
            Set_TempData(username);
            return View(model);
        }

        [HttpGet]
        public ActionResult AddMatch(int id)
        {
            if (Request.Cookies["user"] == null)
            {
                return RedirectToAction("Login", "User");
            }
            string username = Logged_username();
            Set_TempData(username);
            var dal = new Competition_services();
            var dal2 = new Team_services();
            var dal3 = new Player_services();
            var dal4 = new Event_services();
            SingleMatchModel match = new SingleMatchModel();
            match.HomePoints = 0;
            match.AwayPoints = 0;
            match.Match = dal.One_match(id);
            if (dal3.Check_existing(match.Match.Match_contestants.ElementAt(0).Contestant.ID) != null)
            {
                match.HomeContestant = dal3.Check_existing(match.Match.Match_contestants.ElementAt(0).Contestant.ID).Name;
                match.AwayContestant = dal3.Check_existing(match.Match.Match_contestants.ElementAt(1).Contestant.ID).Name;
            }
            else
            {
                match.HomeContestant = dal2.Check_existing(match.Match.Match_contestants.ElementAt(0).Contestant.ID).Name;
                match.AwayContestant = dal2.Check_existing(match.Match.Match_contestants.ElementAt(1).Contestant.ID).Name;
            }
            List<Event_list> allEvents= dal4.Event_list();

            match.Events = new List<EventModel>();
            foreach (var one in allEvents)
            {
                EventModel jedan = new EventModel();
                jedan.Name = one.Name;
                jedan.Counts = one.Counts;
                jedan.Length = one.Length;
                jedan.Player1 = one.Player1;
                jedan.Player2 = one.Player2;
                jedan.Points1 = one.Points1;
                jedan.Points2 = one.Points2;
                jedan.Primary = one.Primary;
                match.Events.Add(jedan);
            }
            match.TeamPlayers = new List<string>();
            match.TeamPlayers = dal2.TeamPlayers(match.HomeContestant, match.AwayContestant);
            
            return View(match);
        }

        [HttpPost]
        public ActionResult AddMatch()
        {
            if (Request.Cookies["user"] == null)
            {
                return RedirectToAction("Login", "User");
            }
            string username = Logged_username();
            var dal = new Competition_services();
            Stream req = Request.InputStream;
            req.Seek(0, System.IO.SeekOrigin.Begin);
            string json = new StreamReader(req).ReadToEnd();
            DeserializedModel deserialized = JsonConvert.DeserializeObject<DeserializedModel>(json);
            dal.ProcessMatch(deserialized, username);
            return RedirectToAction("Competitions");
        }
    }
}