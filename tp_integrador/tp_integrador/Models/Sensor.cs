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
		public int Magnitud { get => Magnitud; set { ORM.Instancia.Insert(new EstadoSensor(idSensor, Magnitud)); Magnitud = value; Notify(); } }
		public List<Regla> Observadores { get; set; }

		public Sensor(int id, string detalle, int cliente, int magnitud, List<Regla> observadores)
		{
			idSensor = id;
			TipoSensor = detalle;
			idCliente = cliente;
			Observadores = observadores;
			Magnitud = magnitud;
		}

        public void AgregarRegla(Regla reg) => Observadores.Add(reg);
        public void QuitarRegla(Regla reg) => Observadores.Remove(reg);

        private void Notify()
        {
            foreach (Regla elem in Observadores) { elem.Cambio(mag: Magnitud);}
        }
    }


}