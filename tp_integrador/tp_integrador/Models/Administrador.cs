using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace tp_integrador.Models
{
    public class Administrador : Usuarios
    {
        public DateTime AltaSistema { get; private set; }
        public DAOUsuario Dao { get; set; }


        public int CantMeses()
        {
            return Math.Abs(DateTime.Now.Year - AltaSistema.Year) * 12 + (DateTime.Now.Month - AltaSistema.Month);
        }


        public Administrador()
        {
        }

        public Administrador(int id, string name, string lastname, string home, string user, string clave, DateTime alta, DAOUsuario dao) : base(id, name, lastname, home, user, clave)
        {
            AltaSistema = alta;
            Dao = dao;
        }

        public void NuevoCliente(int id, string name, string lastname, string home, string user, string clave, string phone, DateTime alta, Categoria categ, string doc_t, string doc_n)
        {
            Cliente unCliente = new Cliente(id, name, lastname, home, user, clave, phone, alta, categ, doc_t, doc_n);
            Dao.CargarCliente(unCliente);
        }

        public Cliente BuscarCliente(int id)
        {
            return Dao.BuscarUsuario(id);
        }

        public void BajaCliente(int id)
        {
            Dao.QuitarUsuario(id);
        }

        
    }
}