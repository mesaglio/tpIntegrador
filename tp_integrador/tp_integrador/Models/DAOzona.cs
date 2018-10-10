using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

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
        public void AgregarTransformadorAZona(Transformador t)
        {
            zonas.First().AgregarTransformador(t);
        }
        public void Agregarzona(Zona z)
        {
            zonas.Add(z);
        }
    }
}