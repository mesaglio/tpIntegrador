using System.Linq;
using System.Web;
using System.Collections.Generic;
using Gmap.net;


namespace tp_integrador.Models
{
    public class Transformador
    {
        public int id { get; set; }
		public int idZona { get; set; }
        public Location location { get; set; }
		public bool EstaActivo { get; set; }

		public List<int> ClientesID { get; set; }

        public Transformador(int _id, int zona, double latitude, double longitude, bool estado, List<int> idClientes)
        {
            id = _id;
			idZona = zona;
            location = new Location(latitude, longitude);
			EstaActivo = estado;
			ClientesID = idClientes;
        }

		public int CantidadEnergia()
		{
			//Hacer
			return 0;
		}

		public bool TenesCliente(int idCliente)
		{
			return ClientesID.Contains(idCliente);
		}
    }
}