using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EFDatabase;
using Application.Models;

namespace Application.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            using (var ctx = new Context())
            {
                UserViewModel model = new UserViewModel
                {
                    UserList = ctx.Users.ToList()
                };
            }
            return View();
        }
    }
}