using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace tp_integrador.Models
{
	public class DispositivoGenerico
	{
		public int ID { get; set; }
		public string Dispositivo { get; set; }
		public string Concreto { get; set; }
		public bool Inteligente { get; set; }
		public bool Bajoconsumo { get; set; }
		public double Consumo { get; set; }
		
		public DispositivoGenerico() { }

		public DispositivoGenerico(int ID, string dispositivo, string concreto, bool inteligente, bool bajoconsumo, double consumo)
		{
			Dispositivo = dispositivo;
			this.ID = ID;
			this.Concreto = concreto;
			this.Inteligente = inteligente;
			this.Bajoconsumo = bajoconsumo;
			this.Consumo = consumo;
		}

		public string getNombreEntero() => Dispositivo + " " + Concreto;
		public string getNombreEnteroConEtiqueta() => (Inteligente ? "[Inteligente]" : "[Estandar]") + " " + Dispositivo + " " + Concreto;
	}
}