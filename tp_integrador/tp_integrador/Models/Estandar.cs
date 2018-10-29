using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace tp_integrador.Models
{
    public class Estandar : Dispositivo
    {
        private byte usoDiario;

        public Estandar(int idDisp, int idClie, int numero, string nombre, double consumo, byte horasPromedio) : base(idDisp, idClie, numero ,nombre, consumo)
        {
			IdDispositivo = idDisp;
			IdCliente = idClie;
			Numero = numero;
            Nombre = nombre;
            Consumo = consumo;
            usoDiario = horasPromedio;
        }


        private double EnergiaPorHora()
        {
            return Consumo;
        }

        public double ConsumoEstimado()
        {
            return EnergiaPorHora() * usoDiario;
        }

        public void SetUsoDiario(byte horas)
        {
            usoDiario = horas;
        }
                
    }
}