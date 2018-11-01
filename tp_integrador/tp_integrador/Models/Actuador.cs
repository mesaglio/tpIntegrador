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
        public List<Inteligente> Dispositivos { get; set; }

		public Actuador(int actuador, string detalle, int cliente, List<Inteligente> disp)
		{
			IdActuador = actuador;
			ActuadorTipo = detalle;
			IdCliente = cliente;
			Dispositivos = disp;
		}

        // TODO: hacer la logica para mandar acciones al dispositivo
        public void Actuar()
        {
        }
    }
}