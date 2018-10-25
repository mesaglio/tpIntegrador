using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace tp_integrador.Models
{
    public class Categoria
    {
        public string IdCategoria { get; set; }
        public byte ConsumoMin { get; set; }
		public byte ConsumoMax { get; set; }
		public decimal CargoFijo { get; set; }
        public decimal CargoVariable { get; set; }

        public Categoria(string id, byte valormin, byte valormax, decimal fijo, decimal variable)
        {
            IdCategoria = id;
            ConsumoMin = valormin;
			ConsumoMax = valormax;
			CargoFijo = fijo;
            CargoVariable = variable;
        }
    }
}