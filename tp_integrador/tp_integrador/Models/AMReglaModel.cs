using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace tp_integrador.Models
{
	public class AMReglaModel
	{
		public Regla Regla { get; set; }
		public List<int> ActuadoresID { get; set; }
		public List<SelectListItem> Operadores { get; set; }
		public List<SelectListItem> Acciones { get; set; }
		public SelectList Actuadores { get; set; }
		public SelectList Sensores { get; set; }

		public AMReglaModel() { ActuadoresID = new List<int>(); }

		public void LoadDataFor(int idCliente)
		{			
			Operadores = new List<SelectListItem> {
				new SelectListItem { Value = "<", Text = "<"},
				new SelectListItem { Value = ">", Text = ">"},
				new SelectListItem { Value = "<=", Text = "<="},
				new SelectListItem { Value = ">=", Text = ">="},
				new SelectListItem { Value = "==", Text = "=="},
				new SelectListItem { Value = "!=", Text = "!="}
			};

			Acciones = new List<SelectListItem> {
				new SelectListItem { Value = "Encender", Text = "Encender"},
				new SelectListItem { Value = "Apagar", Text = "Apagar"},
				new SelectListItem { Value = "ModoAhorro", Text = "Modo Ahorro"},
				new SelectListItem { Value = "SubirTemperatura", Text = "Subir Temperatura"},
				new SelectListItem { Value = "BajarTemperatura", Text = "Bajar Temperatura"},
				new SelectListItem { Value = "SubirIntensidad", Text = "Subir Intensidad"},
				new SelectListItem { Value = "BajarIntensidad", Text = "Bajar Intensidad"}
			};

			Actuadores = new SelectList(DAOSensores.Instancia.FindActuadoresCliente(idCliente), "IdActuador", "ActuadorTipo");

			Sensores = new SelectList(DAOSensores.Instancia.FindAllFromCliente(idCliente), "idSensor", "TipoSensor");
		}

		public void SelectActuadoresID()
		{
			if (Regla == null) return;
			if (Regla.Actuadores == null || Regla.Actuadores.Count == 0) return;

			var lista = new List<int>();

			foreach (var actuador in Regla.Actuadores)
			{
				lista.Add(actuador.IdActuador);
			}

			ActuadoresID = lista;			
		}
	}
}