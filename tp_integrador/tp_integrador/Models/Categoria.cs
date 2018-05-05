using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace tp_integrador.Models
{
    public class Categoria
    {
        public int IdCategoria { get; private set; }
        public string Consumo { get; private set; }
        public decimal CargoFijo { get; private set; }
        public decimal CargoVariable { get; private set; }
    }
}