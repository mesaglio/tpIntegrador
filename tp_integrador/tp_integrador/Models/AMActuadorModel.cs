using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace tp_integrador.Models
{
	public class AMActuadorModel
	{
		public Actuador Actuador { get; set; }
		public List<string> DispositivosID { get; set; }
		public List<SelectListItem> DispositivosCliente { get; set; }
		public List<SelectListItem> ReglasCliente { get; set; }

		public AMActuadorModel() { DispositivosID = new List<string>(); }

		public void LoadDataFor(List<Dispositivo> dispositivos)
		{
			if (dispositivos.Count == 0) return;
			var idCliente = dispositivos[0].IdCliente;

			DispositivosCliente = new List<SelectListItem>();

			string value = "{0},{1},{2}";
			foreach (var dispo in dispositivos.OfType<Inteligente>())
			{
				DispositivosCliente.Add(new SelectListItem { Value = String.Format(value, dispo.IdDispositivo, dispo.IdCliente, dispo.Numero), Text = dispo.Nombre });
			}

			ReglasCliente = new List<SelectListItem>();
			foreach (var regla in DAOSensores.Instancia.FindReglasCliente(idCliente))
			{
				ReglasCliente.Add(new SelectListItem { Value = regla.idRegla.ToString(), Text = regla.GetExpresion() });
			}
		}

		public List<DispositivoData> ParseDispositivosID()
		{			
			var data = new List<DispositivoData>();

			foreach (var codigos in DispositivosID)
			{
				var lista = codigos.Split(',');
				data.Add(new DispositivoData() { IdD = Int32.Parse(lista[0]), IdC = Int32.Parse(lista[1]), Num = Int32.Parse(lista[2]) });
			}

			return data;
		}

		public void SelectReglasDispositivos()
		{
			if (Actuador == null) return;			
			if (Actuador.Dispositivos == null) return;
						
			var listaDispositivos = new List<string>();

			string value = "{0},{1},{2}";
			foreach (var dispo in Actuador.Dispositivos)
			{
				listaDispositivos.Add(String.Format(value, dispo.IdDispositivo, dispo.IdCliente, dispo.Numero));
			}

			DispositivosID = listaDispositivos;

		}

		public class DispositivoData
		{
			public int IdD { get; set; }
			public int IdC { get; set; }
			public int Num { get; set; }
		}
	}
}