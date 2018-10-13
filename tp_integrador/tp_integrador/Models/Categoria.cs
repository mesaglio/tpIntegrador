using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace tp_integrador.Models
{
    public class Categoria
    {
        [Key]
        public string IdCategoria { get; set; }
        public byte Consumo { get; set; }
        public decimal CargoFijo { get; set; }
        public decimal CargoVariable { get; set; }

        public Categoria(string id, byte valor, decimal fijo, decimal variable)
        {
            IdCategoria = id;
            Consumo = valor;
            CargoFijo = fijo;
            CargoVariable = variable;
        }
    }
}