using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFDatabase
{
    [Table("Teams")]
    public class Team : Contestant
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public DateTime Founded { get; set; }
        [Required]
        public string Country { get; set; }
        public string Fans_Name { get; set; }
        public string Webpage { get; set; }
        public byte[] Emblem { get; set; }
        public virtual List<Team_comment> Team_comments { get; set; }
        [Required]
        public virtual User User { get; set; }
        [Required]
        public virtual Sport Sport { get; set; }
        public virtual List<Team_player> Team_players { get; set; }
        public virtual List<Stadium> Stadiums { get; set; }
    }
}
