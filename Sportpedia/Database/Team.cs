using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database
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
        public byte[] Emblem { get; set; }
        [Required]
        public virtual User Last_contributor { get; set; }
        [InverseProperty("Home_Team")]
        public virtual List<Match> Home_Team { get; set; }
        [InverseProperty("Away_Team")]
        public virtual List<Match> Away_Team { get; set; }
    }
}
