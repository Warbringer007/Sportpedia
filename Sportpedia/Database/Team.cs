using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFDatabase
{
    public class Team
    {
        [Key]
        public int ID { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public DateTime Founded { get; set; }
        [Required]
        public string Country { get; set; }
        public string Fans_Name { get; set; }
        public string Stadium { get; set; }
        public byte[] Emblem { get; set; }
        public string Comment { get; set; }
        [Required]
        public virtual User Last_contributor { get; set; }
        [InverseProperty("Home_Team")]
        public virtual List<Match> Home_Matches { get; set; }
        [InverseProperty("Away_Team")]
        public virtual List<Match> Away_Matches { get; set; }
        public virtual List<League_contestants> Competitions { get; set; }
    }
}
