using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace tp_integrador.Models
{
    public class EstadoDispositivo
    {
        public int Usuario { get; set; }
        public int Dispositivo { get; set; }
		public int DispNumero { get; set; }
        public byte Estado { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }

		public EstadoDispositivo(int user, int dispositivo, int numero, byte estado, DateTime desde, DateTime hasta)
        {
            Usuario = user;
            Dispositivo = dispositivo;
			DispNumero = numero;
            Estado = estado;
            FechaInicio = desde;
            FechaFin = hasta;
        }

        public double GetHoras()
        {
            TimeSpan tiempo = FechaFin.Subtract(FechaInicio);
            return tiempo.TotalHours;
        }

        public EstadoDispositivo GetEstadoEntre(DateTime desde, DateTime hasta)
        {
            if (FechaFin < desde || hasta < FechaInicio) return null;
            else if (desde < FechaInicio && FechaFin < hasta) return this;
            else if (FechaInicio < desde && desde < FechaFin)
            {
                EstadoDispositivo tramo = this;
                tramo.FechaInicio = desde;
                return tramo;              
            }
            else
            {
                EstadoDispositivo tramo = this;
                tramo.FechaFin = hasta;
                return tramo;
            }
        }
    }
}