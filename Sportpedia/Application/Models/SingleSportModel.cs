using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EFDatabase;

namespace Application.Models
{

    public class All_Events
    {
        public bool Checked { get; set; }
        public string Event { get; set; }
    }
    public class SingleSportModel
    {
        public Sport Sport { get; set; }
        public string Comment { get; set; }
        public List<All_Events> Eventi { get; set; }
    }
}