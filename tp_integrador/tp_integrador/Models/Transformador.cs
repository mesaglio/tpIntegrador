﻿using System.Linq;
using System.Web;
using System.Collections.Generic;
using Gmap.net;


namespace tp_integrador.Models
{
    public class Transformador
    {
        public int id { get; set; }
        public Location location { get; set; }
		public bool EstaActivo { get; set; }

		public List<int> ClientesID { get; set; }

        public Transformador(int _id, int latitude, int longitude, bool estado, int energia)
        {
            id = _id;
            location = new Location(latitude, longitude);
			EstaActivo = estado;			
        }

		public int CantidadEnergia()
		{
			//Hacer
			return 0;
		}

    }
}