using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using Newtonsoft.Json;

namespace tp_integrador.Models
{
    public class SIMPLEX
    {
		private readonly string API = "https://dds-simplexapi.herokuapp.com/consultar";
		
		private List<Tuple<string, double, double>> datosRestricciones; //dispositivo,minimo,maximo
		public SIMPLEX()
		{
			datosRestricciones = new List<Tuple<string, double, double>>();
			datosRestricciones.Add(Tuple.Create("Aire Acondicionado", 90d, 360d));
			datosRestricciones.Add(Tuple.Create("Lámpara", 90d, 360d));
			datosRestricciones.Add(Tuple.Create("Televisor", 90d, 360d));
			datosRestricciones.Add(Tuple.Create("Lavarropas", 6d, 30d));
			datosRestricciones.Add(Tuple.Create("Computadora", 60d, 360d));
			datosRestricciones.Add(Tuple.Create("Microondas", 3d, 15d));
			datosRestricciones.Add(Tuple.Create("Plancha", 3d, 30d));
			datosRestricciones.Add(Tuple.Create("Ventilador", 120d, 360d));
		}
				
        public string Simplex(string postData)
        {
            WebRequest request = WebRequest.Create(API);
            request.Method = "POST";

            byte[] byteArray = Encoding.UTF8.GetBytes(postData);
            // Set the ContentType property of the WebRequest.  
           // request.ContentType = "application/x-www-form-urlencoded";
            // Set the ContentLength property of the WebRequest.  
            request.ContentLength = byteArray.Length;
            // Get the request stream.  
            Stream dataStream = request.GetRequestStream();
            // Write the data to the request stream.  
            dataStream.Write(byteArray, 0, byteArray.Length);
            // Close the Stream object.  
            dataStream.Close();
            // Get the response.  
            WebResponse response = request.GetResponse();
            // Display the status.  
            Console.WriteLine(((HttpWebResponse)response).StatusDescription);
            // Get the stream containing content returned by the server.  
            dataStream = response.GetResponseStream();
            // Open the stream using a StreamReader for easy access.  
            StreamReader reader = new StreamReader(dataStream);
            // Read the content.  
            string responseFromServer = reader.ReadToEnd();
            reader.Close();
            dataStream.Close();
            response.Close();
            return responseFromServer;
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
					if (res.Item1 == dispositivos[i].Nombre.Substring(0, dispositivos[i].Nombre.Length-2))
					{
						json.Restrictions.Add(new Restriction(">=", res.Item2, i, cantidad));						
						json.Restrictions.Add(new Restriction("<=", res.Item3, i, cantidad));
					}
				}				
			}

			return JsonConvert.SerializeObject(json);
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