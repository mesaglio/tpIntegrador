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
        private List<EstadoGuardado> nuevosEstados;

        public DAOHistorialEstado(int user, int disp, int num)
        {
            usuario = user;
            dispositivo = disp;
			numero = num;
            nuevosEstados = new List<EstadoGuardado>();
        }

		//TODO: WIP
		public List<EstadoGuardado> GetEstados(DateTime desde, DateTime hasta)
        {
            List<EstadoGuardado> solicitados = new List<EstadoGuardado>();
            //Obtener Estados de DB 
            foreach(EstadoGuardado guardado in nuevosEstados)
            {
                EstadoGuardado estado = guardado.GetEstadoEntre(desde, hasta);
                if (estado != null) { solicitados.Add(estado); }               
            }
            return solicitados;
        }

        public void CargarEstado(byte estado, DateTime desde, DateTime hasta)
        {
			nuevosEstados.Add(new EstadoGuardado(usuario, dispositivo, numero, estado, desde, hasta));
        }

    }
}