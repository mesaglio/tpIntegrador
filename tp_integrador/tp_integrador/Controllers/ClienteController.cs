using System.Web;
using System.Web.Mvc;
using tp_integrador.Models;
using tp_integrador.Controllers;
using System;
using System.Collections.Generic;
using System.Text;
using System.Web.Services;
using System.Linq;

namespace tp_integrador.Controllers
{
	public class ClienteController : Controller
	{
		private Cliente Tocliente(Usuarios user) => (Cliente)user;

		private ActionResult PermisoDenegado()
		{
			return PartialView("_NotFound");
		}

		// GET: Cliente

		public ActionResult GestionarDispositivos()
		{
			if (!SessionStateOK()) return View("Index");
			if ((bool)Session["Admin"]) return PermisoDenegado();

			Cliente user = (Cliente)Session["Usuario"];


			return View("GestionarDispositivos", user);
		}

		public ActionResult ConsumoPorPeriodo()// anio actuak
		{
			if (!SessionStateOK()) return View("Index");
			if ((Boolean)Session["Admin"]) return PermisoDenegado();
			Cliente user = (Cliente)Session["Usuario"];
			DateTime localDate = DateTime.Now;
			Session["anio"] = localDate.Year;
			return View(user);
		}

		public ActionResult ConsumoPorPeriodoAnterior()// anio anterior
		{
			if (!SessionStateOK()) return View("Index");
			if ((Boolean)Session["Admin"]) return PermisoDenegado();
			Cliente user = (Cliente)Session["Usuario"];
			Int32 var = (Int32)Session["anio"];
			Session["anio"] = (var - 1);
			return View("ConsumoPorPeriodo", user);
		}


		public ActionResult ConsumoPorPeriodoProximo() // anio siguiente
		{
			if (!SessionStateOK()) return View("Index");
			if ((Boolean)Session["Admin"]) return PermisoDenegado();
			Cliente user = (Cliente)Session["Usuario"];
			Int32 var = (Int32)Session["anio"];
			Session["anio"] = (var + 1);
			return View("ConsumoPorPeriodo", user);
		}

		public ActionResult Simplex()
		{
			if (!SessionStateOK()) return View("Index");
			if ((bool)Session["Admin"]) return PermisoDenegado();

			Cliente user = (Cliente)Session["Usuario"];

			return View("Simplex", user);
		}

		public ActionResult CargarArchivoDispositivos()
		{
			if (!SessionStateOK()) return View("Index");
			if ((bool)Session["Admin"]) return PermisoDenegado();
			// trae los templates para que el cliente sume un dispositivo

			return View("CargarArchivoDispositivos", model: false);
		}

		public ActionResult GestionarSensores()
		{
			if (!SessionStateOK()) return View("Index");
			if ((bool)Session["Admin"]) return PermisoDenegado();
			Cliente user = (Cliente)Session["Usuario"];

			return View("GestionarSensores", user);
		}

		[HttpPost]
		public ActionResult LoadDispositivoJson(HttpPostedFileBase user_file)
		{
			if (!SessionStateOK()) return View("Index");
			if ((bool)Session["Admin"]) return PermisoDenegado();

			if (user_file == null) return CargarArchivoDispositivos();

			Cliente unclietne = (Cliente)Session["Usuario"];
			
			try
			{				
				unclietne.CargarDispositivos(user_file, 1); // INTELIGENTE = 1

				TempData["MsgState"] = "alert-success";
				TempData["Alerta"] = "Dispositivos Inteligentes";
				TempData["Mensaje"] = "Cargados Correctamente :D";				
			}
			catch (Exception ex)
			{
				TempData["MsgState"] = "alert-danger";
				TempData["Alerta"] = "Dispositivos Inteligentes No Guardados";
				TempData["Mensaje"] = "Error al cargar el archivo, revise que el archivo o su contenido sean correctos.";
			}

			return View("CargarArchivoDispositivos", model: true);
		}

		[HttpPost]
		public ActionResult LoadDispositivoJsone(HttpPostedFileBase user_file)
		{
			if (!SessionStateOK()) return View("Index");
			if ((bool)Session["Admin"]) return PermisoDenegado();

			if (user_file == null) return CargarArchivoDispositivos();

			Cliente unclietne = (Cliente)Session["Usuario"];
			
			try
			{
				unclietne.CargarDispositivos(user_file, 0); // ESTANDAR = 0

				TempData["MsgState"] = "alert-success";
				TempData["Alerta"] = "Dispositivos Estandar";
				TempData["Mensaje"] = "Cargados Correctamente :D";
			}
			catch (Exception ex)
			{
				TempData["MsgState"] = "alert-danger";
				TempData["Alerta"] = "Dispositivos Estandar No Guardados";
				TempData["Mensaje"] = "Error al cargar el archivo, revise que el archivo o su contenido sean correctos.";
			}

			return View("CargarArchivoDispositivos", model: true);
		}

		[HttpPost]
		[AllowAnonymous]
		public ActionResult SelecTemplate_dis(int disp)
		{
			if (!SessionStateOK()) return View("Index");
			if ((bool)Session["Admin"]) return PermisoDenegado();
			Cliente unclietne = (Cliente)Session["Usuario"];

			if (disp == 0) return View("Dashboard", model: unclietne);
			else
			{
				unclietne.AgregarDispositivoDeTemplate(disp);

				return View("GestionarDispositivos", model: unclietne);
			}
		}

		public ActionResult EstadoDispositivo(int idD, int idC, int numero)
		{
			if (!SessionStateOK()) return View("Index");
			if ((bool)Session["Admin"]) return PermisoDenegado();

			Cliente uncliente = (Cliente)Session["Usuario"];
			if (uncliente.idUsuario != idC) return PermisoDenegado();

			uncliente.CambiarEstado(uncliente.BuscarDispositivo(idD, idC, numero));
			return View("GestionarDispositivos", model: uncliente);
		}

		public ActionResult EditarEstandar(int idD, int idC, int numero)
		{
			if (!SessionStateOK()) return View("Index");
			if ((bool)Session["Admin"]) return PermisoDenegado();
			
			Cliente uncliente = (Cliente)Session["Usuario"];
			if (uncliente.idUsuario != idC) return PermisoDenegado();

			var dispositivo = uncliente.BuscarDispositivo(idD, idC, numero);
			return View(dispositivo);
		}

		[HttpPost]
		public ActionResult EditarEstandar(Estandar disp)
		{
			if (!SessionStateOK()) return View("Index");
			if ((bool)Session["Admin"]) return PermisoDenegado();

			Cliente uncliente = (Cliente)Session["Usuario"];

			uncliente.UsoDiario(uncliente.BuscarDispositivo(disp.IdDispositivo, disp.IdCliente, disp.Numero), disp.usoDiario);

			return View("GestionarDispositivos", model: uncliente);
		}

		public ActionResult ConvertirEstandar(int idD, int idC, int numero)
		{
			if (!SessionStateOK()) return View("Index");
			if ((bool)Session["Admin"]) return PermisoDenegado();

			Cliente uncliente = (Cliente)Session["Usuario"];
			if (uncliente.idUsuario != idC) return PermisoDenegado();

			uncliente.ConvertirAInteligente(uncliente.BuscarDispositivo(idD, idC, numero));

			return View("GestionarDispositivos", model: uncliente);
		}

		public ActionResult CalculoSimplex()
		{
			if (!SessionStateOK()) return View("Index");
			if ((bool)Session["Admin"]) return PermisoDenegado();

			Cliente uncliente = (Cliente)Session["Usuario"];
			var resultado = uncliente.RunSimplex();

			return View("RespuestaSimplex", model: resultado);
		}

		[HttpPost]
		[AllowAnonymous]
		public ActionResult AhorroAutomatico()
		{
			if (!SessionStateOK()) return View("Index");
			if ((bool)Session["Admin"]) return PermisoDenegado();

			Cliente uncliente = (Cliente)Session["Usuario"];
			uncliente.CambiarAutoSimplex();

			return View("Simplex", model: uncliente);
		}

		public ActionResult AltaSensor()
		{
			if (!SessionStateOK()) return View("Index");
			if ((bool)Session["Admin"]) return PermisoDenegado();

			return View("AltaSensor");
		}

		[HttpPost]
		public ActionResult AltaSensor(Sensor sensor)
		{
			if (!SessionStateOK()) return View("Index");
			if ((bool)Session["Admin"]) return PermisoDenegado();

			var cliente = (Cliente)Session["Usuario"];
			if (!cliente.NuevoSensor(sensor))
			{
				TempData["Alerta"] = "Sensor No Guardado.";
				TempData["Mensaje"] = "El tipo de Sensor ya existe, intente con otro.";
				return AltaSensor();
			}

			return View("GestionarSensores", model: cliente);
		}

		public ActionResult AltaRegla()
		{
			if (!SessionStateOK()) return View("Index");
			if ((bool)Session["Admin"]) return PermisoDenegado();

			var cliente = (Cliente)Session["Usuario"];
			var model = new AMReglaModel();
			model.LoadDataFor(cliente.idUsuario);

			return View("AltaRegla", model);
		}

		[HttpPost]
		public ActionResult AltaRegla(AMReglaModel data)
		{
			if (!SessionStateOK()) return View("Index");
			if ((bool)Session["Admin"]) return PermisoDenegado();

			var cliente = (Cliente)Session["Usuario"];

			var regla = data.Regla;
			regla.Actuadores = new List<Actuador>();

			foreach (var codigo in data.ActuadoresID)
			{
				regla.Actuadores.Add(DAOSensores.Instancia.GetActuador(codigo));
			}

			if (!cliente.NuevaRegla(regla))
			{
				TempData["Alerta"] = "Regla No Guardada.";
				TempData["Mensaje"] = "Esa regla ya existe.";
				data.LoadDataFor(cliente.idUsuario);
				return View("AltaRegla", data);
			}

			return View("GestionarSensores", model: cliente);
		}

		public ActionResult AltaActuador()
		{
			if (!SessionStateOK()) return View("Index");
			if ((bool)Session["Admin"]) return PermisoDenegado();

			var cliente = (Cliente)Session["Usuario"];

			var model = new AMActuadorModel();
			model.LoadDataFor(cliente.dispositivos);

			return View("AltaActuador", model);
		}

		[HttpPost]
		public ActionResult AltaActuador(AMActuadorModel data)
		{
			if (!SessionStateOK()) return View("Index");
			if ((bool)Session["Admin"]) return PermisoDenegado();

			var cliente = (Cliente)Session["Usuario"];

			var actuador = data.Actuador;
			actuador.Dispositivos = new List<Inteligente>();

			foreach (var codDisp in data.ParseDispositivosID())
			{
				actuador.Dispositivos.Add((Inteligente)cliente.dispositivos.Find(x => x.IdDispositivo == codDisp.IdD && x.IdCliente == codDisp.IdC && x.Numero == codDisp.Num));
			}

			if (!cliente.NuevoActuador(actuador))
			{
				TempData["Alerta"] = "Actuador No Guardado.";
				TempData["Mensaje"] = "Ese Tipo de Actuador ya existe, intente con otro.";
				data.LoadDataFor(cliente.dispositivos);
				return View("AltaActuador", data);
			}

			return View("GestionarSensores", model: cliente);
		}
		
		public ActionResult EditarSensor(int idSensor)
		{
			if (!SessionStateOK()) return View("Index");
			if ((bool)Session["Admin"]) return PermisoDenegado();

			var cliente = (Cliente)Session["Usuario"];
			var sensor = DAOSensores.Instancia.GetSensor(idSensor);
			if (cliente.idUsuario != sensor.idCliente) return PermisoDenegado();

			return View("EditarSensor", model: sensor);
		}

		[HttpPost]
		public ActionResult EditarSensor(Sensor sensor)
		{
			if (!SessionStateOK()) return View("Index");
			if ((bool)Session["Admin"]) return PermisoDenegado();

			var cliente = (Cliente)Session["Usuario"];
			if (!cliente.ModificarSensor(sensor))
			{
				TempData["Alerta"] = "Sensor No Modificado.";
				TempData["Mensaje"] = "El tipo de Sensor ya existe, intente con otro.";
				return View("EditarSensor", model: sensor);
			}

			return View("GestionarSensores", model: cliente);
		}
				
		public ActionResult EditarRegla(int idRegla)
		{
			if (!SessionStateOK()) return View("Index");
			if ((bool)Session["Admin"]) return PermisoDenegado();

			var cliente = (Cliente)Session["Usuario"];
			if (cliente.idUsuario != DAOSensores.Instancia.ClienteIDFrom(idRegla)) return PermisoDenegado();

			var model = new AMReglaModel();
			model.LoadDataFor(cliente.idUsuario);
			model.Regla = DAOSensores.Instancia.GetRegla(idRegla);
			model.SelectActuadoresID();

			return View("EditarRegla", model);
		}

		[HttpPost]
		public ActionResult EditarRegla(AMReglaModel data)
		{
			if (!SessionStateOK()) return View("Index");
			if ((bool)Session["Admin"]) return PermisoDenegado();

			var cliente = (Cliente)Session["Usuario"];

			var regla = data.Regla;
			regla.Actuadores = new List<Actuador>();

			foreach (var codigo in data.ActuadoresID)
			{
				regla.Actuadores.Add(DAOSensores.Instancia.GetActuador(codigo));
			}

			if (!cliente.ModificarRegla(regla))
			{
				TempData["Alerta"] = "Regla No Modificada.";
				TempData["Mensaje"] = "Esa regla ya existe o quito un actuador asignado solo a esta regla";
				data.LoadDataFor(cliente.idUsuario);
				return View("EditarRegla", data);
			}
			
			return View("GestionarSensores", model: cliente);
		}

		public ActionResult EditarActuador(int idActuador)
		{
			if (!SessionStateOK()) return View("Index");
			if ((bool)Session["Admin"]) return PermisoDenegado();

			var cliente = (Cliente)Session["Usuario"];
			var actuador = DAOSensores.Instancia.GetActuador(idActuador);
			if (cliente.idUsuario != actuador.IdCliente) return PermisoDenegado();

			var model = new AMActuadorModel();
			model.LoadDataFor(cliente.dispositivos);
			model.Actuador = actuador;
			model.SelectReglasDispositivos();

			return View("EditarActuador", model);
		}

		[HttpPost]
		public ActionResult EditarActuador(AMActuadorModel data)
		{
			if (!SessionStateOK()) return View("Index");
			if ((bool)Session["Admin"]) return PermisoDenegado();

			var cliente = (Cliente)Session["Usuario"];

			var actuador = data.Actuador;
			actuador.Dispositivos = new List<Inteligente>();

			foreach (var codDisp in data.ParseDispositivosID())
			{
				actuador.Dispositivos.Add((Inteligente)cliente.dispositivos.Find(x => x.IdDispositivo == codDisp.IdD && x.IdCliente == codDisp.IdC && x.Numero == codDisp.Num));
			}

			if (!cliente.ModificarActuador(actuador))
			{
				TempData["Alerta"] = "Actuador No Modificado.";
				TempData["Mensaje"] = "Ese Tipo de Actuador ya existe, intente con otro.";
				data.LoadDataFor(cliente.dispositivos);
				return View("EditarActuador", data);
			}

			return View("GestionarSensores", model: cliente);
		}

		public ActionResult BajaActuador(int idActuador)
		{
			if (!SessionStateOK()) return View("Index");
			if ((bool)Session["Admin"]) return PermisoDenegado();

			var cliente = (Cliente)Session["Usuario"];
			var actuador = DAOSensores.Instancia.GetActuador(idActuador);
			if (cliente.idUsuario != actuador.IdCliente) return PermisoDenegado();

			cliente.EliminarActuador(actuador);

			return View("GestionarSensores", model: cliente);
		}

		public ActionResult BajaRegla(int idRegla)
		{
			if (!SessionStateOK()) return View("Index");
			if ((bool)Session["Admin"]) return PermisoDenegado();

			var cliente = (Cliente)Session["Usuario"];			
			if (cliente.idUsuario != DAOSensores.Instancia.ClienteIDFrom(idRegla)) return PermisoDenegado();

			var regla = DAOSensores.Instancia.GetRegla(idRegla);
			cliente.EliminarRegla(regla);

			return View("GestionarSensores", model: cliente);
		}

		public ActionResult BajaSensor(int idSensor)
		{
			if (!SessionStateOK()) return View("Index");
			if ((bool)Session["Admin"]) return PermisoDenegado();

			var cliente = (Cliente)Session["Usuario"];
			var sensor = DAOSensores.Instancia.GetSensor(idSensor);
			if (cliente.idUsuario != sensor.idCliente) return PermisoDenegado();

			cliente.EliminarSensor(sensor);

			return View("GestionarSensores", model: cliente);
		}

		public ActionResult BajaDispositivo(int idD, int idC, int numero)
		{
			if (!SessionStateOK()) return View("Index");
			if ((bool)Session["Admin"]) return PermisoDenegado();

			var cliente = (Cliente)Session["Usuario"];
			if (cliente.idUsuario != idC) return PermisoDenegado();

			cliente.EliminarDispositivo(idD, idC, numero);

			return View("GestionarDispositivos", model: cliente);
		}

		public ActionResult DatosCliente()
		{
			if (!SessionStateOK()) return View("Index");
			if ((bool)Session["Admin"]) return PermisoDenegado();

			var cliente = (Cliente)Session["Usuario"];

			return View("DatosCliente", cliente);
		}

		public ActionResult EditPasswordCliente()
		{
			if (!SessionStateOK()) return View("Index");
			if ((bool)Session["Admin"]) return PermisoDenegado();

			return View("EditPasswordCliente");
		}

		[HttpPost]
		public ActionResult EditPasswordCliente(PasswordDataModel data)
		{
			if (!SessionStateOK()) return View("Index");
			if ((bool)Session["Admin"]) return PermisoDenegado();

			var cliente = (Cliente)Session["Usuario"];

			if (data.IsOK(cliente.password))
			{
				cliente.CambiarContrasenia(data.NewPasswordHash);

				return DatosCliente();
			}
			else
			{
				TempData["MsgState"] = "alert-danger";
				TempData["Alerta"] = "Contraseña No Modificada.";
				TempData["Mensaje"] = "Contraseña incorrecta o las contraseñas ingresadas no coinciden.";
			}

			return View("EditPasswordCliente", data);
		}

		public bool SessionStateOK()
		{
			if (Session["Usuario"] == null) return false;
			if (Session["Admin"] == null) Session["Admin"] = (Session["Usuario"].GetType() == typeof(Administrador));
			return true;			
		}
	}
}