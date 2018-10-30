using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace tp_integrador.Models
{
	public class Sensor
	{
		//TODO: WIP
		public int idSensor { get; set; }
		public string TipoSensor { get; set; }
		public int idCliente { get; set; }
		public int Magnitud { get => Magnitud; set { Magnitud = value; Notify(); } }
		private List<Regla> observador = new List<Regla>();

		public Sensor(int id, string detalle, int cliente, int magnitud, List<Regla> observadores)
		{
			idSensor = id;
			TipoSensor = detalle;
			idCliente = cliente;
			observador = observadores;
			Magnitud = magnitud;
		}

        public void AgregarRegla(Regla reg) => observador.Add(reg);
        public void QuitarRegla(Regla reg) => observador.Remove(reg);

        private void Notify()
        {
            foreach (Regla elem in observador) { elem.Cambio(mag: Magnitud);}
        }
    }


}