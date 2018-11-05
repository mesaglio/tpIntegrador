using Gmap.net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace tp_integrador.Models
{
    public class Administrador : Usuarios
    {
        public DateTime AltaSistema { get; set; }
        public DAOUsuario Dao;
   

        public int CantMeses()
        {
            return Math.Abs(DateTime.Now.Year - AltaSistema.Year) * 12 + (DateTime.Now.Month - AltaSistema.Month);
        }

        public Administrador(string v1, string v2)
        {
            usuario = v1;
            password = v2;
            esadmin =true;

    }

        public Administrador()
        {
            Dao = MvcApplication.Daobjeto;
            esadmin = true;
        }

        public Administrador(int id, string name, string lastname, string home, string user, string clave, DateTime alta) : base(id, name, lastname, home, user, clave)
        {
            AltaSistema = alta;
            esadmin = true;
            Dao = MvcApplication.Daobjeto;
        }

        public void NuevoCliente(int id, string name, string lastname, string home, Location coords, string user, string clave, string phone, DateTime alta, Categoria categ, string doc_t, string doc_n)
        {
			Cliente unCliente = new Cliente(id, name, lastname, home, coords, user, clave, phone, alta, categ, doc_t, doc_n);
            Dao.CargarUsuario(unCliente);
        }

        public Cliente BuscarCliente(int id)
        {
            return Dao.BuscarCliente(id);
        }

        public void BajaCliente(int id)
        {
            Dao.QuitarUsuario(id);
        }

        
    }
}