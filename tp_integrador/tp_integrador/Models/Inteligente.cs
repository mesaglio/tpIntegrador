using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace tp_integrador.Models
{
    public class Inteligente : Dispositivo
    {
        public byte Estado { get; private set; } // Apagado = 0; Encendido = 1; ModoAhorro = 2
        public DateTime fechaEstado { get; private set; }
        private DAOHistorialEstado daoEstado;
		public bool Convertido { get; set; }

        public Inteligente(int idDisp, int idClie, int numero, string nombre, double consumo, bool bajoconsumo, byte estado, DateTime estadoFecha, bool convertido) : base(idDisp, idClie, numero, nombre, consumo, bajoconsumo)
        {
			IdDispositivo = idDisp;
			IdCliente = idClie;
			Numero = numero;
            Nombre = nombre;
            Consumo = consumo;
			BajoConsumo = bajoconsumo;
            Estado = estado;
            fechaEstado = estadoFecha;
            daoEstado = new DAOHistorialEstado(idClie, idDisp, numero);
            EsInteligente = true;
			Convertido = convertido;
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

		public double ConsumoEnEstadoActual()
		{
			if (Estado == 1) return Consumo;
			else if (Estado == 2) return Consumo / 2;
			else return 0;
		}

        private void CambiarAEstado(byte nuevoEstado)
        {
            DateTime nuevaFecha = DateTime.Now;
            daoEstado.CargarEstado(Estado, fechaEstado, nuevaFecha);
            Estado = nuevoEstado;
            fechaEstado = nuevaFecha;

			ORM.Instancia.Update(this);
        }
		
        public bool Encendido()
        {
            return (Estado != 0);
        }

		public double ConsumoDesdeHasta(DateTime desde, DateTime hasta)
		{
			List<EstadoDispositivo> estadosEnPeriodo = daoEstado.GetEstados(desde, hasta);

			double valor = 0;
			foreach (EstadoDispositivo guardado in estadosEnPeriodo)
			{
				valor += ConsumoEnPor(guardado.Estado, guardado.GetHoras());
			}

			if (fechaEstado < hasta) valor += ConsumoEnPor(Estado, hasta.Subtract(fechaEstado).TotalHours);
			return valor;
        }
               
        public double ConsumoUltimasHoras(int horas)
        {
            DateTime fechaAhora = DateTime.Now;
            DateTime fechaObjetivo = fechaAhora.Subtract(new TimeSpan(horas,0,0));

            return ConsumoDesdeHasta(fechaObjetivo, fechaAhora);           
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

		public double ConsumoEnElPeriodo(PeriodoData periodoData)
		{
			//if (!periodoData.EsElActual()) return 0; //Traer Datos de Mongo
			 return ConsumoDesdeHasta(periodoData.FechaInicio, periodoData.FechaFin);
		}
		
		public void BajarTemperatura()
		{			
		}

		public void SubirTemperatura()
		{
		}

		public void BajarIntensidad()
		{
		}

		public void SubirIntensidad()
		{
		}
	}
}