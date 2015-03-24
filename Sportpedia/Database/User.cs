using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database
{
    public enum Permission
    {
        Normal = 1,
        Moderator = 2
    }
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
        public Permission Permissions { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public DateTime Date_of_birth { get; set; }
        public virtual List<League> Leagues { get; set; }
        public virtual List<Team> Teams { get; set; }
        public virtual List<Match_Information> Match_Informations { get; set; }
    }
    
}
