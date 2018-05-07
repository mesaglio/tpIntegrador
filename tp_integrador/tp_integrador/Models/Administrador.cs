using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace tp_integrador.Models
{
    public class Administrador : Usuarios
    {
        public DateTime AltaSistema { get; private set; }
        private List<Cliente> clientes = new List<Cliente>();


        public int CantMeses()
        {
            return Math.Abs(DateTime.Now.Year - AltaSistema.Year) * 12 + (DateTime.Now.Month - AltaSistema.Month);
        }

        public Administrador()
        {
        }

        public Administrador(int id, string name, string lastname, string home, string user, string clave, DateTime alta) : base(id, name, lastname, home, user, clave)
        {
            AltaSistema = alta;
        }

        public void NuevoCliente(Cliente unCliente)
        {
            clientes.Add(unCliente);
        }

        public void NuevoCliente(int id, string name, string lastname, string home, string user, string clave, string phone, DateTime alta, Categoria categ, string doc_t, string doc_n)
        {
            Cliente unCliente = new Cliente(id, name, lastname, home, user, clave, phone, alta, categ, doc_t, doc_n);
            clientes.Add(unCliente);
        }

        public Cliente BuscarCliente(int id)
        {
            foreach(Cliente cliente in clientes)
            {
                if(cliente.idUsuario == id)
                {
                    return cliente;
                }
            }
            return null;
        }

        public void BajaCliente(int id)
        {
            clientes.Remove(BuscarCliente(id));
        }

        
    }
}