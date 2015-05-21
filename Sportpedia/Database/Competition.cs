using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFDatabase
{
    public class Competition
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
        public virtual Type_of_competition Type_of_competition { get; set; }
        [Required]
        public virtual User User { get; set; }
        [Required]
        public virtual Sport Sport { get; set; }
        public int Number_of_competitors { get; set; }
        public byte[] Emblem { get; set; }
        public virtual List<Competition_comment> Competition_comments { get; set; }
        public virtual List<Competition_contestant> Competition_contestants { get; set; }
        public virtual List<Match> Match { get; set; }
    }
}
