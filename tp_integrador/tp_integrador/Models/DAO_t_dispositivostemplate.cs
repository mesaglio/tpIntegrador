using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace tp_integrador.Models
{
    public class DAO_t_dispositivostemplate
    {
        public List<TemplateDispositivo> templateDisps; 
        public TemplateDispositivo Searchtemplatebyid(int id)
        {
            foreach (TemplateDispositivo tem in templateDisps)
            {
                if (tem.ID == id) return tem;
            }
            return null;
        }

        public DAO_t_dispositivostemplate()
        {
           List<TemplateDispositivo> item = new List<TemplateDispositivo>();
            item.Add(new TemplateDispositivo(1,"Aire-Acondicionado", "3500 frigorias", "si", "no", 1.163));
            item.Add(new TemplateDispositivo(2,"Aire-Acondicionado", "2200 frigorias", "si", "si", 1.013));
            item.Add(new TemplateDispositivo(3,"Televisor", "color de tubo fluorecente de 21", "no", "no", 0.075));
            item.Add(new TemplateDispositivo(4,"Televisor", "color de tubo fluorecente de 29 a 34", "no", "no", 0.175));
            item.Add(new TemplateDispositivo(5,"Televisor", "LCD de 40", "no", "no", 0.18));
            item.Add(new TemplateDispositivo(6,"Televisor", "LED 24", "si", "si", 0.004));
            item.Add(new TemplateDispositivo(7,"Televisor", "LED 32", "si", "si", 0.055));
            item.Add(new TemplateDispositivo(8,"Televisor", "LED 40", "si", "si", 0.08));
            item.Add(new TemplateDispositivo(9,"Heladera", "con freexer", "si", "si", 0.09));
            item.Add(new TemplateDispositivo(10,"Heladera", "sin freexer", "si", "si", 0.075));
            item.Add(new TemplateDispositivo(11,"Lavarropas", "automatico 5 kg con calentamiento de agua", "no", "no", 0.875));
            item.Add(new TemplateDispositivo(12,"Lavarropas", "automatico 5kg", "si", "si", 0.175));
            item.Add(new TemplateDispositivo(13,"Lavarropas", "semi-automatico 5kg", "no", "si", 0.1275));
            item.Add(new TemplateDispositivo(14,"Ventilador", "de pie", "no", "si", 0.09));
            item.Add(new TemplateDispositivo(15,"Ventilador", "de techo", "no", "si", 0.06));
            item.Add(new TemplateDispositivo(16,"Lampara", "halogenas de 40 w", "si", "no", 0.04));
            item.Add(new TemplateDispositivo(17,"Lampara", "halogenas de 60 w", "si", "no", 0.06));
            item.Add(new TemplateDispositivo(18,"Lampara", "halogenas de 100 w", "si", "no", 0.015));
            item.Add(new TemplateDispositivo(19,"Lampara", "de 11 w", "si", "si", 0.011));
            item.Add(new TemplateDispositivo(20,"Lampara", "de 11 w", "si", "si", 0.011));
            item.Add(new TemplateDispositivo(21,"Lampara", "de 15 w", "si", "si", 0.015));
            item.Add(new TemplateDispositivo(22,"Lampara", "de 20 w", "si", "si", 0.02));
            item.Add(new TemplateDispositivo(23,"PC", "de escritorio", "si", "si", 0.04));
            item.Add(new TemplateDispositivo(24,"Microondas", "convencional", "no", "si", 0.64));
            item.Add(new TemplateDispositivo(25,"Plancha", "a vapor", "no", "si", 0.75));
            this.templateDisps = item;

        }
    }    
}