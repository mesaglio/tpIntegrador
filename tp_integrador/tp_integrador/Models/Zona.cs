using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Gmap.net;//localizacion
using Gmap.net.Overlays;

namespace tp_integrador.Models
{
    public class Zona
    {
        //objetos por defecto para elejir
        
            [Key]
        public String nombre { get; set; }

        public List<Transformador> transformadores = new List<Transformador>();
        public CircleMarker Radar { get; set; }  //atributo
        public Zona(String id, int radio, double latitude, double longitude)
        {
            Radar = new CircleMarker(id);
            Radar.Radius = radio;
            Radar.Point = new Location(latitude, longitude);
            nombre = id;
        }
      
        public void AgregarTransformador(Transformador unTransformador)
        {
            if (distancia(unTransformador.location, Radar.Point) < Radar.Radius)
                transformadores.Add(unTransformador);
            //else podemos mandar un exeption diciendo que el transformador no esta localizado en esta zona
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

        public void RellenarTransformadores() {
            //AQUI ESTARIA BUENO LA LOGICA DE cargar los transformadores del json y guardarlos en la db 
            var trans1 = new Transformador("Trans01", 4896, 7811);
            var trans2 = new Transformador("Trans02", 4800, 7000);
            var trans3 = new Transformador("Trans03", 4700, 7912);
            var trans4 = new Transformador("Trans03", 4700, 7912);

            this.AgregarTransformador(trans1);
            this.AgregarTransformador(trans2);
            this.AgregarTransformador(trans3);
            this.AgregarTransformador(trans4);
        }


    }
}