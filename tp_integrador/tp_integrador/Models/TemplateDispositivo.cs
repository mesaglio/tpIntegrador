using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace tp_integrador.Models
{
	public class TemplateDispositivo
	{
		public int ID { get; set; }
		public string Dispositivo { get; set; }
		public string concreto { get; set; }
		public string inteligente { get; set; }
		public string bajoconsumo { get; set; }
		public double consumo { get; set; }
				
		public TemplateDispositivo(int ID, string dispositivo, string concreto, string inteligente, string bajoconsumo, double consumo)
		{
			Dispositivo = dispositivo;
			this.ID = ID;
			this.concreto = concreto;
			this.inteligente = inteligente;
			this.bajoconsumo = bajoconsumo;
			this.consumo = consumo;
		}

		public string getNombreEntero() => Dispositivo + " " + concreto;
	}
}