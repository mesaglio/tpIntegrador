using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace tp_integrador.Models
{
    public class DAO_t_dispositivostemplate
    {
        public List<templateDisp> templateDisps; 
        public templateDisp Searchtemplatebyid(int id)
        {
            foreach (templateDisp tem in templateDisps)
            {
                if (tem.ID == id) return tem;
            }
            return null;
        }
        public DAO_t_dispositivostemplate()
        {
           List<templateDisp> item = new List<templateDisp>();
            item.Add(new templateDisp(1,"aire acondicionado", "3500 frigorias", "si", "no", 1.163));
            item.Add(new templateDisp(2,"aire acondicionado", "2200 frigorias", "si", "si", 1.013));
            item.Add(new templateDisp(3,"televisor", "color de tubo fluorecente de 21", "no", "no", 0.075));
            item.Add(new templateDisp(4,"televisor", "color de tubo fluorecente de 29 a 34", "no", "no", 0.175));
            item.Add(new templateDisp(5,"televisor", "LCD de 40", "no", "no", 0.18));
            item.Add(new templateDisp(6,"televisor", "LED 24", "si", "si", 0.004));
            item.Add(new templateDisp(7,"televisor", "LED 32", "si", "si", 0.055));
            item.Add(new templateDisp(8,"televisor", "LED 40", "si", "si", 0.08));
            item.Add(new templateDisp(9,"heladera", "con freexer", "si", "si", 0.09));
            item.Add(new templateDisp(10,"heladera", "sin freexer", "si", "si", 0.075));
            item.Add(new templateDisp(11,"lavarropas", "automatico 5 kg con calentamiento de agua", "no", "no", 0.875));
            item.Add(new templateDisp(12,"lavarropas", "automatico 5kg", "si", "si", 0.175));
            item.Add(new templateDisp(13,"lavarropas", "semi-automatico 5kg", "no", "si", 0.1275));
            item.Add(new templateDisp(14,"ventilador", "de pie", "no", "si", 0.09));
            item.Add(new templateDisp(15,"ventilador", "de techo", "no", "si", 0.06));
            item.Add(new templateDisp(16,"lampara", "halogenas de 40 w", "si", "no", 0.04));
            item.Add(new templateDisp(17,"lampara", "halogenas de 60 w", "si", "no", 0.06));
            item.Add(new templateDisp(18,"lampara", "halogenas de 100 w", "si", "no", 0.015));
            item.Add(new templateDisp(19,"lampara", "de 11 w", "si", "si", 0.011));
            item.Add(new templateDisp(20,"lampara", "de 11 w", "si", "si", 0.011));
            item.Add(new templateDisp(21,"lampara", "de 15 w", "si", "si", 0.015));
            item.Add(new templateDisp(22,"lampara", "de 20 w", "si", "si", 0.02));
            item.Add(new templateDisp(23,"pc", "de escritorio", "si", "si", 0.04));
            item.Add(new templateDisp(24,"microondas", "convencional", "no", "si", 0.64));
            item.Add(new templateDisp(25,"plancha", "a vapor", "no", "si", 0.75));
            this.templateDisps = item;

        }
    }

    public class templateDisp
    {
        public int ID;
        public string Dispositivo;
        public string concreto;
        public string inteligente;
        public string bajoconsumo;
        public double consumo;
        public string getNombreEntero() { return Dispositivo + " " + concreto; }
        public templateDisp(int ID ,string dispositivo, string concreto, string inteligente, string bajoconsumo, double consumo)
        {
            Dispositivo = dispositivo;
            this.ID = ID;
            this.concreto = concreto;
            this.inteligente = inteligente;
            this.bajoconsumo = bajoconsumo;
            this.consumo = consumo;
        }
    }
}