using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using Gmap.net;

namespace tp_integrador.Models
{
    public class DAOzona
    {
		private static DAOzona instancia;
		public List<Zona> zonas { get; set; }

		public DAOzona()
        {
			//TOOD: WIP 
            zonas = new List<Zona>();
            //zonas.Add(new Zona(1,100, 4586, 4452, new List<Transformador>()));
        }

		public static DAOzona Instancia
		{
			get
			{
				if (instancia == null) instancia = new DAOzona();
				return instancia;
			}
		}

		public void InitialLoad()
		{
			zonas = ORM.Instancia.GetAllZonas();
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

		public int AsignarTransformador(Cliente unCliente)
		{
			foreach (Zona zona in zonas)
			{
				if (zona.ClienteViveAqui(unCliente))
				{
					zona.AsignarTransformadorAlCliente(unCliente);
					break;
				}
			}

			var idTrans = BuscarTransformadorDeCliente(unCliente.idUsuario);

			return idTrans != -1 ? idTrans : -1;
		}

		public int BuscarTransformadorDeCliente(int idCliente)
		{
			int id; 
			foreach (Zona z in zonas)
			{
				id = z.TransformadorQueTiene(idCliente);

				if (id != -1) return id;
			}

			return -1;
		}

		public List<TransJson> GetTransformadores()
		{
			var lista = new List<TransJson>();
			var trans = new List<Transformador>();

			foreach (var z in zonas)
			{
				trans.AddRange(z.Transformadores.FindAll(x => !trans.Contains(x)));
			}

			foreach (var t in trans)
			{
				lista.Add(new TransJson(t));
			}

			return lista;
		}

		public class TransJson
		{

			public int Id;
			public string PlaceName, GeoLong, GeoLat;

			public TransJson(Transformador t)
			{
				Id = t.id; PlaceName = "Transformador " + t.id; GeoLong = t.location.Longitude.ToString(); GeoLat = t.location.Latitude.ToString();
			}

		}
	}
}