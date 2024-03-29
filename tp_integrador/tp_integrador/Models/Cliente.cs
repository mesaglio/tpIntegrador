﻿using Gmap.net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;


namespace tp_integrador.Models
{
    public class Cliente : Usuarios
    {
        public string Telefono { get; set; }
        public DateTime AltaServicio { get; set; }
        public Categoria Categoria { get; set; }
        public string Documento_tipo { get; set; }
        public string Documento_numero { get; set; }
        public int Puntos { get; set; }
        public bool AutoSimplex { get; set; }
        public Location Coordenadas { get; set; }

        public List<Dispositivo> dispositivos;

        public List<Dispositivo> DispositivosInteligentes => dispositivos.FindAll(i => i.EsInteligente);
        public List<Dispositivo> DispositivosEstandar => dispositivos.FindAll(i => !i.EsInteligente);
		//TODO: verificar si funciona estandar

		public Cliente() { }

        public Cliente(int id, string name, string lastname, string home, Location coords, string user, string clave, string phone, DateTime alta, Categoria categ, string doc_t, string doc_n, bool simplex, List<Dispositivo> disp = null) : base(id, name, lastname, home, user, clave)
        {
            Coordenadas = coords;
            Telefono = phone;
            Categoria = categ;
            Documento_tipo = doc_t;
            Documento_numero = doc_n;
            AltaServicio = alta;
            AutoSimplex = simplex;
            dispositivos = new List<Dispositivo>();
            if (disp != null) dispositivos = disp;
        }

        public int CantDispositivos()
        {
            return dispositivos.Count;
        }

        public bool EstasEncendido(Inteligente aparato)
        {
            return aparato.Encendido();
        }

        public int CantEncendidos()
        {
            int i = 0;
            foreach (Inteligente aparato in dispositivos.OfType<Inteligente>())
            {
                if (aparato.Encendido()) { i++; }
            }
            return i;
        }

        public bool AlgunoEncendido()
        {
            return (CantEncendidos() != 0);
        }

        public int CantApagados()
        {
            return dispositivos.OfType<Inteligente>().Count() - CantEncendidos();
        }


        public void ApagarDispositivo(Inteligente aparato)
        {
            aparato.Apagar();
        }

        public void EncenderDispositivo(Inteligente aparato)
        {
            aparato.Encender();
        }

        public void ModoAhorroDispositivo(Inteligente aparato)
        {
            aparato.ModoAhorro();
        }

        public double GetEstimado(Estandar aparato)
        {
            return aparato.ConsumoEstimado();
        }

        public void UsoDiario(Estandar aparato, byte horas)
        {	
            aparato.SetUsoDiario(horas);
        }

        public void NuevoDispositivoInteligente(int idDisp, string nombre, double consumo, bool bajoconsumo)
        {
			Inteligente nuevo = new Inteligente(idDisp, idUsuario, CalcularNumero(nombre), nombre, consumo, bajoconsumo, 0, DateTime.Now, false);

			dispositivos.Add(nuevo);
			DAODispositivo.Instancia.CargarDispositivo(nuevo);
			ORM.Instancia.Insert(nuevo);			

			Puntos += 15;
			ORM.Instancia.Update(this);
		}

        public void NuevoDispositivoEstandar(int idDisp, string nombre, double consumo, bool bajoconsumo, byte usoPromedio)
        {
			Estandar nuevo = new Estandar(idDisp, idUsuario, CalcularNumero(nombre), nombre, consumo, bajoconsumo, usoPromedio);

			dispositivos.Add(nuevo);
			DAODispositivo.Instancia.CargarDispositivo(nuevo);
			ORM.Instancia.Insert(nuevo);			
        }

        public void ConvertirAInteligente(Estandar aparato)
        {
            Inteligente adaptado = new Inteligente(aparato.IdDispositivo, idUsuario, aparato.Numero, aparato.Nombre, aparato.Consumo, aparato.BajoConsumo, 0, DateTime.Now, true);
            dispositivos.Remove(aparato);
            dispositivos.Add(adaptado);
			DAODispositivo.Instancia.ReemplazarPorAdaptado(adaptado);
			ORM.Instancia.Update(adaptado);

            Puntos += 10;
			ORM.Instancia.Update(this);
        }

        public void AgregarDispositivoDesdeJson(Dispositivo dispositivo)
        {
			Inteligente inteligente;
			Estandar estandar;

			if (dispositivo.EsInteligente)
			{
				inteligente = (Inteligente)dispositivo;
				NuevoDispositivoInteligente(inteligente.IdDispositivo, inteligente.Nombre, inteligente.Consumo, inteligente.BajoConsumo);
			} else
			{
				estandar = (Estandar)dispositivo;
				NuevoDispositivoEstandar(estandar.IdDispositivo, estandar.Nombre, estandar.Consumo, estandar.BajoConsumo, estandar.usoDiario);
			}			
		}

        private int CalcularNumero(string nombre)
        {
            return (dispositivos.FindAll(x => x.Nombre == nombre)).Count + 1;
        }

		public dynamic BuscarDispositivo(int idDispositivo, int idCliente, int numero)
		{
			return dispositivos.Find(x => (x.IdDispositivo == idDispositivo) && (x.IdCliente == idCliente) && (x.Numero == numero));
		}

		public double ConsumoActual()
		{
			double total = 0;

			foreach (var disp in dispositivos.OfType<Inteligente>())
			{
				total += disp.ConsumoEnEstadoActual();
			}

			return total;
		}

		public double ConsumoEstandarDiario()
		{
			double total = 0;

			foreach (var disp in dispositivos.OfType<Estandar>())
			{
				total += GetEstimado(disp);
			}

			return total;
		}

		public PeriodoData PeriodoActual()
		{
			var periodo = new PeriodoData();
			periodo.PeriodoActual();

			return periodo;
		}

		public PeriodoData ConsumoDelPeriodoActual()
		{
			var periodo = PeriodoActual();

			return ConsumoDelPeriodo(periodo);
		}

		private PeriodoData ConsumoDelPeriodo(PeriodoData periodo)
		{
			double total = 0;

			foreach (var disp in dispositivos.OfType<Inteligente>())
			{
				total += disp.ConsumoEnElPeriodo(periodo);
			}

			var eliminadosEnElPeriodo = ORM.Instancia.GetDispositivosEliminadosEnFrom(periodo, idUsuario);

			foreach (var disp in eliminadosEnElPeriodo.OfType<Inteligente>())
			{
				total += disp.ConsumoEnElPeriodo(periodo);
			}

			periodo.Consumo = total;
			return periodo;
		}

		public PeriodoData ConsumoDelPeriodo(int numero, int anio)
		{
			var periodo = new PeriodoData();
			periodo.Periodo((Byte)numero, anio);

			return ConsumoDelPeriodo(periodo);
		}

		public PeriodoData ConsumoDelPeriodo(DateTime desde, DateTime hasta)
        {
            var periodo = new PeriodoData();
            periodo.Periodo((byte)desde.Month, desde.Year);

			return ConsumoDelPeriodo(periodo);
        }

        public List<Sensor> MisSensores()
		{
			return DAOSensores.Instancia.FindAllFromCliente(idUsuario);
		}

		public List<Regla> MisReglas()
		{
			return DAOSensores.Instancia.FindReglasCliente(idUsuario);
		}

		public List<Actuador> MisActuadores()
		{
			return DAOSensores.Instancia.FindActuadoresCliente(idUsuario);
		}

		public bool NuevoSensor(Sensor nuevo)
		{			
			var dbSensor = ORM.Instancia.GetSensor(idUsuario, nuevo.TipoSensor);
			if (dbSensor != null) return false;

			var sensor = new Sensor(0, nuevo.TipoSensor, idUsuario, 0, new List<Regla>());
			ORM.Instancia.Insert(sensor);
			
			dbSensor = ORM.Instancia.GetSensor(idUsuario, nuevo.TipoSensor);
			DAOSensores.Instancia.CargarSensor(dbSensor);
			return true;
		}

		public bool NuevaRegla(Regla nueva)
		{
			var idRegla = ORM.Instancia.GetReglaID(nueva.idSensor, nueva.Detalle, nueva.Valor, nueva.Operador, nueva.Accion);
			if (idRegla != -1) return false;

			var regla = new Regla(0,nueva.idSensor, nueva.Detalle, nueva.Operador, nueva.Valor, nueva.Accion, nueva.Actuadores);
			DAOSensores.Instancia.CargarNuevaRegla(regla);

			return true;
		}

		public bool NuevoActuador(Actuador nuevo)
		{
			var idActuador = ORM.Instancia.GetActuadorID(idUsuario, nuevo.ActuadorTipo);
			if (idActuador != -1) return false;

			var actuador = new Actuador(0, nuevo.ActuadorTipo, nuevo.Reglas, idUsuario, nuevo.Dispositivos);
			DAOSensores.Instancia.CargarNuevoActuador(actuador);

			return true;
		}

		public bool ModificarSensor(Sensor modificado)
		{
			var dbSensor = ORM.Instancia.GetSensor(idUsuario, modificado.TipoSensor);
			if (dbSensor != null) return false;

			DAOSensores.Instancia.ModificarSensor(modificado);
			return true;
		}

		public bool ModificarRegla(Regla modificada)
		{
			var idRegla = ORM.Instancia.GetReglaID(modificada.idSensor, modificada.Detalle, modificada.Valor, modificada.Operador, modificada.Accion);
			if (idRegla != -1 && idRegla != modificada.idRegla) return false;
			
			return DAOSensores.Instancia.ModificarRegla(modificada);			
		}

		public bool ModificarActuador(Actuador modificado)
		{
			var idActuador = ORM.Instancia.GetActuadorID(idUsuario, modificado.ActuadorTipo);
			if (idActuador != -1 && idActuador != modificado.IdActuador) return false;

			if (modificado.Reglas.Count == 0) return false;

			DAOSensores.Instancia.ModificarActuador(modificado);
			return true;
		}

		public void EliminarActuador(Actuador actuador)
		{
			DAOSensores.Instancia.EliminarActuador(actuador);
		}

		public void EliminarRegla(Regla regla)
		{
			DAOSensores.Instancia.EliminarRegla(regla);
		}

		public void EliminarSensor(Sensor sensor)
		{
			DAOSensores.Instancia.EliminarSensor(sensor);
		}

		public void EliminarDispositivo(int idDispositivo, int idCliente, int numero)
		{			
			var dispositivo = BuscarDispositivo(idDispositivo, idCliente, numero);
			if (dispositivo.EsInteligente) DAOSensores.Instancia.QuitarDispositivoDeActuadores(dispositivo);
				
			DAODispositivo.Instancia.QuitarDispositivo(dispositivo);

			dispositivos.Remove(dispositivo);
			ORM.Instancia.Delete(dispositivo);
		}

		public void UpdateMyData(Cliente modificado)
		{
			nombre = modificado.nombre;
			apellido = modificado.apellido;
			domicilio = modificado.domicilio;
			Telefono = modificado.Telefono;
			Documento_numero = modificado.Documento_numero;
			Documento_tipo = modificado.Documento_tipo;

			ORM.Instancia.Update(this);
		}

		#region INTERFAZ CONTROLLER
		public SimplexResult RunSimplex()
        {
            SIMPLEX sim = new SIMPLEX();
			
			var resultado = sim.Consulta(dispositivos);
			           
			return resultado;
        }

        public void CargarDispositivos(HttpPostedFileBase file, int flag)
        {
            CargarJson cargar = new CargarJson();
            if (flag == 1) cargar.LoadJson<Inteligente>(file.InputStream, idUsuario);
            else cargar.LoadJson<Estandar>(file.InputStream, idUsuario);
        }

        public void AgregarDispositivoDeTemplate(int disp)
        {
            DAOTemplates a = new DAOTemplates();
            DispositivoGenerico dispositivo = a.Searchtemplatebyid(disp);			

            if (dispositivo.Inteligente) NuevoDispositivoInteligente(dispositivo.ID, dispositivo.getNombreEntero(), dispositivo.Consumo, dispositivo.Bajoconsumo);
            else NuevoDispositivoEstandar(dispositivo.ID, dispositivo.getNombreEntero(), dispositivo.Consumo, dispositivo.Bajoconsumo, 0);
        }

		public List<DispositivoGenerico> GetTemplateDisp()
		{
			var daot = new DAOTemplates();

			return daot.templateDisps;
		}

		public void CambiarEstado(Inteligente disp)
		{
			if (disp.Estado == 0) EncenderDispositivo(disp);
			else if (disp.Estado == 1)
			{
				if (disp.BajoConsumo) ModoAhorroDispositivo(disp);
				else ApagarDispositivo(disp);
			}
			else if (disp.Estado == 2) ApagarDispositivo(disp);
		}

		public void CambiarAutoSimplex()
		{
			AutoSimplex = !AutoSimplex;
			ORM.Instancia.Update(this);
		}

    #endregion
}
}