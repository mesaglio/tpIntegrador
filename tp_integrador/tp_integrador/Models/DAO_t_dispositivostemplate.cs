using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace tp_integrador.Models
{
    public class DAO_t_dispositivostemplate
    {
        public List<templateDisp> templateDisps; 
        public DAO_t_dispositivostemplate()
        {
           List<templateDisp> item = new List<templateDisp>();
            item.Add(new templateDisp("aire acondicionado", "3500 frigorias", "si", "no", 1.163));
            item.Add(new templateDisp("aire acondicionado", "2200 frigorias", "si", "si", 1.013));
            item.Add(new templateDisp("televisor", "color de tubo fluorecente de 21", "no", "no", 0.075));
            item.Add(new templateDisp("televisor", "color de tubo fluorecente de 29 a 34", "no", "no", 0.175));
            item.Add(new templateDisp("televisor", "LCD de 40", "no", "no", 0.18));
            item.Add(new templateDisp("televisor", "LED 24", "si", "si", 0.004));
            item.Add(new templateDisp("televisor", "LED 32", "si", "si", 0.055));
            item.Add(new templateDisp("televisor", "LED 40", "si", "si", 0.08));
            item.Add(new templateDisp("heladera", "con freexer", "si", "si", 0.09));
            item.Add(new templateDisp("heladera", "sin freexer", "si", "si", 0.075));
            item.Add(new templateDisp("lavarropas", "automatico 5 kg con calentamiento de agua", "no", "no", 0.875));
            item.Add(new templateDisp("lavarropas", "automatico 5kg", "si", "si", 0.175));
            item.Add(new templateDisp("lavarropas", "semi-automatico 5kg", "no", "si", 0.1275));
            item.Add(new templateDisp("ventilador", "de pie", "no", "si", 0.09));
            item.Add(new templateDisp("ventilador", "de techo", "no", "si", 0.06));
            item.Add(new templateDisp("lampara", "halogenas de 40 w", "si", "no", 0.04));
            item.Add(new templateDisp("lampara", "halogenas de 60 w", "si", "no", 0.06));
            item.Add(new templateDisp("lampara", "halogenas de 100 w", "si", "no", 0.015));
            item.Add(new templateDisp("lampara", "de 11 w", "si", "si", 0.011));
            item.Add(new templateDisp("lampara", "de 11 w", "si", "si", 0.011));
            item.Add(new templateDisp("lampara", "de 15 w", "si", "si", 0.015));
            item.Add(new templateDisp("lampara", "de 20 w", "si", "si", 0.02));
            item.Add(new templateDisp("pc", "de escritorio", "si", "si", 0.04));
            item.Add(new templateDisp("microondas", "convencional", "no", "si", 0.64));
            item.Add(new templateDisp("plancha", "a vapor", "no", "si", 0.75));
            this.templateDisps = item;

        }
    }

    public class templateDisp
    {
        public string Dispositivo;
        public string concreto;
        public string inteligente;
        public string bajoconsumo;
        public double consumo;
        public string getNombreEntero() { return Dispositivo + concreto; }
        public templateDisp(string dispositivo, string concreto, string inteligente, string bajoconsumo, double consumo)
        {
            Dispositivo = dispositivo;
            this.concreto = concreto;
            this.inteligente = inteligente;
            this.bajoconsumo = bajoconsumo;
            this.consumo = consumo;
        }
    }
}