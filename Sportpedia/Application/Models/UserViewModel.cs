using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EFDatabase;

namespace Application.Models
{
    public class UserViewModel
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string NewPassword { get; set; }
        public string Email { get; set; }
        public string Permissions { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public DateTime Date_of_birth { get; set; }
    }
}