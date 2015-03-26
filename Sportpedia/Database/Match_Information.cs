using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace EFDatabase
{
    public class Match_Information
    {
        [Required]
        public int ID { get; set; }
        [Required, InverseProperty("Match_Informations")]
        public virtual Match Match { get; set; }
        [Required]
        public DateTime Date { get; set; }
        public int Home_first_quarter_goals { get; set; }
        public int Home_first_half_goals { get; set; }
        public int Home_third_quarter_goals { get; set; }
        [Required]
        public int Home_end_goals { get; set; }
        public int Away_first_quarter_goals { get; set; }
        public int Away_first_half_goals { get; set; }
        public int Away_third_quarter_goals { get; set; }
        [Required]
        public int Away_end_goals { get; set; }
        [Required]
        public virtual User Contributor { get; set; }
        [Required]
        public int Viewers { get; set; }
    }
}
