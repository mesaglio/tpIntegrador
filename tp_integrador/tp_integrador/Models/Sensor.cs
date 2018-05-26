using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace tp_integrador.Models
{
    public class Sensor
    {
        public string tipoSensor { get; set; }
        public int magnitud { get; set; }
       private List<Regla> observador;



        public void agregarregla(Regla reg) => observador.Add(reg);
        public void removerregla(Regla reg) => observador.Remove(reg);
    }


}