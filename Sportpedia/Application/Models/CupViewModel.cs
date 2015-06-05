using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Application.Models
{
    public class CupViewModel
    {
        public List<MatchViewModel> Matches { get; set; }
        public int Rounds { get; set; }
    }
}