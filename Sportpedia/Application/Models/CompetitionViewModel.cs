using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EFDatabase;

namespace Application.Models
{
    public class CompetitionViewModel
    {
        public Competition Competition { get; set; }
        public IEnumerable<SelectListItem> Sports { get; set; }
        public HttpPostedFileBase Image { get; set; }
        public string Comment { get; set; }
        public List<Team> Teams { get; set; }
        public List<Player> Players { get; set; }
        public int Rounds { get; set; }
    }
}