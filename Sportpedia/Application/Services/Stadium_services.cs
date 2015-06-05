using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Application.Models;
using EFDatabase;

namespace Application.Services
{
    public class Stadium_services
    {
        public void New_stadium(Stadium model, string username)
        {
            using (var ctx = new Context())
            {
                ctx.Stadiums.Add(model);
                ctx.SaveChanges();
            }
        }
    }
}