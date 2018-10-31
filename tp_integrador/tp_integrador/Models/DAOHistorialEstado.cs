using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace tp_integrador.Models
{
    public class DAOHistorialEstado
    {		
        private int usuario;
        private int dispositivo;
		private int numero;        

        public DAOHistorialEstado(int user, int disp, int num)
        {
            usuario = user;
            dispositivo = disp;
			numero = num;
        }
		
		public List<EstadoGuardado> GetEstados(DateTime desde, DateTime hasta)
        {
            List<EstadoGuardado> solicitados = new List<EstadoGuardado>();
			EstadoGuardado estado;

			var fromDB = ORM.Instancia.GetEstadosEntre(usuario, dispositivo, numero, desde, hasta);

			if (fromDB.Count > 0)
			{
				estado = fromDB[0].GetEstadoEntre(desde, hasta);
				if (estado != null) solicitados.Add(estado);

				if (fromDB.Count > 1)
				{
					solicitados.AddRange(fromDB.GetRange(1, fromDB.Count - 2));
					estado = fromDB[fromDB.Count - 1].GetEstadoEntre(desde, hasta);
					if (estado != null) solicitados.Add(estado);
				}
			}
			
            return solicitados;
        }

        public void CargarEstado(byte estado, DateTime desde, DateTime hasta)
        {						
			ORM.Instancia.Insert(new EstadoGuardado(usuario, dispositivo, numero, estado, desde, hasta));
        }

    }
}