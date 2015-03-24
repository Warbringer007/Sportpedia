using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Database
{
    public class League_contestants
    {
        [Key]
        public int ID { get; set; }
        [Required]
        public virtual League League { get; set; }
        [Required]
        public virtual Team Team { get; set; }
    }
}
