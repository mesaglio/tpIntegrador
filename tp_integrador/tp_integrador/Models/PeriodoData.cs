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
		private bool actual;

		public PeriodoData()
		{
			FechaInicio = DateTime.MinValue;
			FechaFin = DateTime.MinValue;
			actual = false;
		}

		public void PeriodoActual()
		{
			FechaFin = DateTime.Now;
			Numero = (Byte)FechaFin.Month;
			FechaInicio = new DateTime(FechaFin.Year, FechaFin.Month, 1, 0, 0, 0, 0);
			actual = true;
		}

		public void Periodo(byte numero, int anio)
		{
			if (numero == DateTime.Now.Month && anio == DateTime.Now.Year) { PeriodoActual(); return; }
			if (anio > DateTime.Now.Year || (numero > DateTime.Now.Month && anio == DateTime.Now.Year)) return;

			Numero = numero;
			FechaFin = new DateTime(anio, numero, DateTime.DaysInMonth(anio, numero),23,59,59,999);
			FechaInicio = new DateTime(FechaFin.Year, FechaFin.Month, 1, 0, 0, 0, 0);
			actual = false;
		}

		public bool EsElActual()
		{
			return actual;
		}

		public int TotalDias()
		{
			return DateTime.DaysInMonth(FechaInicio.Year, FechaInicio.Month);
		}
	}
}