using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace tp_integrador.Models
{
    public class Usuarios 
    {
        public Usuarios()
        {
        }

        public Usuarios(string v1, string v2)
        {
            this.usuario = v1;
            this.password = v2;
        }

        public int idusuario { get; set; }
        public string nombre { get; set; }
        public string apellido { get; set; }
        public string domicilio { get; set; }
        public string usuario { get; set; }
        public string password { get; set; }


        public bool Singin(Usuarios u)
        {
            if (u.password == password && u.usuario == usuario)
                return true;
            else
                return false;
    }

}
}