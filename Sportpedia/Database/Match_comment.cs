using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace EFDatabase
{
    public class Match_comment
    {
        [Key]
        public int ID { get; set; }
        [Required]
        public string Comment { get; set; }
        [Required]
        public bool Active { get; set; }
        [Required]
        public DateTime Date { get; set; }
        public virtual User User { get; set; }
        public virtual Match Match { get; set; }
    }
}
