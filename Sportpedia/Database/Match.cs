using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database
{
    public class Match
    {
        [Key]
        public int ID { get; set; }
        [Required]
        public virtual League League { get; set; }
        [Required, InverseProperty("Home_Team")]
        public virtual Team Home_Team { get; set; }
        [Required, InverseProperty("Away_Team")]
        public virtual Team Away_Team { get; set; }
        [Required]
        public bool Locked { get; set; }
        public virtual Match_Information Current_Match_Information { get; set; }
        [InverseProperty("Match")]
        public virtual List<Match_Information> Match_Informations { get; set; }
    }
}
