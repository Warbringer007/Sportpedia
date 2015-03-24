using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database
{
    public enum League_type
    {
        Two_Games = 1,
        Four_Games = 2
    }
    public class League
    {
        [Key]
        public int ID { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public int Season { get; set; }
        [Required]
        public string Country { get; set; }
        [Required]
        public League_type Type_of_league{ get; set; }
        [Required]
        public virtual User Creator { get; set; }
        [Required]
        public int Sport { get; set; }
        public byte[] Emblem { get; set; }
        public string Comment { get; set; }
        public virtual List<League_contestants> League_contestants { get; set; }
        public virtual List<Match> Match { get; set; }
    }
}
