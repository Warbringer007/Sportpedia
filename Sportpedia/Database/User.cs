using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFDatabase
{
    public class User
    {
        [Key]
        public int ID { get; set; }
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
        public string Email { get; set; }
        [Required]
        public virtual Permission Permissions { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public DateTime Date_of_birth { get; set; }
        public virtual List<Competition> Competitions { get; set; }
        public virtual List<Team> Teams { get; set; }
        public virtual List<Player> Players { get; set; }
        public virtual List<Sport> Sports { get; set; }
        public virtual List<Sport_comment> Sport_comments { get; set; }
        public virtual List<Player_comment> Player_comments { get; set; }
        public virtual List<Team_comment> Team_comments { get; set; }
        public virtual List<Match_comment> Match_comments { get; set; }
        public virtual List<Competition_comment> Competition_comments { get; set; }
    }
    
}
