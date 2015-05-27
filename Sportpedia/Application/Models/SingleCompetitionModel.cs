using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EFDatabase;

namespace Application.Models
{
    public class SingleCompetitionModel
    {
        public Competition Competition { get; set; }
        public IEnumerable<SelectListItem> Sports { get; set; }
        public List<string> Competitors { get; set; }
        public HttpPostedFileBase Image { get; set; }
        public string Comment { get; set; }
    }
}