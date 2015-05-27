﻿using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;
using Application.Models;
using EFDatabase;

namespace Application.Services
{
    public class Competition_services
    {
        public void GenerateMatches(Competition competition, List<Contestant> teams)
        {
            using (var ctx = new Context())
            {
                List<Contestant> firstHalf = new List<Contestant>();
                int number = teams.Count/2;
                for (int n = 0; n < number; n++)
                {
                    firstHalf.Add(teams.ElementAt(0));
                    teams.RemoveAt(0);
                }
                for (int i = 0; i < teams.Count * 2 - 1; i++)
                {
                    for (int j = 0; j < teams.Count/2 + 1; j++)
                    {
                        SingleMatch(competition, firstHalf.ElementAt(j), teams.ElementAt(j));
                    }
                    Contestant movingTeam = teams.ElementAt(0);
                    teams.RemoveAt(0);
                    firstHalf.Insert(1, movingTeam);
                    movingTeam = firstHalf.ElementAt(firstHalf.Count - 1);
                    firstHalf.RemoveAt(firstHalf.Count - 1);
                    teams.Insert(teams.Count, movingTeam);
                }
                for (int i = 0; i < teams.Count * 2 - 1; i++)
                {
                    for (int j = 0; j < teams.Count / 2 + 1; j++)
                    {
                        SingleMatch(competition, teams.ElementAt(j), firstHalf.ElementAt(j));
                    }
                    Contestant movingTeam = teams.ElementAt(0);
                    teams.RemoveAt(0);
                    firstHalf.Insert(1, movingTeam);
                    movingTeam = firstHalf.ElementAt(firstHalf.Count - 1);
                    firstHalf.RemoveAt(firstHalf.Count - 1);
                    teams.Insert(teams.Count, movingTeam);
                }
                if (competition.Type_of_competition.DoubleQuadra == false)
                {
                    for (int i = 0; i < teams.Count * 2 - 1; i++)
                    {
                        for (int j = 0; j < teams.Count / 2 + 1; j++)
                        {
                            SingleMatch(competition, firstHalf.ElementAt(j), teams.ElementAt(j));
                        }
                        Contestant movingTeam = teams.ElementAt(0);
                        teams.RemoveAt(0);
                        firstHalf.Insert(1, movingTeam);
                        movingTeam = firstHalf.ElementAt(firstHalf.Count - 1);
                        firstHalf.RemoveAt(firstHalf.Count - 1);
                        teams.Insert(teams.Count, movingTeam);
                    }
                    for (int i = 0; i < teams.Count * 2 - 1; i++)
                    {
                        for (int j = 0; j < teams.Count / 2 + 1; j++)
                        {
                            SingleMatch(competition, teams.ElementAt(j), firstHalf.ElementAt(j));
                        }
                        Contestant movingTeam = teams.ElementAt(0);
                        teams.RemoveAt(0);
                        firstHalf.Insert(1, movingTeam);
                        movingTeam = firstHalf.ElementAt(firstHalf.Count - 1);
                        firstHalf.RemoveAt(firstHalf.Count - 1);
                        teams.Insert(teams.Count, movingTeam);
                    }
                }
            }
        }

        private void SingleMatch(Competition competition, Contestant homeTeam, Contestant awayTeam)
        {
            using (var ctx = new Context())
            {
                Match match = new Match();
                match.Date = DateTime.UtcNow;
                match.Locked = false;
                match.League = ctx.Competitions.FirstOrDefault(m => m.Name == competition.Name);
                ctx.Matches.Add(match);
                ctx.SaveChanges();

                Match_contestant home = new Match_contestant();
                home.Match = match;
                home.Contestant = ctx.Contestants.FirstOrDefault(m => m.ID == homeTeam.ID);
                ctx.Match_contestants.Add(home);
                ctx.SaveChanges();

                Match_contestant away = new Match_contestant();
                away.Match = match;
                away.Contestant = ctx.Contestants.FirstOrDefault(m => m.ID == awayTeam.ID);
                ctx.Match_contestants.Add(away);
                ctx.SaveChanges();
            }
        }

        public Competition Check_existing(string name)
        {
            using (var ctx = new Context())
            {
                return ctx.Competitions.Include("Sport").Include("User").Include("Competition_comments").Include("Competition_comments.User").FirstOrDefault(t => t.Name == name);
            }
        }

        public Competition Check_existing(int id)
        {
            using (var ctx = new Context())
            {
                return ctx.Competitions.Include("Sport").Include("Type_of_competition").Include("Sport.Sport_type").Include("User").Include("Competition_comments").Include("Competition_comments.User").Include("Competition_contestants").Include("Competition_contestants.Contestant").FirstOrDefault(t => t.ID == id);
            }
        }

        public List<Competition> Competition_list()
        {
            using (var ctx = new Context())
            {
                return ctx.Competitions.Include("Type_of_competition").Include("Sport").Include("User").Include("Competition_comments").Include("Competition_comments.User").ToList();
            }
        }

        public void New_competition(SingleCompetitionModel model, string username)
        {
            using (var ctx = new Context())
            {
                List<Contestant> contestants = new List<Contestant>();
                foreach (var one in model.Competitors)
                {
                    var dal = new Team_services();
                    var dal2 = new Player_services();
                    Team x = dal.Check_existing(one);
                    if (x == null)
                    {
                        Player y = dal2.Check_existing(one);
                        contestants.Add(ctx.Contestants.FirstOrDefault(m => m.ID == y.ID));
                    }
                    else contestants.Add(ctx.Contestants.FirstOrDefault(m => m.ID == x.ID));
                }

                Competition competition = new Competition();
                competition.Name = model.Competition.Name;
                competition.Country = model.Competition.Country;
                competition.Emblem = model.Competition.Emblem;
                competition.Sport = ctx.Sports.FirstOrDefault(u => u.ID == model.Competition.Sport.ID);
                competition.User = ctx.Users.FirstOrDefault(u => u.Username == username);
                competition.Type_of_competition = ctx.Type_of_competitions.FirstOrDefault(u => u.ID == model.Competition.Type_of_competition.ID);
                competition.Number_of_competitors = model.Competitors.Count;
                competition.Season = DateTime.Today.Year;
                ctx.Competitions.Add(competition);
                ctx.SaveChanges();

                if (model.Comment != null)
                {
                    Competition_comment commentDB = new Competition_comment();
                    commentDB.Comment = model.Comment;
                    commentDB.Active = true;
                    commentDB.Date = DateTime.UtcNow;
                    commentDB.Competition = competition;
                    commentDB.User = ctx.Users.FirstOrDefault(u => u.Username == username);
                    ctx.Competition_comments.Add(commentDB);
                    ctx.SaveChanges();
                }

                foreach (Contestant one in contestants)
                {
                    Competition_contestant contestant = new Competition_contestant();
                    contestant.Competition = competition;
                    contestant.Contestant = one;
                    ctx.Competition_contestants.Add(contestant);
                    ctx.SaveChanges();
                }

                GenerateMatches(competition, contestants);
            }
        }

        public List<Match> RoundMatches(Competition competition, int kolo)
        {
            using (var ctx = new Context())
            {
                List<Match> allMatches = ctx.Matches.Include("Match_contestants.Contestant").Where(m => m.League.ID == competition.ID).ToList();
                return allMatches.GetRange((kolo - 1) * competition.Number_of_competitors / 2, competition.Number_of_competitors / 2);
            }
        }

        public Match One_match(int id)
        {
            using (var ctx = new Context())
            {
                return ctx.Matches.Include("League").Include("Events").Include("Match_comments").Include("Match_contestants.Contestant").FirstOrDefault(m => m.ID == id);
            }
        }

        public void ProcessMatch(DeserializedModel deserialized, string username)
        {
            using (var ctx = new Context())
            {
                Match currentMatch = ctx.Matches.FirstOrDefault(m => m.ID == deserialized.ID);

                if (deserialized.Comment != null)
                {
                    try
                    {
                        Match_comment commentDB = new Match_comment();
                        commentDB.Comment = deserialized.Comment;
                        commentDB.Active = true;
                        commentDB.Date = DateTime.UtcNow;
                        commentDB.Match = currentMatch;
                        commentDB.User = ctx.Users.FirstOrDefault(u => u.Username == username);
                        ctx.Match_comments.Add(commentDB);
                        ctx.SaveChanges();
                    }
                    catch (DbEntityValidationException ex)
                    {

                        Console.WriteLine(ex);
                    }
                }
                
                foreach (var one in deserialized.Events)
                {
                    try
                    {
                        Event matchEvent = new Event();
                        matchEvent.Length = one.LE;
                        matchEvent.Timestamp = DateTime.UtcNow;
                        matchEvent.Time = "LOL";
                        matchEvent.Name = one.Name;
                        if ( one.PO1 != "") matchEvent.Points1 = Convert.ToInt32(one.PO1);
                        if ( one.PO2 != "") matchEvent.Points2 = Convert.ToInt32(one.PO2);
                        matchEvent.Match = currentMatch;
                        var dal = new Player_services();
                        if ( one.Player1 ) matchEvent.Player1 = ctx.Players.FirstOrDefault(m => m.Name == one.P1);
                        if ( one.Player2 ) matchEvent.Player2 = ctx.Players.FirstOrDefault(m => m.Name == one.P2);
                        ctx.Events.Add(matchEvent);
                        ctx.SaveChanges();
                    }
                    catch (DbEntityValidationException ex)
                    {

                        Console.WriteLine(ex);
                    }
                }
            }
        }

        public int HomePoints(Match one)
        {
            int points = 0;
            using (var ctx = new Context())
            {
                List<Event> allMatchEvents = ctx.Events.Where(m => m.Match.ID == one.ID).ToList();
                foreach (var eventOne in allMatchEvents)
                {
                    Event_list type = ctx.Event_list.FirstOrDefault(m => m.Name == eventOne.Name);
                    if (type.Primary)
                    {
                        if (!(type.Player1 ^ type.Player2))
                        {
                            if (type.Counts)
                            {
                                points += eventOne.Points1;
                            }
                            else if (eventOne.Points1 > eventOne.Points2)
                            {
                                points += 1;
                            }
                        }
                        else
                        {
                            Player igrac = ctx.Players.FirstOrDefault(m => m.Name == eventOne.Player1.Name);
                            if (igrac.Play_in_team.ElementAt(0).Team.ID ==
                                one.Match_contestants.ElementAt(0).Contestant.ID)
                            {
                                points += eventOne.Points1;
                            }
                            var x = 2;
                        } 
                    }     
                }
            }
            return points;
        }

        public int AwayPoints(Match one)
        {
            int points = 0;
            using (var ctx = new Context())
            {
                List<Event> allMatchEvents = ctx.Events.Where(m => m.Match.ID == one.ID).ToList();
                foreach (var eventOne in allMatchEvents)
                {
                    Event_list type = ctx.Event_list.FirstOrDefault(m => m.Name == eventOne.Name);
                    if (type.Primary)
                    {
                        if (!(type.Player1 ^ type.Player2))
                        {
                            if (type.Counts)
                            {
                                points += eventOne.Points2;
                            }
                            else if (eventOne.Points1 < eventOne.Points2)
                            {
                                points += 1;
                            }
                        }
                        else
                        {
                            Player igrac = ctx.Players.FirstOrDefault(m => m.Name == eventOne.Player1.Name);
                            if (igrac.Play_in_team.ElementAt(0).Team.ID ==
                                one.Match_contestants.ElementAt(1).Contestant.ID)
                            {
                                points += eventOne.Points1;
                            }
                        }
                    }
                }
            }
            return points;
        }
    }
}