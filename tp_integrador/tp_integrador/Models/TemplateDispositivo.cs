using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace tp_integrador.Models
{
	public class TemplateDispositivo
	{
		public int ID;
		public string Dispositivo;
		public string concreto;
		public string inteligente;
		public string bajoconsumo;
		public double consumo;
		public string getNombreEntero() { return Dispositivo + " " + concreto; }
		public TemplateDispositivo(int ID, string dispositivo, string concreto, string inteligente, string bajoconsumo, double consumo)
		{
			Dispositivo = dispositivo;
			this.ID = ID;
			this.concreto = concreto;
			this.inteligente = inteligente;
			this.bajoconsumo = bajoconsumo;
			this.consumo = consumo;
		}
	}
}