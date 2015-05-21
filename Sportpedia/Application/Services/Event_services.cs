using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EFDatabase;

namespace Application.Services
{
    public class Event_services
    {
        public Event_list Check_existing(string name)
        {
            using (var ctx = new Context())
            {
                return ctx.Event_list.FirstOrDefault(s => s.Name == name);
            }
        }

        public void New_event(Event_list model)
        {
            using (var ctx = new Context())
            {
                ctx.Event_list.Add(model);
                ctx.SaveChanges();
            }
        }

        public List<Event_list> Event_list()
        {
            using (var ctx = new Context())
            {
                return ctx.Event_list.ToList();
            }
        }
    }
}