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
      
        public void AgregarTransformador(Transformador unTransformador)
        {
            if (distancia(unTransformador.location, Radar.Point) < Radar.Radius)
                Transformadores.Add(unTransformador);
            //else podemos mandar un exeption diciendo que el transformador no esta localizado en esta zona
        }


        public double distancia(Location l1, Location l2)
        {			
            return Math.Sqrt(Math.Pow(l1.Latitude - l2.Latitude, 2) + Math.Pow(l1.Latitude - l2.Latitude, 2));
        }

        public bool ClienteViveAqui(Cliente cliente)
        {
            return distancia(cliente.ubicacion, Radar.Point) < Radar.Radius;
        }

        public void AsignarTransformadorAlCliente(Cliente cliente)
        {
            Location l = cliente.ubicacion;
            if (ClienteViveAqui(cliente))
            {
                Transformador masCercano = Transformadores.First();

                foreach (Transformador t in Transformadores)
                {
                    if (distancia(t.location, l) <= distancia(masCercano.location, l))
                        masCercano = t;
                }
                masCercano.ClientesID.Add(cliente.idUsuario);
            }
        }

		public int TransformadorQueTiene(int idCliente)
		{
			var t = Transformadores.Find(x => x.TenesCliente(idCliente));
			return (t != null) ? t.id : -1;			
		}
    }
}