using System.Linq;
using System.Web;
using System.Collections.Generic;
using Gmap.net;


namespace tp_integrador.Models
{
    public class Transformador
    {
        public string id { get; set; }
        public Location location { get; set; }
        public Transformador(string _id, int latitude, int longitude)
        {
            id = _id;//no se si se puede asignar directamente una cadena si no como en C
            location = new Location(latitude, longitude);
        }

        public int CantidadEnergia { get; set; }
        public List<Cliente> clientes = new List<Cliente>();
        public bool EstaActivo { get; set; }

        /*public int CalcularTotalEnergia() {
			return  sumlist(map (clientes -> clientes.energia) clientes);
		}
		*/


    }
}