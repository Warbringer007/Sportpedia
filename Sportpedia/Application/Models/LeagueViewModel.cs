using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EFDatabase;

namespace Application.Models
{
    public class Standing
    {
        public Team Team { get; set; }
        public int PlayedMatches { get; set; }
        public int Wins { get; set; }
        public int Draws { get; set; }
        public int Loses { get; set; }
        public int GoalsScored { get; set; }
        public int GoalsConceded { get; set; }
        public int Points { get; set; }

    }
    public class LeagueViewModel
    {
        public List<RoundViewModel> Rounds { get; set; }
        public List<Standing> Standings { get; set; }
        public int Competition { get; set; }
    }
}