using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EFDatabase;
using Application.Models;
using System.Linq;
using System.ComponentModel;
using Application.Services;

namespace Application.Services
{
    public class Sport_services
    {
        public BindingList<Sport_type> Sportski_tipovi()
        {
            BindingList<Sport_type> list = new BindingList<Sport_type>();
            using (var ctx = new Context())
            {
                List<Sport_type> lista = ctx.Sport_types.ToList();
                foreach (var tip in lista)
                {
                    list.Add(tip);
                }
            }
            return list;
        }

        public Sport Check_existing(string name)
        {
             using (var ctx = new Context())
             {
                 return ctx.Sports.Where(s => s.Name == name).FirstOrDefault();
             }
        }

        public void New_sport(SingleSportModel model, string username)
        {
            using (var ctx = new Context())
            {
                Sport sport = new Sport();
                sport.Name = model.Sport.Name;
                sport.Sport_type = ctx.Sport_types.FirstOrDefault(t => t.ID == model.Sport.Sport_type.ID);
                sport.User = ctx.Users.FirstOrDefault(u => u.Username == username);
                ctx.Sports.Add(sport);
                ctx.SaveChanges();

                Sport_comment commentDB = new Sport_comment();
                commentDB.Comment = model.Comment;
                commentDB.Active = true;
                commentDB.Date = DateTime.UtcNow;
                commentDB.Sport = sport;
                commentDB.User = ctx.Users.FirstOrDefault(u => u.Username == username);
                ctx.Sport_comments.Add(commentDB);
                ctx.SaveChanges();

                foreach (All_Events one in model.Eventi)
                {
                    if (one.Checked)
                    {
                        Sport_event oneevent = new Sport_event();
                        oneevent.Sport = sport;
                        oneevent.Event_list = ctx.Event_list.Single(e => e.Name == one.Event);
                        ctx.Sport_events.Add(oneevent);
                        ctx.SaveChanges();
                    }
                }

                /*Sport_event mainevent = new Sport_event();
                mainevent.Sport = sport;
                if (model.Eventi.ToString().Equals("Set")) mainevent.Event_list = ctx.Event_list.Single(e => e.Name == "Set");
                else mainevent.Event_list = ctx.Event_list.Single(e => e.Name == "Goal");
                ctx.Sport_events.Add(mainevent);
                ctx.SaveChanges();

                if (model.Penalty)
                {
                    Sport_event penalty = new Sport_event();
                    penalty.Sport = sport;
                    penalty.Event_list = ctx.Event_list.Single(e => e.Name == "Penalty");
                    ctx.Sport_events.Add(penalty);
                    ctx.SaveChanges();
                }

                if (model.Penalty_kick)
                {
                    Sport_event penalty_kick = new Sport_event();
                    penalty_kick.Sport = sport;
                    penalty_kick.Event_list = ctx.Event_list.Single(e => e.Name == "Penalty kick");
                    ctx.Sport_events.Add(penalty_kick);
                    ctx.SaveChanges();
                }

                if (model.Red_card)
                {
                    Sport_event red_card = new Sport_event();
                    red_card.Sport = sport;
                    red_card.Event_list = ctx.Event_list.Single(e => e.Name == "Red card");
                    ctx.Sport_events.Add(red_card);
                    ctx.SaveChanges();
                }

                if (model.Substitution)
                {
                    Sport_event substitution = new Sport_event();
                    substitution.Sport = sport;
                    substitution.Event_list = ctx.Event_list.Single(e => e.Name == "Substitution");
                    ctx.Sport_events.Add(substitution);
                    ctx.SaveChanges();
                }

                if (model.Yellow_card)
                {
                    Sport_event yellow_card = new Sport_event();
                    yellow_card.Sport = sport;
                    yellow_card.Event_list = ctx.Event_list.Single(e => e.Name == "Yellow card");
                    ctx.Sport_events.Add(yellow_card);
                    ctx.SaveChanges();
                }*/
            }
        }

        public SportViewModel Sport_information(string name)
        {
            using (var ctx = new Context())
            {
                SportViewModel sport = new SportViewModel();
                sport.Sport = ctx.Sports.Include("User").Single(s => s.Name == name);
                sport.ActualComment = ctx.Sport_comments.Single(s => s.Sport.ID == sport.Sport.ID && s.Active == true).Comment;
                return sport;
            }
        }

        public List<SportViewModel> Sport_list()
        {
            using (var ctx = new Context())
            {
                List<SportViewModel> list = new List<SportViewModel>();
                IEnumerable<Sport> sports = ctx.Sports.ToList();
                foreach (var sport in sports)
                {
                    var jedan = new SportViewModel();
                    jedan.Sport = ctx.Sports.Include("User").Single(s => s.Name == sport.Name);
                    jedan.ActualComment = ctx.Sport_comments.Single(s => s.Sport.ID == sport.ID && s.Active == true).Comment;
                    list.Add(jedan);
                }
                return list;
            }
        }
    }
}