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
            Categoria categoria = new Categoria("R1", 0, 150, 18.76m, 0.644m);
            Cliente u = new Cliente(31,"nicolas","perez","calle cualquiera 123", "nico", "1234", "44112233",DateTime.Now.AddYears(-20).AddMonths(-3), categoria, "DNI", "12345678", null);
            listusuarios.Add(u);
            Administrador a = new Administrador(1, "pepe", "lado", "av algo 1234", "pepe", "1234", DateTime.Now.AddYears(-2).AddMonths(-3));
            listusuarios.Add(a);
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

        public void CargarCliente(int id, string name, string lastname, string home, string user, string clave, string phone, DateTime alta, Categoria categ, string doc_t, string doc_n)
        {
            Cliente unCliente = new Cliente(id, name, lastname, home, user, clave, phone, alta, categ, doc_t, doc_n, null);
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