using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace tp_integrador.Models
{
    public class Estandar : Dispositivo
    {
        public byte usoDiario { get; set; }

		public Estandar() :base() { }

        public Estandar(int idDisp, int idClie, int numero, string nombre, double consumo, bool bajoconsumo, byte horasPromedio) : base(idDisp, idClie, numero ,nombre, consumo, bajoconsumo)
        {
			IdDispositivo = idDisp;
			IdCliente = idClie;
			Numero = numero;
            Nombre = nombre;
            Consumo = consumo;
			BajoConsumo = bajoconsumo;
            usoDiario = horasPromedio;
        }		

        public double ConsumoEstimado()
        {
            return Consumo * usoDiario;
        }

        public void SetUsoDiario(byte horas)
        {
            usoDiario = horas;

			ORM.Instancia.Update(this);
        }
				
	}
}