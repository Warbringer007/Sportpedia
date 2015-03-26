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
        public virtual DbSet<League> Leagues { get; set; }
        public virtual DbSet<Team> Teams { get; set; }
        public virtual DbSet<League_contestants> League_contestants { get; set; }
        public virtual DbSet<Match> Matches { get; set; }
        public virtual DbSet<Match_Information> Match_Informations { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
        }

    }
}
