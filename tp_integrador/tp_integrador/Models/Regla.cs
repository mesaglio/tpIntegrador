using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace tp_integrador.Models
{
    public class Regla
    {
        int regla { get; set; }
        List<Actuador> actuadores { get; set; }
		
		// recive la notificacion de un cambio en magnitud del sensor
		public void Cambio(int mag)
        {
			foreach (Actuador ac in actuadores)
			{
				if (regla == mag) ac.Actuar();
			}
        }
    }
}