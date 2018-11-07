using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Headers;

namespace tp_integrador.Models
{
    public class SIMPLEX
    {
		private readonly string API = "https://dds-simplexapi.herokuapp.com/consultar";		
		private List<Tuple<string, double, double>> datosRestricciones; //dispositivo,minimo,maximo

		public SIMPLEX()
		{
			datosRestricciones = new List<Tuple<string, double, double>>();
			datosRestricciones.Add(Tuple.Create("Aire-Acondicionado", 90d, 360d));
			datosRestricciones.Add(Tuple.Create("Lámpara", 90d, 360d));
			datosRestricciones.Add(Tuple.Create("Televisor", 90d, 360d));
			datosRestricciones.Add(Tuple.Create("Lavarropas", 6d, 30d));
			datosRestricciones.Add(Tuple.Create("PC", 60d, 360d));
			datosRestricciones.Add(Tuple.Create("Microondas", 3d, 15d));
			datosRestricciones.Add(Tuple.Create("Plancha", 3d, 30d));
			datosRestricciones.Add(Tuple.Create("Ventilador", 120d, 360d));
		}
				
        public string[] GetSimplexData(string postData)
        {
			HttpResponseMessage response = null;
			using (var client = new HttpClient())
			{
				response = client.PostAsync(API, new StringContent(postData, Encoding.UTF8, "application/json")).Result;
				var resultado = response.Content.ReadAsStringAsync().Result;
				resultado = resultado.Substring(1, resultado.Length - 2);

				return resultado.Split(',');				
			}
		}

		public string CrearConsulta(List<Dispositivo> dispositivos)
		{			
			var json = new JSONObject(dispositivos);
			json.Restrictions.Add(new Restriction(dispositivos));

			var cantidad = dispositivos.Count();
			for(int i = 0; i < cantidad; i++)
			{
				foreach (var res in datosRestricciones)
				{
					if (res.Item1 == dispositivos[i].Nombre.Split(' ')[0])
					{
						json.Restrictions.Add(new Restriction(">=", res.Item2, i, cantidad));
						json.Restrictions.Add(new Restriction("<=", res.Item3, i, cantidad));
					}
				}
			}
			
			return JsonConvert.SerializeObject(json).ToLower();
		}

		//Clases Para Crear JSON
		private class Restriction
		{
			public List<double> Values { get; set; }
			public string Operator { get; set; }

			public Restriction(string operador, double valor, int pos, int cant)
			{
				Operator = operador;
				Values = new List<double>();
				for (int i = 0; i <= cant; i++)
				{
					Values.Add(0);
				}
				Values[0] = valor;
				Values[pos + 1] = 1;

			}

			public Restriction(List<Dispositivo> lista)
			{
				Values = new List<double>();
				Operator = "<=";
				Values.Add(612);
				foreach (var item in lista)
				{
					Values.Add(item.Consumo);
				}
			}
		}

		private class JSONObject
		{
			public List<int> Vars { get; set; }
			public List<Restriction> Restrictions { get; set; }

			public JSONObject(List<Dispositivo> lista)
			{
				Vars = new List<int>();
				Restrictions = new List<Restriction>();
				foreach (var item in lista)
				{
					Vars.Add(1);
				}
			}
		}
	}
}