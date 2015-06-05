using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFDatabase
{
    public class Context : DbContext
    {
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Type_of_competition> Type_of_competitions { get; set; }
        public virtual DbSet<Team_player> Team_players { get; set; }
        public virtual DbSet<Team_comment> Team_comments { get; set; }
        public virtual DbSet<Sport_type> Sport_types { get; set; }
        public virtual DbSet<Sport_event> Sport_events { get; set; }
        public virtual DbSet<Sport_comment> Sport_comments { get; set; }
        public virtual DbSet<Sport> Sports { get; set; }
        public virtual DbSet<Player_comment> Player_comments { get; set; }
        public virtual DbSet<Player> Players { get; set; }
        public virtual DbSet<Permission> Permissions { get; set; }
        public virtual DbSet<Match_contestant> Match_contestants { get; set; }
        public virtual DbSet<Match_comment> Match_comments { get; set; }
        public virtual DbSet<Match> Matches { get; set; }
        public virtual DbSet<Event_list> Event_list { get; set; }
        public virtual DbSet<Event> Events { get; set; }
        public virtual DbSet<Contestant> Contestants { get; set; }
        public virtual DbSet<Stadium> Stadiums { get; set; }
        public virtual DbSet<Competition_contestant> Competition_contestants { get; set; }
        public virtual DbSet<Competition_comment> Competition_comments { get; set; }
        public virtual DbSet<Competition> Competitions { get; set; }
        public virtual DbSet<Team> Teams { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
        }

    }
}
