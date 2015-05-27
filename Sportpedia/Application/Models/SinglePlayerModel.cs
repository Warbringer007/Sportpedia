using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EFDatabase;

namespace Application.Models
{
    public class SinglePlayerModel
    {
        public bool Sex { get; set; }
        public Player Player { get; set; }
        public string Comment { get; set; }
        public HttpPostedFileBase Image { get; set; }
    }
}