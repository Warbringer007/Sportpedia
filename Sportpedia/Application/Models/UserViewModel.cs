using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Database;

namespace Application.Models
{
    public class UserViewModel
    {
        public IEnumerable<User> UserList { get; set; }
    }
}