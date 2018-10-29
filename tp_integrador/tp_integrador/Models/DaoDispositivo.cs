﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace tp_integrador.Models
{
    public class DAODispositivo
    {
		private static DAODispositivo instancia;
        public List<Dispositivo> listdispositivos;

		public DAODispositivo()
        {
			listdispositivos = new List<Dispositivo>();
        }

		public static DAODispositivo Instancia
		{
			get
			{
				if (instancia == null) instancia = new DAODispositivo();
				return instancia;
			}
		}

		public void InitialLoad()
		{
			if (listdispositivos.Count != 0) return;

			listdispositivos = ORM.Instancia.GetAllDispositivos();
		}

		public void CargarDispositivo(Dispositivo dispositivo)
        {
            listdispositivos.Add(dispositivo);
        }

		public List<Dispositivo> FindAllFromCliente(int idCliente)
		{
			var lista = new List<Dispositivo>();
			foreach (var d in listdispositivos) if (d.IdCliente == idCliente) lista.Add(d);

			return lista;
		}
        
    }
}