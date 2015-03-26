using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EFDatabase;

namespace Application.Models
{
    public class UserViewModel
    {
        public IEnumerable<User> UserList { get; set; }
    }
}