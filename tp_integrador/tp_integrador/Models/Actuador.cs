using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace tp_integrador.Models
{
	public class Actuador
	{
		public int IdActuador { get; set; }
		public string ActuadorTipo { get; set; }
		public int IdCliente { get; set; }
		public List<int> Reglas { get; set; }
        public List<Inteligente> Dispositivos { get; set; }

		public Actuador() { }

		public Actuador(int actuador, string detalle, List<int> reglas, int cliente, List<Inteligente> disp)
		{
			IdActuador = actuador;
			ActuadorTipo = detalle;
			Reglas = reglas;
			IdCliente = cliente;
			Dispositivos = disp;
		}
		
		public void Apagar()
		{
			Dispositivos.ForEach(x => x.Apagar());
		}

		public void Encender()
		{
			Dispositivos.ForEach(x => x.Encender());
		}

		public void ModoAhorro()
		{
			Dispositivos.ForEach(x => x.ModoAhorro());
		}

		public void BajarTemperatura()
		{
			Dispositivos.ForEach(x => x.BajarTemperatura());
		}

		public void SubirTemperatura()
		{
			Dispositivos.ForEach(x => x.SubirTemperatura());
		}

		public void BajarIntensidad()
		{
			Dispositivos.ForEach(x => x.BajarIntensidad());
		}

		public void SubirIntensidad()
		{
			Dispositivos.ForEach(x => x.SubirIntensidad());
		}

		public void AgregarDispositivo(Inteligente dispositivo)
		{
			Dispositivos.Add(dispositivo);			
		}

		public void QuitarDispositivo(Inteligente dispositivo)
		{
			Dispositivos.Remove(dispositivo);			
		}

		public void AgregarRegla(int idRegla)
		{
			Reglas.Add(idRegla);			
		}

		public void QuitarRegla(int idRegla)
		{
			Reglas.Remove(idRegla);			
		}
	}
}