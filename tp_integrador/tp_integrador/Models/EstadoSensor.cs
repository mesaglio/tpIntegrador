using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace tp_integrador.Models
{
	public class EstadoSensor
	{
		public int IdEstadoSensor { get; set; }
		public int IdSensor { get; set; }
		public int Magnitud { get; set; }

		public EstadoSensor(int idSensor, int magnitud)
		{
			IdSensor = idSensor;
			Magnitud = magnitud;
		}

		public EstadoSensor(int idEstadoSensor, int idSensor, int magnitud)
		{
			IdEstadoSensor = idEstadoSensor;
			IdSensor = idSensor;
			Magnitud = magnitud;
		}
	}
}