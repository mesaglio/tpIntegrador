using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace tp_integrador.Models
{
    public class Dispositivo
    {
        public string Nombre { get; set; }
        public byte Consumo;


        public Dispositivo(string nombre, byte consumo)
        {
            Nombre = nombre;
            Consumo = consumo;
        }

    }
}