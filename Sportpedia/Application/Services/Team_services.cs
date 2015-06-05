using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using Application.Models;
using EFDatabase;

namespace Application.Services
{
    public class Team_services
    {
        public BindingList<Sport> Sportovi()
        {
            BindingList<Sport> list = new BindingList<Sport>();
            using (var ctx = new Context())
            {
                List<Sport> lista = ctx.Sports.ToList();
                foreach (var tip in lista)
                {
                    list.Add(tip);
                }
            }
            return list;
        }

        public Team Check_existing(string name)
        {
            using (var ctx = new Context())
            {
                return ctx.Teams.Include("Sport").Include("User").Include("Team_comments").Include("Team_players").Include("Team_players.Player").Include("Team_comments.User").FirstOrDefault(t => t.Name == name);
            }
        }

        public Team Check_existing(int id)
        {
            using (var ctx = new Context())
            {
                return ctx.Teams.Include("Stadiums").Include("Sport").Include("User").Include("Team_comments").Include("Team_players").Include("Team_players.Player").Include("Team_comments.User").FirstOrDefault(t => t.ID == id);
            }
        }

        public void New_Team(SingleTeamModel model, string username)
        {
            using (var ctx = new Context())
            {
                Team team = new Team();
                team.Name = model.Team.Name;
                team.Country = model.Team.Country;
                team.Fans_Name = model.Team.Fans_Name;
                team.Founded = model.Team.Founded;
                team.Emblem = model.Team.Emblem;
                team.Webpage = model.Team.Webpage;
                team.Sport = ctx.Sports.FirstOrDefault(u => u.ID == model.Team.Sport.ID);
                team.User = ctx.Users.FirstOrDefault(u => u.Username == username);
                ctx.Teams.Add(team);
                ctx.SaveChanges();

                if (model.Comment != null)
                {
                    Team_comment commentDB = new Team_comment();
                    commentDB.Comment = model.Comment;
                    commentDB.Active = true;
                    commentDB.Date = DateTime.UtcNow;
                    commentDB.Team = team;
                    commentDB.User = ctx.Users.FirstOrDefault(u => u.Username == username);
                    ctx.Team_comments.Add(commentDB);
                    ctx.SaveChanges();
                }

                if (model.Stadiums != null)
                {
                    foreach (var one in model.Stadiums)
                    {
                        Stadium stadium = ctx.Stadiums.FirstOrDefault(m => m.ID == one);
                        stadium.Team = team;
                        ctx.SaveChanges();
                    }
                }
            }
        }

        public List<Team> Team_list()
        {
            using (var ctx = new Context())
            {
                IEnumerable<Team> teams = ctx.Teams.ToList();
                return (from team in teams let jedan = new Team() select ctx.Teams.Include("Sport").Include("User").Single(s => s.Name == team.Name)).ToList();
            }
        }

        public void Add_comment(SingleTeamModel model, string username)
        {
            using (var ctx = new Context())
            {
                Team_comment commentDB = new Team_comment();
                commentDB.Comment = model.Comment;
                commentDB.Active = true;
                commentDB.Date = DateTime.UtcNow;
                commentDB.Team = ctx.Teams.Include("Sport").Include("User").FirstOrDefault(t => t.ID == model.Team.ID);
                commentDB.User = ctx.Users.FirstOrDefault(u => u.Username == username);
                ctx.Team_comments.Add(commentDB);
                ctx.SaveChanges();
            }
        }

        public void Edit_Team(SingleTeamModel model)
        {
            using (var ctx = new Context())
            {
                Team team = ctx.Teams.Include("Sport").Include("User").Include("Team_comments").Include("Team_comments.User").FirstOrDefault(t => t.ID == model.Team.ID);
                team.Name = model.Team.Name;
                team.Country = model.Team.Country;
                team.Fans_Name = model.Team.Fans_Name;
                team.Founded = model.Team.Founded;
                //team.Stadium = model.Team.Stadium;
                if ( model.Team.Emblem != null ) team.Emblem = model.Team.Emblem;
                team.Webpage = model.Team.Webpage;
                ctx.SaveChanges();
            }
        }

        public BindingList<Player> Igraci(Sport sport)
        {
            using (var ctx = new Context())
            {
                return new BindingList<Player>(ctx.Players.Where(m => m.Sport.Name == sport.Name && m.Play_in_team.Count == 0).ToList());
            }
        }

        public void Add_Player(SingleTeamModel model)
        {
            using (var ctx = new Context())
            {
                Team_player newPlayer = new Team_player();
                newPlayer.Player = ctx.Players.FirstOrDefault(m => m.ID == model.Player);
                newPlayer.Team = ctx.Teams.FirstOrDefault(m => m.ID == model.Team.ID);
                newPlayer.Season = DateTime.Today.Year;
                ctx.Team_players.Add(newPlayer);
                ctx.SaveChanges();
            }
        }

        public List<string> TeamPlayers(string homeContestant, string awayContestant)
        {
            using (var ctx = new Context())
            {
                List<string> players = new List<string>();
                Team home = Check_existing(homeContestant);
                if (home != null)
                {
                    foreach (var one in home.Team_players)
                    {
                        players.Add(one.Player.Name);
                    }
                    Team away = Check_existing(awayContestant);
                    foreach (var one in away.Team_players)
                    {
                        players.Add(one.Player.Name);
                    }
                }
                return players;
            }
        }

        public BindingList<Stadium> Stadioni()
        {
            using (var ctx = new Context())
            {
                return new BindingList<Stadium>(ctx.Stadiums.Where(m => m.Team == null).ToList());
            }
        }
    }
}