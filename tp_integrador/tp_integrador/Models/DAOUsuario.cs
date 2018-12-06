using Gmap.net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using tp_integrador.Models;

namespace tp_integrador.Models
{
    public class DAOUsuario
    {
		private static DAOUsuario _instancia;
		private Timer timerSimplex;
		private Timer timerPrimeroDeMes;
        public List<Usuarios> listusuarios;
		       
        private DAOUsuario()
        {
			listusuarios = new List<Usuarios>();
			IniciarAutoSimplex();
			IniciarPrimeroDeMes();
        }

		public static DAOUsuario Instancia
		{
			get
			{
				if (_instancia == null) _instancia = new DAOUsuario();
				return _instancia;
			}
		}

		public void CargarUsuario(Usuarios unUsuario)
        {
            listusuarios.Add(unUsuario);
        }

		public void CargarUsuarioDeJson(Usuarios usuario)
		{
			ORM.Instancia.Insert(usuario);
		}

        public void CargarCliente(int id, string name, string lastname, string home, Location coords, string user, string clave, string phone, DateTime alta, Categoria categ, string doc_t, string doc_n, bool simplex)
        {
            Cliente unCliente = new Cliente(id, name, lastname, home, coords, user, clave, phone, alta, categ, doc_t, doc_n, simplex);
            listusuarios.Add(unCliente);
        }

        public Cliente BuscarCliente(int id)
        {
            foreach (Cliente usuario in listusuarios.OfType<Cliente>())
            {
                if (usuario.idUsuario == id)
                {
                    return usuario;
                }
            }
            return null;
        }

		public dynamic BuscarUsuario(int id)
		{
			return listusuarios.Find(x => x.idUsuario == id);
		}

        public void QuitarUsuario(int id)
        {
            listusuarios.RemoveAll(x => x.idUsuario == id);
        }
				
		public void IniciarAutoSimplex()
		{
			var startTimeSpan = TimeSpan.Zero;
			var periodTimeSpan = TimeSpan.FromDays(15);

			timerSimplex = new Timer((e) => {	AutoSimplex(); }, null, startTimeSpan, periodTimeSpan);
		}

		public void StopAutoSimplex()
		{
			timerSimplex.Dispose();
		}

		private void AutoSimplex()
		{
			var clientes = ORM.Instancia.GetClientesAutoSimplex();
			var simplex = new SIMPLEX();
			var listaDisp = new List<Inteligente>();
			SimplexResult respuesta;
			int total;
			double consumoMes;
			double consumoRespuesta;
			var periodo = new PeriodoData();
			periodo.PeriodoActual();

			foreach (Cliente c in clientes)
			{
				listaDisp = c.dispositivos.OfType<Inteligente>().ToList();				
				total = listaDisp.Count;
				
				respuesta = simplex.Consulta(new List<Dispositivo>(listaDisp));
				foreach(var dispo in listaDisp)
				{
					consumoMes = dispo.ConsumoEnElPeriodo(periodo);
					consumoRespuesta = Double.Parse(respuesta.Valores.Find(x => x.Nombre == dispo.Nombre && x.Numero == dispo.Numero).Consumo);
					if (consumoMes >= consumoRespuesta)
					{
						dispo.Apagar();
						ORM.Instancia.Update(dispo);
					}					
				}
			}
		}

		private void IniciarPrimeroDeMes()
		{
			var startTimeSpan = TimeSpan.Zero;
			var periodTimeSpan = TimeSpan.FromDays(1);

			timerPrimeroDeMes = new Timer((e) => { PrimeroDeMes(); }, null, startTimeSpan, periodTimeSpan);
		}

		private void PrimeroDeMes()
		{
			var hoy = DateTime.Now;

			if (hoy.Day != 1) return;

			int anio, mes;
			if (hoy.Month == 1)
			{
				anio = hoy.Year - 1;
				mes = 12;
			}
			else
			{
				anio = hoy.Year;
				mes = hoy.Month - 1;
			}

			Recategorizar(mes, anio);			
		}

		private void Recategorizar(int mes, int anio)
		{
			var lista = ORM.Instancia.GetAllClientes();

			Categoria categoria;
			foreach (var cliente in lista)
			{
				categoria = ORM.Instancia.GetCatgoriaFor(cliente.ConsumoDelPeriodo(mes, anio).Consumo);
				if (categoria != null)
				{
					cliente.Categoria = categoria;
					ORM.Instancia.Update(cliente);
				}
			}
		}

		public Cliente GetClienteFromDB(int idCliente)
		{
			return ORM.Instancia.GetUsuario(idCliente);
		}
	}

}