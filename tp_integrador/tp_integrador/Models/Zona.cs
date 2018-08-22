using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Gmap.net;//localizacion
using Gmap.net.Overlays;

namespace tp_integrador.Models
{
    public class Zona
    {
		public int id;
		public Location location;
		public string nombre;
        public List<Transformador> transformadores;

        public Zona(int id, string nomb, double latitude, double longitude)
        {
            this.id = id;
            nombre = nomb;
            location = new Location(latitude, longitude);
        }

        public void AddTransformador(Transformador transformador)
        {
            transformadores.Add(transformador);
        }

        
    }
}