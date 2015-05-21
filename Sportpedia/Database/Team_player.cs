using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace EFDatabase
{
    public class Team_player
    {
        [Key]
        public int ID { get; set; }
        [Required]
        public int Season { get; set; }
        [Required]
        public virtual Player Player { get; set; }
        [Required]
        public virtual Team Team { get; set; }
    }
}
