using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace tp_integrador.Models
{
    public class Dispositivo
    {
        public string Nombre { get; set; }
        public double Consumo;
        public bool EsInteligente = false;

        public Dispositivo(string nombre, double consumo)
        {
            Nombre = nombre;
            Consumo = consumo;
            
        }

       

    }
}