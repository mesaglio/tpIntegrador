using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace tp_integrador.Models
{
	public class PeriodoData
	{
		public byte Numero { get; set; }
		public DateTime FechaInicio { get; set; }
		public DateTime FechaFin { get; set; }
		public double Consumo { get; set; }

		public PeriodoData()
		{
			FechaInicio = DateTime.MinValue;
			FechaFin = DateTime.MinValue;
		}

		public void PeriodoActual()
		{
			FechaFin = DateTime.Now;

			Double divMes = FechaFin.Month / 2;
			Numero = (Byte)Math.Ceiling(divMes);		

			if (divMes % 1 == 0) FechaInicio = new DateTime(FechaFin.Year, FechaFin.Month - 1, 1, 0, 0, 0, 0);
			else FechaInicio = new DateTime(FechaFin.Year, FechaFin.Month, 1, 0, 0, 0, 0);	
		}

		public void Periodo(byte numero, int anio)
		{
			Numero = numero;
			FechaFin = new DateTime(anio, numero*2, DateTime.DaysInMonth(anio, numero*2),23,59,59,999);			
			FechaInicio = new DateTime(FechaFin.Year, FechaFin.Month - 1, 1, 0, 0, 0, 0);
		}

		public bool EsElActual()
		{
			var periodoActual = new PeriodoData();
			periodoActual.PeriodoActual();

			return FechaFin > periodoActual.FechaInicio;
		}

		public int TotalDias()
		{
			var anio = FechaInicio.Year;
			FechaFin = new DateTime(anio, Numero * 2, DateTime.DaysInMonth(anio, Numero * 2), 23, 59, 59, 999);
			FechaInicio = new DateTime(FechaFin.Year, FechaFin.Month - 1, 1, 0, 0, 0, 0);

			return (DateTime.DaysInMonth(anio, FechaInicio.Month)) + (DateTime.DaysInMonth(anio, FechaFin.Month));
		}
	}
}