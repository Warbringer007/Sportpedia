using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Application.Models
{
    public class RoundViewModel
    {
        public List<SingleMatchModel> matches { get; set; }
        public int Round { get; set; }
    }
}