using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Gmap.net;//localizacion

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
        private bool islogin { get; set; }
        public bool esadmin { get; set; }

        public bool Esadmin() { return esadmin; }
        public Location ubicacion { get; set; }

        public void AgregarUbicacion(double latitude, double longitude) {
            ubicacion.Latitude = latitude;
            ubicacion.Longitude = longitude;
        }

        public Usuarios()
        {
        }

        public Usuarios(string v1, string v2)
        {
            this.usuario = v1;
            this.password = v2;
            esadmin = false;
        }

        public Usuarios(int id, string name, string lastname, string home, string user, string clave)
        {
            idUsuario = id;
            nombre = name;
            apellido = lastname;
            domicilio = home;
            usuario = user;
            password = clave;
            islogin = false;
            esadmin = false;



        }
        public bool IsAuthenticated()
        {
            if (islogin)
                return true;
            else
            {
                return false;
            }
        }

        public Usuarios nulluser()
        {
            Usuarios user = new Usuarios(0, null, null, null, null, null);
            user.islogin = false;
            return user;
        }

        public void SetLoginOn() => islogin = true;


        public bool Singin(Usuarios u)
        {
            if (u.password == password && u.usuario == usuario)
                return true;
            else
                return false;
        }


    }
}