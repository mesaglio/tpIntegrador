using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace tp_integrador.Models
{
    public class Categoria
    {
        public string IdCategoria { get; set; }
        public int Consumo { get; set; }
        public decimal CargoFijo { get; set; }
        public decimal CargoVariable { get; set; }

        public Categoria(string id, int valor, decimal fijo, decimal variable)
        {
            IdCategoria = id;
            Consumo = valor;
            CargoFijo = fijo;
            CargoVariable = variable;
        }
    }
}