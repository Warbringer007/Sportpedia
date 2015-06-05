using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFDatabase
{
    [Table("Stadiums")]
    public class Stadium
    {
        [Key]
        public int ID { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public int Capacity { get; set; }
        [Required]
        public string Country { get; set; }
        public byte[] Picture { get; set; }
        public int Latitude { get; set; }
        public int Longitude { get; set; }
        public virtual Team Team { get; set; }
    }
}
