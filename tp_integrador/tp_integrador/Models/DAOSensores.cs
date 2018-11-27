using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace tp_integrador.Models
{
	public class DAOSensores
	{
		private static DAOSensores instancia;
		public List<Sensor> ListaSensores { get; set; }

		public DAOSensores()
		{
			ListaSensores = new List<Sensor>();
		}

		public static DAOSensores Instancia
		{
			get
			{
				if (instancia == null) instancia = new DAOSensores();
				return instancia;
			}
		}

		public void InitialLoad()
		{
			if (ListaSensores.Count != 0) return;

			ListaSensores = ORM.Instancia.GetAllSensores();
		}

		public void CargarSensor(Sensor sensor)
		{
			ListaSensores.Add(sensor);
		}

		public List<Sensor> FindAllFromCliente(int idCliente)
		{
			var lista = new List<Sensor>();
			foreach (var s in ListaSensores) if (s.idCliente == idCliente) lista.Add(s);

			return lista;
		}

		public List<Regla> FindReglasCliente(int idCliente)
		{
			var lista = new List<Regla>();

			var sensores = FindAllFromCliente(idCliente);
			foreach (var sensor in sensores)
			{
				lista.AddRange(sensor.Observadores.FindAll(x => !lista.Contains(x)));
			}

			return lista;
		}

		public Regla GetRegla(int idRegla)
		{
			var sensor = ListaSensores.Find(x => x.Observadores.Exists(z => z.idRegla == idRegla));
			var regla = sensor.Observadores.Find(x => x.idRegla == idRegla);

			return regla;
		}
	}
}