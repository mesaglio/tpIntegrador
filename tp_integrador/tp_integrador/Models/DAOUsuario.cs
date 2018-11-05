using Gmap.net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using tp_integrador.Models;

namespace tp_integrador.Models
{
    public class DAOUsuario
    {

        public List<Usuarios> listusuarios;
        public void Listusuarios()
        {
            listusuarios = new List<Usuarios>();
        }

        public DAOUsuario()
        {
            Listusuarios();            
        }
              
        public Usuarios InicioSecion(Usuarios u)
        {
            Usuarios retur = new Usuarios();
            foreach (Usuarios item in listusuarios)
            {
                if (item.Singin(u))
                    return  item;
                else
                    retur = item.nulluser();
            }
            return retur;
        }

        public void CargarUsuario(Usuarios unUsuario)
        {
            listusuarios.Add(unUsuario);
        }

        public void CargarCliente(int id, string name, string lastname, string home, Location coords, string user, string clave, string phone, DateTime alta, Categoria categ, string doc_t, string doc_n)
        {
            Cliente unCliente = new Cliente(id, name, lastname, home, coords, user, clave, phone, alta, categ, doc_t, doc_n);
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

        public void QuitarUsuario(int id)
        {
            listusuarios.Remove(BuscarCliente(id));
        }
    }

}