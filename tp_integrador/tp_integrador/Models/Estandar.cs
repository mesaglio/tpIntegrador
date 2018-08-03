using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace tp_integrador.Models
{
    public class Estandar : Dispositivo
    {
        private byte usoDiario;

        public Estandar(string nombre, double consumo, byte horasPromedio) : base(nombre, consumo)
        {
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