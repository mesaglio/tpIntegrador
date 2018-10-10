using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Gmap.net;

namespace tp_integrador.Models
{
    public class DAOzona
    {
        public List<Zona> zonas { get; set; }
       public DAOzona()
        {
            zonas = new List<Zona>();
            zonas.Add(new Zona("1",100, 4586, 4452));
        }

        public void Agregarzona(Zona z)
        {
            zonas.Add(z);
        }
        



        public void AgregarTransformadorAZona(Transformador t)
        {
            foreach (Zona z in zonas)
            {
                if (z.distancia(z.Radar.Point, t.location) < z.Radar.Radius)
                {
                    z.AgregarTransformador(t);
                } 
            }

        }



    }
}