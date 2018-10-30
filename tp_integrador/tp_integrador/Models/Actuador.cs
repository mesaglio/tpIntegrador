using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace tp_integrador.Models
{
	public class Actuador
	{
		public int idActuador { get; set; }		
		public string actuadorTipo { get; set; }		
        List<Inteligente> Dispositivos { get; set; }

		public Actuador(int actuador, string detalle, List<Inteligente> disp)
		{
			idActuador = actuador;
			actuadorTipo = detalle;
			Dispositivos = disp;
		}

        // TODO: hacer la logica para mandar acciones al dispositivo
        public void Actuar()
        {
        }
    }
}