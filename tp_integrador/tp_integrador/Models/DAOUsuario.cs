using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace tp_integrador.Models
{
    public class DAOUsuario
    {

        public List<Usuarios> Listusuarios;
        public void listusuarios()
        {
            Listusuarios = new List<Usuarios>();
        }

        public DAOUsuario()
        {
            listusuarios();   
            Usuarios u = new Usuarios("nico","1234");
            Listusuarios.Add(u);
        }

        

        public Usuarios InicioSecion(Usuarios u)
        {
            Usuarios retur = new Usuarios();
            foreach (Usuarios item in Listusuarios)
            {
                if (item.Singin(u.usuario,u.password))
                    retur = item;
                else
                    retur = null;
            }
            return retur;
        }
    }
}