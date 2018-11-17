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
		private Timer timer;
        public List<Usuarios> listusuarios;
		       
        private DAOUsuario()
        {
			listusuarios = new List<Usuarios>();
			IniciarAutoSimplex();
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
            listusuarios.Remove(BuscarCliente(id));
        }
				
		public void IniciarAutoSimplex()
		{
			var startTimeSpan = TimeSpan.Zero;
			var periodTimeSpan = TimeSpan.FromDays(30);

			timer = new Timer((e) => {	AutoSimplex(); }, null, startTimeSpan, periodTimeSpan);
		}

		public void StopAutoSimplex()
		{
			timer.Dispose();
		}

		private void AutoSimplex()
		{
			var clientes = ORM.Instancia.GetClientesAutoSimplex();
			var simplex = new SIMPLEX();
			var listaDisp = new List<Inteligente>();
			string[] respuesta;
			int total;
			double consumoMes;			

			foreach (Cliente c in clientes)
			{
				listaDisp = c.dispositivos.OfType<Inteligente>().ToList();
				listaDisp.RemoveAll(x => x.Nombre.Split(' ')[0] == "Heladera");
				total = listaDisp.Count;
				
				respuesta = simplex.GetSimplexData(simplex.CrearConsulta(new List<Dispositivo>(listaDisp)));
				for(var i = 0; i < total; i++)
				{
					consumoMes = listaDisp[i].ConsumoEnElMes();
					if (consumoMes > Double.Parse(respuesta[i + 1])) listaDisp[i].Apagar();
					ORM.Instancia.Update(listaDisp[i]);
				}

			}
		}
	}

}