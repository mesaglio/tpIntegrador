using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace tp_integrador.Models
{
    public class Sensor
    {
        public string TipoSensor { get; set; }
        public int Magnitud { get; set; }
        private List<Regla> observador;



        public void AgregarRegla(Regla reg) => observador.Add(reg);
        public void QuitarRegla(Regla reg) => observador.Remove(reg);
    }


}