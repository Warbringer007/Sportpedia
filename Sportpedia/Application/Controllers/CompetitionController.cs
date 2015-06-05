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

        public ActionResult Cup(int competition)
        {
            if (Request.Cookies["user"] == null)
            {
                return RedirectToAction("Login", "User");
            }
            string username = Logged_username();
            Set_TempData(username);
            var dal = new Competition_services();
            var dal2 = new Team_services();
            Competition current = dal.Check_existing(competition);
            List<Match> Matches = dal.CompetitionMatches(current);
            CupViewModel model = new CupViewModel();
            model.Matches = new List<MatchViewModel>();
            foreach (var one in Matches)
            {
                MatchViewModel add = new MatchViewModel();
                add.Match = one;
                add.HomeContestant = "";
                add.AwayContestant = "";
                add.HomePoints = 0;
                add.AwayPoints = 0;
                if (one.Playing)
                {
                    add.HomeContestant = dal2.Check_existing(one.Match_contestants.ElementAt(0).Contestant.ID).Name;
                    add.AwayContestant = dal2.Check_existing(one.Match_contestants.ElementAt(1).Contestant.ID).Name;
                    if (one.Locked)
                    {
                        add.HomePoints = dal.HomePoints(one);
                        add.AwayPoints = dal.AwayPoints(one);
                    }
                }
                model.Matches.Add(add);
            }
            /*MatchViewModel one = new MatchViewModel();
            one.HomeContestant = "Testualni";
            one.AwayContestant = "Neki";
            one.HomePoints = 3;
            one.AwayPoints = 2;
            model.Matches.Add(one);
            MatchViewModel two = new MatchViewModel();
            two.HomeContestant = "Ranstualni";
            two.AwayContestant = "Nedsa";
            two.HomePoints = 5;
            two.AwayPoints = 1;
            model.Matches.Add(two);
            MatchViewModel tree = new MatchViewModel();
            tree.HomeContestant = "mam";
            tree.AwayContestant = "NDat";
            tree.HomePoints = 3;
            tree.AwayPoints = 6;
            model.Matches.Add(tree);
            MatchViewModel four = new MatchViewModel();
            four.HomeContestant = "Testdsa";
            four.AwayContestant = "Nerwawraretki";
            four.HomePoints = 8;
            four.AwayPoints = 5;
            model.Matches.Add(four);
            MatchViewModel fiv = new MatchViewModel();
            fiv.HomeContestant = "Tni";
            fiv.AwayContestant = "Nu";
            fiv.HomePoints = 1;
            fiv.AwayPoints = 2;
            model.Matches.Add(fiv);
            MatchViewModel six = new MatchViewModel();
            six.HomeContestant = "UzTe";
            six.AwayContestant = "Nekiter";
            six.HomePoints = 3;
            six.AwayPoints = 1;
            model.Matches.Add(six);
            MatchViewModel sevn = new MatchViewModel();
            sevn.HomeContestant = "Uoes";
            sevn.AwayContestant = "Nekib";
            sevn.HomePoints = 3;
            sevn.AwayPoints = 0;
            model.Matches.Add(sevn);*/
            model.Rounds = (int)Math.Log(model.Matches.Count + 1, 2);
            return View(model);
        }

        public ActionResult Table(int competition)
        {
            if (Request.Cookies["user"] == null)
            {
                return RedirectToAction("Login", "User");
            }
            string username = Logged_username();
            Set_TempData(username);
            var dal = new Competition_services();
            var dal2 = new Team_services();
            Competition current = dal.Check_existing(competition);
            LeagueViewModel model = new LeagueViewModel();
            model.Rounds = new List<RoundViewModel>();
            model.Standings = new List<Standing>();
            foreach (var one in current.Competition_contestants)
            {
                Standing TeamStanding = new Standing();
                TeamStanding.Team = dal2.Check_existing(one.Contestant.ID);
                model.Standings.Add(TeamStanding);
            }
            var i = (current.Competition_contestants.Count() - 1)*2;
            if (!current.Type_of_competition.DoubleQuadra) i *= 2;
            for (int j = 1; j < i + 1; j++)
            {
                RoundViewModel round = new RoundViewModel();
                round.matches = new List<SingleMatchModel>();
                List<Match> matches = dal.RoundMatches(current, j);
                foreach (var one in matches)
                {
                    SingleMatchModel match = new SingleMatchModel();
                    match.HomePoints = dal.HomePoints(one);
                    match.AwayPoints = dal.AwayPoints(one);
                    match.Match = one;
                    match.HomeContestant = dal2.Check_existing(one.Match_contestants.ElementAt(0).Contestant.ID).Name;
                    match.AwayContestant = dal2.Check_existing(one.Match_contestants.ElementAt(1).Contestant.ID).Name;
                    if (one.Locked)
                    {
                        model.Standings.FirstOrDefault(m => m.Team.Name == match.HomeContestant).PlayedMatches += 1;
                        model.Standings.FirstOrDefault(m => m.Team.Name == match.HomeContestant).GoalsScored += match.HomePoints;
                        model.Standings.FirstOrDefault(m => m.Team.Name == match.HomeContestant).GoalsConceded += match.AwayPoints;
                        model.Standings.FirstOrDefault(m => m.Team.Name == match.AwayContestant).PlayedMatches += 1;
                        model.Standings.FirstOrDefault(m => m.Team.Name == match.AwayContestant).GoalsScored += match.AwayPoints;
                        model.Standings.FirstOrDefault(m => m.Team.Name == match.AwayContestant).GoalsConceded += match.HomePoints;
                        if (match.HomePoints > match.AwayPoints)
                        {
                            model.Standings.FirstOrDefault(m => m.Team.Name == match.HomeContestant).Wins += 1;
                            model.Standings.FirstOrDefault(m => m.Team.Name == match.HomeContestant).Points += current.Type_of_competition.Victory_points;
                            model.Standings.FirstOrDefault(m => m.Team.Name == match.AwayContestant).Loses += 1;
                            model.Standings.FirstOrDefault(m => m.Team.Name == match.AwayContestant).Points += current.Type_of_competition.Defeat_points;
                        }
                        else if (match.HomePoints < match.AwayPoints)
                        {
                            model.Standings.FirstOrDefault(m => m.Team.Name == match.AwayContestant).Wins += 1;
                            model.Standings.FirstOrDefault(m => m.Team.Name == match.AwayContestant).Points +=
                                current.Type_of_competition.Victory_points;
                            model.Standings.FirstOrDefault(m => m.Team.Name == match.HomeContestant).Loses += 1;
                            model.Standings.FirstOrDefault(m => m.Team.Name == match.HomeContestant).Points +=
                                current.Type_of_competition.Defeat_points;
                        }
                        else
                        {
                            model.Standings.FirstOrDefault(m => m.Team.Name == match.HomeContestant).Draws += 1;
                            model.Standings.FirstOrDefault(m => m.Team.Name == match.HomeContestant).Points += current.Type_of_competition.Tie_points;
                            model.Standings.FirstOrDefault(m => m.Team.Name == match.AwayContestant).Draws += 1;
                            model.Standings.FirstOrDefault(m => m.Team.Name == match.AwayContestant).Points += current.Type_of_competition.Tie_points;
                        }
                    }
                    round.matches.Add(match);
                }
                round.Round = j;
                model.Rounds.Add(round);
            }
            model.Standings = model.Standings.OrderByDescending(m => m.Points).ToList();
            return View(model);
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
            List<Event_list> allEvents = dal.Sport_events(match.Match.League.Sport.ID);

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
        public String AddMatch()
        {
            if (Request.Cookies["user"] == null)
            {
                //return RedirectToAction("Login", "User");
            }
            string username = Logged_username();
            var dal = new Competition_services();
            Stream req = Request.InputStream;
            req.Seek(0, System.IO.SeekOrigin.Begin);
            string json = new StreamReader(req).ReadToEnd();
            DeserializedModel deserialized = JsonConvert.DeserializeObject<DeserializedModel>(json);
            dal.ProcessMatch(deserialized, username);
            string x = Request.Url.AbsolutePath;
            x = x.Replace("AddMatch", "ViewMatch");
            return x; 
        }

        public ActionResult ViewMatch(int id)
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
            MatchViewModel model = new MatchViewModel();
            model.Match = dal.One_match(id);
            model.HomePoints = dal.HomePoints(model.Match);
            model.AwayPoints = dal.AwayPoints(model.Match);
            if (dal3.Check_existing(model.Match.Match_contestants.ElementAt(0).Contestant.ID) != null)
            {
                model.HomeContestant = dal3.Check_existing(model.Match.Match_contestants.ElementAt(0).Contestant.ID).Name;
                model.AwayContestant = dal3.Check_existing(model.Match.Match_contestants.ElementAt(1).Contestant.ID).Name;
            }
            else
            {
                model.HomeContestant = dal2.Check_existing(model.Match.Match_contestants.ElementAt(0).Contestant.ID).Name;
                model.AwayContestant = dal2.Check_existing(model.Match.Match_contestants.ElementAt(1).Contestant.ID).Name;
            }
            model.Events = dal.Match_events(model.Match.ID);
            return View(model);
        }
    }
}