using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace tp_integrador.Models
{
    public class Regla
    {
        public int idRegla { get; set; }
		public int idSensor { get; set; }
		public string Detalle { get; set; }
		public string Operador { get; set; }
		public string Accion { get; set; }
		public int Valor { get; set; }
        public List<Actuador> Actuadores { get; set; }

		public Regla() { }

		public Regla(int regla, int sensor, string detalle, string operador, int valor, string accion, List<Actuador> actuadores)
		{
			idRegla = regla;
			idSensor = sensor;
			Detalle = detalle;
			Operador = operador;
			Valor = valor;
			Accion = accion;
			Actuadores = actuadores;
		}

		public void Cambio(int mag)
        {
			if (Operacion(mag, Operador, Valor)) Actuar();
		}

		private bool Operacion(int x, string operador, int y)
		{
			switch (operador)
			{
				case "<": return x < y;					
				case ">": return x > y;
				case "<=": return x <= y;
				case ">=": return x >= y;
				case "==": return x == y;
				case "!=": return x != y;
				default: return false;
			}
		}

		private void Actuar()
		{
			switch (Accion)
			{
				case "Apagar": Actuadores.ForEach(x => x.Apagar()); break;					
				case "Encender": Actuadores.ForEach(x => x.Encender()); break;
				case "ModoAhorro": Actuadores.ForEach(x => x.ModoAhorro()); break;
				case "SubirTemperatura": Actuadores.ForEach(x => x.SubirTemperatura()); break;
				case "BajarTemperatura": Actuadores.ForEach(x => x.BajarTemperatura()); break;
				case "SubirIntensidad": Actuadores.ForEach(x => x.SubirIntensidad()); break;
				case "BajarIntensidad": Actuadores.ForEach(x => x.BajarIntensidad()); break;
				default: return;
			}
		}

		public string GetExpresion()
		{
			return Detalle + " " + Operador + " " + Valor + " => " + Accion;
		}
    }
}