using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Web;
using Application.Models;
using EFDatabase;

namespace Application.Services
{
    public class Player_services
    {
        public Player Check_existing(string name)
        {
            using (var ctx = new Context())
            {
                return ctx.Players.Include("Sport").Include("User").Include("Player_comments").Include("Play_in_team.Team").Include("Player_comments.User").FirstOrDefault(t => t.Name == name);
            }
        }

        public Player Check_existing(int id)
        {
            using (var ctx = new Context())
            {
                return ctx.Players.Include("Sport").Include("User").Include("Player_comments").Include("Play_in_team.Team").Include("Player_comments.User").FirstOrDefault(t => t.ID == id);
            }
        }

        public void New_Player(SinglePlayerModel model, string username)
        {
            using (var ctx = new Context())
            {
                Player player = new Player();
                player.Name = model.Player.Name;
                player.Country = model.Player.Country;
                player.Sex = model.Sex ? "Ženski" : "Muški";
                player.Date_of_birth = model.Player.Date_of_birth;
                player.Picture = model.Player.Picture;
                player.Sport = ctx.Sports.FirstOrDefault(u => u.ID == model.Player.Sport.ID);
                player.User = ctx.Users.FirstOrDefault(u => u.Username == username);
                ctx.Players.Add(player);
                ctx.SaveChanges();

                if (model.Comment != null)
                {
                    Player_comment commentDB = new Player_comment();
                    commentDB.Comment = model.Comment;
                    commentDB.Active = true;
                    commentDB.Date = DateTime.UtcNow;
                    commentDB.Player = player;
                    commentDB.User = ctx.Users.FirstOrDefault(u => u.Username == username);
                    ctx.Player_comments.Add(commentDB);
                    ctx.SaveChanges();
                }
            }
        }

        public List<Player> Player_list()
        {
            using (var ctx = new Context())
            {
                return ctx.Players.Include("User").Include("Sport").ToList();
            }
        }

        public void Add_comment(SinglePlayerModel model, string username)
        {
            using (var ctx = new Context())
            {
                Player_comment commentDB = new Player_comment();
                commentDB.Comment = model.Comment;
                commentDB.Active = true;
                commentDB.Date = DateTime.UtcNow;
                commentDB.Player = ctx.Players.Include("Sport").Include("User").FirstOrDefault(t => t.ID == model.Player.ID);
                commentDB.User = ctx.Users.FirstOrDefault(u => u.Username == username);
                ctx.Player_comments.Add(commentDB);
                ctx.SaveChanges();
            }
        }

        public void Edit_Player(SinglePlayerModel model)
        {
            using (var ctx = new Context())
            {
                Player player = ctx.Players.Include("Sport").Include("User").Include("Player_comments").Include("Player_comments.User").FirstOrDefault(t => t.ID == model.Player.ID);
                player.Name = model.Player.Name;
                player.Country = model.Player.Country;
                player.Sex = model.Sex ? "Ženski" : "Muški";
                player.Date_of_birth = model.Player.Date_of_birth;
                if (model.Player.Picture != null) player.Picture = model.Player.Picture;
                ctx.SaveChanges();
            }
        }
    }
}