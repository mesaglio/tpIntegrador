using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace tp_integrador.Models
{
    public class Usuarios 
    {
        public int idUsuario { get; set; }
        public string nombre { get; set; }
        public string apellido { get; set; }
        public string domicilio { get; set; }
        public string usuario { get; set; }
        public string password { get; set; }

        public Usuarios()
        {
        }

        public Usuarios(string v1, string v2)
        {
            this.usuario = v1;
            this.password = v2;
        }
		
		public Usuarios(int id, string name, string lastname, string home, string user, string clave)
        {
            idUsuario = id;
            nombre = name;
            apellido = lastname;
            domicilio = home;
            usuario = user;
            password = clave;
        }
        
        public bool Singin(string u, string p)
        {
            if (p == password && u == usuario)
                return true;
            else
                return false;
        }
    }
}