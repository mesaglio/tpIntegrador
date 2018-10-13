using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace tp_integrador.Models
{
    public class Dispositivo
    {
        [Key]
		public int Numero { get; set; }
		public string Nombre { get; set; }
        public double Consumo;
        public bool EsInteligente = false;

        public Dispositivo(int numero, string nombre, double consumo)
        {
			Numero = numero;
            Nombre = nombre;
            Consumo = consumo;            
        }

       

    }
}