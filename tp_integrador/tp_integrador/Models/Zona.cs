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
        public List<Transformador> transformadores { get; set; }
        public CircleMarker Radar { get; set; }  //atributo
        public Zona(String id, int radio, int latitude, int longitude)
        {
            Radar = new CircleMarker(id);
            Radar.Radius = radio;
            Radar.Point = new Location(latitude, longitude);
        }

        public double distancia(Location l1, Location l2)
        {
            return Math.Sqrt(Math.Pow(l1.Latitude - l2.Latitude, 2) + Math.Pow(l1.Latitude - l2.Latitude, 2));
        }

        bool ClienteViveAqui(Cliente cliente)
        {
            return distancia(cliente.ubicacion, Radar.Point) < Radar.Radius;
        }

        public void AsignarTransformadorAlCliente(Cliente cliente)
        {
            Location l = cliente.ubicacion;
            if (ClienteViveAqui(cliente))
            {
                Transformador masCercano = transformadores.First();

                foreach (Transformador t in transformadores)
                {
                    if (distancia(t.location, l) <= distancia(masCercano.location, l))
                        masCercano = t;
                }
                masCercano.clientes.Add(cliente);
            }
        }

    }
}