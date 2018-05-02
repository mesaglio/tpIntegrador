using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace tp_integrador.Models
{
    public class Cliente : Usuarios
    {
        public string telefono;
        public DateTime altaservicio;
        public Categoria categoria;
        public string documento_tipo;
        public string documento_numero;

        public List<Dispositivo> Conectados() // develve dispositivos
        {
            List<Dispositivo> l = new List<Dispositivo>();
            return l;
        }
    public int Cantencendidos()
        {
            int cantidad = 1;
            return cantidad;
        }
        public int Cantapagados()
        {
            int cantidad = 1;
            return cantidad;
        }
    }
}