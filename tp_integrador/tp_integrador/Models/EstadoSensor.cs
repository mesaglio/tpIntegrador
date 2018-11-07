using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace tp_integrador.Models
{
	public class EstadoSensor
	{
		public int IdSensor { get; set; }
		public int Magnitud { get; set; }

		public EstadoSensor(int id, int magnitud)
		{
			IdSensor = id;
			Magnitud = magnitud;
		}
	}
}