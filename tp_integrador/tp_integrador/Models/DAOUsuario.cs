using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace tp_integrador.Models
{
    public class DAOUsuario
    {
        public List<Usuarios> listusuarios;

        public Usuarios InicioSecion(Usuarios u)
        {
            Usuarios retur = new Usuarios();
            foreach (Usuarios item in listusuarios)
            {
                if (item.Singin(u))
                    retur = item;
                else
                    retur = null;
            }
            return retur;
        }
    }
}