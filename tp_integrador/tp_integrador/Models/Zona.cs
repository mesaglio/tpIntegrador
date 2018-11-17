using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Gmap.net;
using Gmap.net.Overlays;

namespace tp_integrador.Models
{
    public class Zona
    {      
        public int idZona { get; set; }
		public int Radio { get; set; }
		public double Latitude { get; set; }
		public double Longitude { get; set; }
        public CircleMarker Radar { get; set; }

		public List<Transformador> Transformadores { get; set; }

		public Zona(int id, int radio, double latitude, double longitude, List<Transformador> trans)
        {
			idZona = id;
			Radio = radio;
			Latitude = latitude;
			Longitude = longitude;
			
            Radar = new CircleMarker(id.ToString());
            Radar.Radius = radio;
            Radar.Point = new Location(latitude, longitude);
			Transformadores = trans;
        }
      
        public void AgregarNuevoTransformador(Transformador unTransformador)
        {
			unTransformador.idZona = idZona;
            Transformadores.Add(unTransformador);
			ORM.Instancia.Insert(unTransformador);
        }

		public bool EstaEnLaZona(Location tlocation)
		{
			return (Distancia(tlocation, Radar.Point) < Radar.Radius);
		}

        public double Distancia(Location l1, Location l2)
        {
			var l1Rad = Math.PI * l1.Latitude / 180;
			var l2Rad = Math.PI * l2.Latitude / 180;
			var theta = l1.Longitude - l2.Longitude;
			var thetaRad = Math.PI * theta / 180;

			double dist = Math.Sin(l1Rad) * Math.Sin(l2Rad) + Math.Cos(l1Rad) *	Math.Cos(l2Rad) * Math.Cos(thetaRad);
			dist = Math.Acos(dist);

			dist = dist * 180 / Math.PI;
			dist = dist * 60 * 1.1515;

			return dist * 1.609344;
		}       

        public void AsignarTransformadorAlCliente(Cliente cliente)
        {
            Location l = cliente.Coordenadas;
            if (EstaEnLaZona(cliente.Coordenadas))
            {
                Transformador masCercano = Transformadores.First();

                foreach (Transformador t in Transformadores)
                {
                    if (Distancia(t.location, l) <= Distancia(masCercano.location, l))
                        masCercano = t;
                }

				masCercano.AgregarCliente(cliente);
            }
        }

		public int TransformadorQueTiene(int idCliente)
		{
			var t = Transformadores.Find(x => x.TenesCliente(idCliente));
			return (t != null) ? t.id : -1;			
		}
    }
}