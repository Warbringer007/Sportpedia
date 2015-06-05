using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EFDatabase;

namespace Application.Models
{
    public class StadiumModel
    {
        public Stadium Stadium { get; set; }
        public HttpPostedFileBase Image { get; set; }
    }
}