using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace tp_integrador.Models
{
    public class Dispositivo
    {
        public string Nombre { get; private set; }
        public int Consumo { get; private set; }
        public bool Estado { get; private set; }
        private DateTime encendidoDesde;


        public double EnergiaConsumida()
        {
            if (Estado == true) { 
                TimeSpan tiempo = DateTime.Now.Subtract(encendidoDesde);
                long horas = (tiempo.Days * 24) + tiempo.Hours;
                return Consumo * horas;
            }
            else
            {
                return 0;
            }
        }

        public Dispositivo(string nombre, int consumo)
        {
            Nombre = nombre;
            Consumo = consumo;
            Estado = false;
            encendidoDesde = DateTime.MinValue;
        }

        public void Apagar()
        {
            Estado = false;
        }
        
        public void Encender()
        {
            if (Estado == false)
            {
                Estado = true;
                encendidoDesde = DateTime.Now;
            }
        }
        
    }   
}