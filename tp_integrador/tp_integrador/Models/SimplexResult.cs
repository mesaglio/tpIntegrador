using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace tp_integrador.Models
{
	public class SimplexResult
	{
		public string Maximo { get; set; }
		public List<Data> Valores { get; set; }

		public SimplexResult(string[] respuesta, List<Dispositivo> listaDisp)
		{
			Maximo = respuesta[0];
			Valores = new List<Data>();

			var cantDisp = listaDisp.Count;
			Dispositivo dispo = null;
			
			for (int i = 1; i < respuesta.Length; i++)
			{
				dispo = listaDisp[cantDisp - i];
				Valores.Add(new Data(dispo.Nombre, dispo.Numero, respuesta[i]));				
			}

		}

		public class Data
		{
			public string Nombre { get; set; }
			public int Numero { get; set; }
			public string Consumo { get; set; }

			public Data(string nombre, int numero, string consumo)
			{
				Nombre = nombre;
				Numero = numero;
				Consumo = consumo;
			}
		}
	}
}