using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFDatabase
{
    public class Match
    {
        [Key]
        public int ID { get; set; }
        [Required]
        public virtual Competition League { get; set; }
        public virtual List<Match_contestant> Match_contestants { get; set; }
        [Required]
        public bool Locked { get; set; }
        [Required]
        public DateTime Date { get; set; }
        public virtual List<Match_comment> Match_comments { get; set; }
        public virtual List<Event> Events { get; set; }
    }
}
