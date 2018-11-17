using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace tp_integrador.Models
{
    public abstract class Dispositivo
    {
		public int IdDispositivo { get; set; }
		public int IdCliente { get; set; }
		public int Numero { get; set; }
		public string Nombre { get; set; }
        public double Consumo { get; set; }
		public bool BajoConsumo { get; set; }

        public bool EsInteligente = false;

		protected Dispositivo()
		{
		}

		public Dispositivo(int idDisp, int idClie, int numero, string nombre, double consumo, bool bajoconsumo)
        {
			IdDispositivo = idDisp;
			IdCliente = idClie;
			Numero = numero;
            Nombre = nombre;
            Consumo = consumo;
			BajoConsumo = bajoconsumo;
        }	

		public abstract double ConsumoEnElMes();  

    }
}