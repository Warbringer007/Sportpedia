using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using EFDatabase;

namespace Application.Services
{
    public class CompetitionType_services
    {
        public Type_of_competition Check_existing(string typeName)
        {
            using (var ctx = new Context())
            {
                return ctx.Type_of_competitions.FirstOrDefault(s => s.Type_name == typeName);
            }
       }

        public void New_event(Type_of_competition model)
        {
            using (var ctx = new Context())
            {
                ctx.Type_of_competitions.Add(model);
                ctx.SaveChanges();
            }
        }

        public BindingList<Type_of_competition> Competition_type_list()
        {
            BindingList<Type_of_competition> list = new BindingList<Type_of_competition>();
            using (var ctx = new Context())
            {
                List<Type_of_competition> lista = ctx.Type_of_competitions.ToList();
                foreach (var tip in lista)
                {
                    list.Add(tip);
                }
            }
            return list;
        }
    }
}