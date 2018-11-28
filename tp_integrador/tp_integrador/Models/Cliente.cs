using Gmap.net;
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
			ORM.Instancia.Insert(nuevo);

            Puntos += 15;
			ORM.Instancia.Update(this);
        }

        public void NuevoDispositivoEstandar(int idDisp, string nombre, double consumo, bool bajoconsumo, byte usoPromedio)
        {
			Estandar nuevo = new Estandar(idDisp, idUsuario, CalcularNumero(nombre), nombre, consumo, bajoconsumo, usoPromedio);
			dispositivos.Add(nuevo);

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
			dispositivo.Numero = CalcularNumero(nombre);
            dispositivos.Add(dispositivo);
			ORM.Instancia.Insert(dispositivo);
			DAODispositivo.Instancia.CargarDispositivo(dispositivo);
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
			double total = 0;

			foreach (var disp in dispositivos.OfType<Inteligente>())
			{
				total += disp.ConsumoEnElPeriodo(periodo);
			}

			periodo.Consumo = total;
			return periodo;
		}

        public PeriodoData ConsumoDelPeriodo(DateTime desde, DateTime hasta)
        {
            var periodo = new PeriodoData();
            periodo.Periodo((byte)desde.Month, desde.Year);

            double total = 0;
            
            foreach (var disp in dispositivos.OfType<Inteligente>())
            {
                total += disp.ConsumoEnElPeriodo(periodo);
            }

            periodo.Consumo = total;
            return periodo;
        }

        public List<Sensor> MisSensores()
		{
			return DAOSensores.Instancia.FindAllFromCliente(idUsuario);
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
            if (flag == 1)
                cargar.LoadJson<Inteligente>(file.InputStream);
            else
                cargar.LoadJson<Estandar>(file.InputStream);
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