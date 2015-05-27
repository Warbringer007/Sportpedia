using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Application.Models
{
    public class DeserializedEvent
    {
        public string Name { get; set; }
        public bool Player1 { get; set; }
        public bool Player2 { get; set; }
        public bool Points1 { get; set; }
        public bool Points2 { get; set; }
        public bool Length { get; set; }
        public string P1 { get; set; }
        public string P2 { get; set; }
        public string PO1 { get; set; }
        public string PO2 { get; set; }
        public string LE { get; set; }
        public string DA { get; set; }
    }
    public class DeserializedModel
    {
        public int ID { get; set; }
        public string homeContestant { get; set; }
        public string awayContestant { get; set; }
        public string Date { get; set; }
        public string Comment { get; set; }
        public List<DeserializedEvent> Events { get; set; }
        public List<object> EventSettings { get; set; }
        public List<string> Contestants { get; set; }
    }
}