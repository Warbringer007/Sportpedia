using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EFDatabase;

namespace Application.Models
{
    public class MatchViewModel
    {
        public Match Match { get; set; }
        public List<Event> Events { get; set; }
        public string HomeContestant { get; set; }
        public string AwayContestant { get; set; }
        public int HomePoints { get; set; }
        public int AwayPoints { get; set; }
        public string Comment { get; set; }
    }
}