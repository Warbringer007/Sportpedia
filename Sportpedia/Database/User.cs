using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database
{
    public enum Permission
    {
        Normal = 1,
        Moderator = 2
    }
    public class User
    {
        [Key]
        public int ID { get; set; }
        [Required]
        public string username { get; set; }
        [Required]
        public string password { get; set; }
        public string email { get; set; }
        [Required]
        public Permission permissions { get; set; }
        public string name { get; set; }
        public string surname { get; set; }
        public DateTime date_of_birth { get; set; }
    }
    
}
