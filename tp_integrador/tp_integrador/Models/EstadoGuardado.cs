using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace tp_integrador.Models
{
    public class EstadoGuardado
    {
        public int Usuario { get; set; }
        public string Dispositivo { get; set; }
        public byte Estado { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }

        public EstadoGuardado(int user, string dispositivo, byte estado, DateTime desde, DateTime hasta)
        {
            Usuario = user;
            Dispositivo = dispositivo;
            Estado = estado;
            FechaInicio = desde;
            FechaFin = hasta;
        }

        public double GetHoras()
        {
            TimeSpan tiempo = FechaFin.Subtract(FechaInicio);
            return tiempo.TotalHours;
        }

        public EstadoGuardado GetEstadoEntre(DateTime desde, DateTime hasta)
        {
            if (FechaFin < desde || hasta < FechaInicio) return null;
            else if (desde < FechaInicio && FechaFin < hasta) return this;
            else if (FechaInicio < desde && desde < FechaFin)
            {
                EstadoGuardado tramo = this;
                tramo.FechaInicio = desde;
                return tramo;              
            }
            else
            {
                EstadoGuardado tramo = this;
                tramo.FechaFin = hasta;
                return tramo;
            }
        }
    }
}