using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace tp_integrador.Models
{
    public class Categoria
    {
        public string IdCategoria { get; private set; }
        public int Consumo { get; private set; }
        public decimal CargoFijo { get; private set; }
        public decimal CargoVariable { get; private set; }

        public Categoria(string id, int valor, decimal fijo, decimal variable)
        {
            IdCategoria = id;
            Consumo = valor;
            CargoFijo = fijo;
            CargoVariable = variable;
        }
    }
}