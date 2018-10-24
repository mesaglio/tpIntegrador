using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace tp_integrador.Models
{
    public class Inteligente : Dispositivo
    {
        private byte Estado; // Apagado = 0; Encendido = 1; ModoAhorro = 2
        private DateTime fechaEstado;
        private DAOHistorialEstado daoEstado;


        public Inteligente(int id, int numero, string nombre, double consumo, byte estado, DateTime estadoFecha) : base(numero, nombre, consumo)
        {
			Numero = numero;
            Nombre = nombre;
            Consumo = consumo;
            Estado = estado;
            fechaEstado = estadoFecha;
            daoEstado = new DAOHistorialEstado(id, Nombre);
            EsInteligente = true;
        }

        private double ConsumoEnPor(int estado, double horas)
        {
            double valor = 0;
            switch (estado)
            {
                case 1:
                    valor = horas * Consumo;
                    break;
                case 2:
                    valor = horas * Consumo / 2; //Consumo en modo ahorro (provisorio)
                    break;
                default:
                    valor = 0;
                    break;
            }
            return valor;
        }

        private void CambiarAEstado(byte nuevoEstado)
        {
            DateTime nuevaFecha = DateTime.Now;
            daoEstado.CargarEstado(Estado, fechaEstado, nuevaFecha);
            Estado = nuevoEstado;
            fechaEstado = nuevaFecha;
        }      



        public bool Encendido()
        {
            return (Estado != 0);
        }

        public double ConsumoDesdeHasta(DateTime desde, DateTime hasta)
        {
            List<EstadoGuardado> estadosEnPeriodo = daoEstado.GetEstados(desde, hasta);

            double valor = 0;
            foreach (EstadoGuardado guardado in estadosEnPeriodo)
            {
                valor += ConsumoEnPor(guardado.Estado, guardado.GetHoras());
            }

            return valor;
        }
               
        public double ConsumoUltimasHoras(byte horas)
        {
            DateTime fechaAhora = DateTime.Now;
            DateTime fechaObjetivo = fechaAhora.Subtract(new TimeSpan(horas,0,0));

            double valor = ConsumoDesdeHasta(fechaObjetivo, fechaAhora);
            return valor += ConsumoEnPor(Estado, fechaAhora.Subtract(fechaEstado).TotalHours);
        }
                
        public void Apagar()
        {
            if (!Encendido()) return;

            CambiarAEstado(0);
        }

        public void Encender()
        {
            if (Estado == 1) return;

            CambiarAEstado(1);
        }

        public void ModoAhorro()
        {
            if (Estado == 2) return;

            CambiarAEstado(2);
        }
        
    }
}