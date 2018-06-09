using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace tp_integrador.Models
{
    public class DAODispositivo
    {
        public List<Dispositivo> listdispositivo;
        public void Listdispositivo()
        {
            listdispositivo = new List<Dispositivo>();
        }



        public void CargarDispositivo(Dispositivo dispositivo)
        {
            listdispositivo.Add(dispositivo);
        }
    }
}