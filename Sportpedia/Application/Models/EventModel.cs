using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Application.Models
{
    public class EventModel
    {
        public string Name { get; set; }
        public bool Primary { get; set; }
        public bool Player1 { get; set; }
        public bool Player2 { get; set; }
        public bool Points1 { get; set; }
        public bool Points2 { get; set; }
        public bool Counts { get; set; }
        public bool Length { get; set; }
    }
}