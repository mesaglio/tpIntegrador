using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;


namespace tp_integrador.Models
{
    public class Sensor
    {   
        [Key]
        public int idSensor { get; set; }
        public string TipoSensor { get; set; }
        public int Magnitud { get => Magnitud; set { Magnitud = value; Notify(); } }
        private List<Regla> observador;



        public void AgregarRegla(Regla reg) => observador.Add(reg);
        public void QuitarRegla(Regla reg) => observador.Remove(reg);

        // llamar en el set magnitud
        private void Notify()
        {
            foreach (Regla elem in observador) { elem.Cambio(mag: Magnitud);}
        }
    }


}