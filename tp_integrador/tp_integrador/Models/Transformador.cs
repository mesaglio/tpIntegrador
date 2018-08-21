using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Gmap.net;//localizacion
using Gmap.net.Overlays;//radio

namespace tp_integrador.Models
{
    public class Transformador
    {
        public bool EstaActivo { get; set; }
        public int CantidadEnergia { get; set; }
        public List<Cliente> clientes { get; set; }
        public CircleMarker Radar { get; set; }  //atributo

        public Transformador(String id, int radio, double latitude, double longitude)
        {
            Radar = new CircleMarker(id);
            Radar.Radius = radio;
            Radar.Point = new Location(latitude, longitude);
        }

        public bool clienteMePertenece(Cliente cliente)
        {
            return Radar.Radius > distanciaHaciaCliente(cliente);
        }

        public double distanciaHaciaCliente(Cliente cliente)
        {
            return distancia(cliente.ubicacion, Radar.Point);
        }

        public double distancia(Location l1, Location l2)
        {
            return Math.Sqrt(Math.Pow(l1.Latitude - l2.Latitude, 2) + Math.Pow(l1.Latitude - l2.Latitude, 2));
        }

        public void AgregarCliente(Cliente cliente)
        {
            if (clienteMePertenece(cliente))
            {
                clientes.Add(cliente);
            }
        }

       
    }
}