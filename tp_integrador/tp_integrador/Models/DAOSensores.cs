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

		public Sensor GetSensor(int idSensor)
		{
			return ListaSensores.Find(x => x.idSensor == idSensor);
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

		public List<Actuador> FindActuadoresCliente(int idCliente)
		{
			var lista = new List<Actuador>();
			var reglas = FindReglasCliente(idCliente);

			foreach (var regla in reglas)
			{
				lista.AddRange(regla.Actuadores.FindAll(x => !lista.Contains(x)));
			}			

			return lista;
		}

		public Actuador GetActuador(int idActuador)
		{
			var sensor = ListaSensores.Find(x => x.Observadores.Exists(z => z.Actuadores.Exists(y => y.IdActuador == idActuador)));
			var actuador = sensor.Observadores.Find(x => x.Actuadores.Exists(z => z.IdActuador == idActuador)).Actuadores.Find(y => y.IdActuador == idActuador);

			return actuador;
		}

		public void CargarNuevaRegla(Regla regla)
		{
			if (regla.idRegla != 0) return;

			ORM.Instancia.Insert(regla);			

			foreach (var actuador in regla.Actuadores)
			{
				if (!actuador.Reglas.Contains(regla.idRegla)) actuador.Reglas.Add(regla.idRegla);
			}

			var sensor = ListaSensores.Find(x => x.idSensor == regla.idSensor);
			sensor.Observadores.Add(regla);
		}

		public void CargarNuevoActuador(Actuador actuador)
		{
			if (actuador.IdActuador != 0) return;
			
			var reglas = FindReglasCliente(actuador.IdCliente).FindAll(x => actuador.Reglas.Contains(x.idRegla));

			foreach (var regla in reglas)
			{
				regla.Actuadores.Add(actuador);
			}

			ORM.Instancia.Insert(actuador);
		}

		public void ModificarSensor(Sensor modificado)
		{
			var sensor = GetSensor(modificado.idSensor);

			sensor.TipoSensor = modificado.TipoSensor;

			ORM.Instancia.Update(sensor);
		}

		public bool ModificarRegla(Regla modificada)
		{
			var regla = GetRegla(modificada.idRegla);

			var agregar = modificada.Actuadores.FindAll(x => !regla.Actuadores.Contains(x));
			var quitar = regla.Actuadores.FindAll(x => !modificada.Actuadores.Contains(x));

			foreach (var actuador in quitar)
			{				
				if (actuador.Reglas.Count == 1) return false;
			}

			if (regla.idSensor != modificada.idSensor)
			{
				GetSensor(regla.idSensor).QuitarRegla(regla);
				regla.idSensor = modificada.idSensor;
				GetSensor(regla.idSensor).AgregarRegla(regla);
			}

			regla.Detalle = modificada.Detalle;
			regla.Operador = modificada.Operador;
			regla.Valor = modificada.Valor;
			regla.Accion = modificada.Accion;
						
			agregar.ForEach(x => x.AgregarRegla(regla.idRegla));
			quitar.ForEach(x => x.QuitarRegla(regla.idRegla));
			
			regla.Actuadores = modificada.Actuadores;
			ORM.Instancia.Update(regla);
			return true;
		}

		public void ModificarActuador(Actuador modificado)
		{
			var actuador = GetActuador(modificado.IdActuador);
						
			var rAgregar = modificado.Reglas.FindAll(x => !actuador.Reglas.Contains(x));
			var rQuitar = actuador.Reglas.FindAll(x => !modificado.Reglas.Contains(x));
			
			actuador.ActuadorTipo = modificado.ActuadorTipo;

			rAgregar.ForEach(x => GetRegla(x).AgregarActuador(actuador));
			rQuitar.ForEach(x => GetRegla(x).QuitarActuador(actuador));

			actuador.Reglas = modificado.Reglas;
			actuador.Dispositivos = modificado.Dispositivos;

			ORM.Instancia.Update(actuador);
		}

		public int ClienteIDFrom(int idRegla)
		{
			return ListaSensores.Find(x => x.Observadores.Exists(z => z.idRegla == idRegla)).idCliente;
		}

		public void EliminarActuador(Actuador actuador)
		{
			var reglas = FindReglasCliente(actuador.IdCliente).FindAll(x => x.Actuadores.Contains(actuador));

			reglas.ForEach(x => x.QuitarActuador(actuador));

			ORM.Instancia.Delete(actuador);
		}

		public void EliminarRegla(Regla regla)
		{
			var actuadoresSolos = regla.Actuadores.FindAll(x => x.Reglas.Count == 1);

			foreach (var actua in actuadoresSolos)
			{
				ORM.Instancia.Delete(actua);
				regla.QuitarActuador(actua);
			}

			regla.Actuadores.ForEach(x => x.QuitarRegla(regla.idRegla));
			GetSensor(regla.idSensor).QuitarRegla(regla);

			ORM.Instancia.Delete(regla);
		}

		public void EliminarSensor(Sensor sensor)
		{
			sensor.Observadores.ToList().ForEach(x => EliminarRegla(x));

			ListaSensores.Remove(sensor);

			ORM.Instancia.Delete(sensor);
		}

		public void QuitarDispositivoDeActuadores(Inteligente dispositivo)
		{
			var actuadoresDispositivo = FindActuadoresCliente(dispositivo.IdCliente).FindAll(x => x.Dispositivos.Contains(dispositivo));
			actuadoresDispositivo.ForEach(x => x.QuitarDispositivo(dispositivo));						
		}
	}
}