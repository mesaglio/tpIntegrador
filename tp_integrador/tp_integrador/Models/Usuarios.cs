using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace tp_integrador.Models
{
    public class Usuarios 
    {
        public int idusuario;
        public string nombre;
        public string apellido;
        public string domicilio;
        public string usuario;
        public string password;
    

    public bool Singin(string u, string p)
        {
            if (p == password && u == usuario)
                return true;
            else
                return false;
    }

}
}