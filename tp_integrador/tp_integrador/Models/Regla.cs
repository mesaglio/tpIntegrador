using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace tp_integrador.Models
{
    public class Regla
    {
        public int idRegla { get; set; }
		public int idSensor { get; set; }
		public int Valor { get; set; }
        List<Actuador> Actuadores { get; set; }

		public Regla(int regla, int sensor, int valor, List<Actuador> actuadores)
		{
			idRegla = regla;
			idSensor = sensor;
			Valor = valor;
			Actuadores = actuadores;
		}

		public void Cambio(int mag)
        {	
			foreach (Actuador ac in Actuadores)
			{
				if (Valor == mag) ac.Actuar();
			}
        }
    }
}