using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using tp_integrador.Models;
using System.IO;


namespace tp_integrador.Controllers
{
    public class HomeController : Controller
    {
        private ActionResult PermisoDenegado()
        {
            return PartialView("_NotFound"); ;
        }

		[HttpPost]
		[AllowAnonymous]
		public ActionResult Login(Usuarios model)
		{
			// DAOUsuario data = new DAOUsuario();
			string p = model.password;

			int sesionid = ORM.Instancia.GetIDUsuarioIfExists(model.usuario, HashThis.Instancia.GetHash(p));
			Session["IDUsuario"] = sesionid;
			//Usuarios u =  data.InicioSecion(model);
			if ((int)Session["IDUsuario"] <= 0)
			{
				ModelState.AddModelError(String.Empty,"Usuario y/o Contraseña incorecto/s");
				return View("Index",model);
			}
			else
			{
				dynamic user = ORM.Instancia.GetUsuario((int)Session["IDUsuario"]);

				Session["Usuario"] = user;

				//u.SetLoginOn();
				if (user.GetType() == typeof(Administrador))
				{
					DAOUsuario.Instancia.CargarUsuario((Administrador)Session["Usuario"]);
					Session["Admin"] = true;
					return Administrador();
				}
				else
				{
					DAOUsuario.Instancia.CargarUsuario((Cliente)Session["Usuario"]);
					Session["Admin"] = false;
					return Cliente();
				}
			}
		}

		public ActionResult Administrador()
		{
			if (!SessionStateOK()) return View("Index");
			if(!(Boolean)Session["Admin"]) return PermisoDenegado();

			var admin = (Administrador)Session["Usuario"];

			return View("Administrador", model: admin);
		}

		public ActionResult Cliente()
		{
			if (!SessionStateOK()) return View("Index");
			if ((Boolean)Session["Admin"]) return PermisoDenegado();
			var user = (Cliente)Session["Usuario"];

			return View("../Cliente/Cliente", model: user );
		}

		public ActionResult Logout()
        {
			if (!SessionStateOK()) return View("Index");

			Session.Contents.RemoveAll();
            return View("Index");
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        // ADMINISTRADOR
        public ActionResult DatosAdministrador()
        {
            if (!SessionStateOK()) return View("Index");
            if (!(bool)Session["Admin"]) return PermisoDenegado();

            Administrador adm = (Administrador)Session["Usuario"];

            return View(adm);
        }

		public ActionResult EditarAdministrador()
		{
			if (!SessionStateOK()) return View("Index");
			if (!(bool)Session["Admin"]) return PermisoDenegado();

			Administrador adm = (Administrador)Session["Usuario"];
			
			return View("EditarAdministrador", adm);
		}

		[HttpPost]
        public ActionResult EditarAdministrador(Administrador modificado)
        {
            if (!SessionStateOK()) return View("Index");
            if (!(bool)Session["Admin"]) return PermisoDenegado();

            Administrador adm = (Administrador)Session["Usuario"];

			try
			{
				adm.UpdateMyData(modificado);

				return DatosAdministrador();
			}
			catch (Exception ex)
			{
				TempData["MsgState"] = "alert-danger";
				TempData["Alerta"] = "Datos No Modificados.";
				TempData["Mensaje"] = "Error al Modificar los datos, compruebe los datos ingresados.";
			}

			return View("EditarAdministrador", adm);            
        }

		public ActionResult EditPasswordAdmin()
		{
			if (!SessionStateOK()) return View("Index");
			if (!(bool)Session["Admin"]) return PermisoDenegado();

			return View("EditPasswordAdmin");
		}

		[HttpPost]
		public ActionResult EditPasswordAdmin(PasswordDataModel data)
		{
			if (!SessionStateOK()) return View("Index");
			if (!(bool)Session["Admin"]) return PermisoDenegado();

			Administrador adm = (Administrador)Session["Usuario"];

			if (data.IsOK(adm.password))
			{
				adm.CambiarContrasenia(data.NewPasswordHash);

				return DatosAdministrador();
			}
			else
			{
				TempData["MsgState"] = "alert-danger";
				TempData["Alerta"] = "Contraseña No Modificada.";
				TempData["Mensaje"] = "Contraseña incorrecta o las contraseñas ingresadas no coinciden.";
			}

			return View("EditPasswordAdmin", data);
		}

		public ActionResult JsonImport()
        {
			if (!SessionStateOK()) return View("Index");
			if (!(bool)Session["Admin"]) return PermisoDenegado();

			var admin = (Administrador)Session["Usuario"];

            return View("JsonImport", admin);
        }

        public ActionResult AltaAdmin()
        {
            if (!SessionStateOK()) return View("Index");
            if (!(bool)Session["Admin"]) return PermisoDenegado();
            
            return View();
        }

		[HttpPost]
		public ActionResult AltaAdmin(Administrador administrador)
		{
			if (!SessionStateOK()) return View("Index");
			if (!(bool)Session["Admin"]) return PermisoDenegado();

			Administrador admin = (Administrador)Session["Usuario"];

			if (!admin.NuevoAdministrador(administrador))
			{
				TempData["MsgState"] = "alert-danger";
				TempData["Alerta"] = "Administrador No Guardado.";
				TempData["Mensaje"] = "El Username ya existe, intente con otro.";

				return View("AltaAdmin", administrador);
			}
			else
			{
				TempData["MsgState"] = "alert-success";
				TempData["Alerta"] = "Administrador";
				TempData["Mensaje"] = "Creado Correctamente :D";
			}

			return JsonImport();
		}

		public ActionResult AltaCliente()
        {
            if (!SessionStateOK()) return View("Index");
            if (!(bool)Session["Admin"]) return PermisoDenegado();

			return View();
        }		       

        [HttpPost]
        public ActionResult AltaCliente(Cliente cliente)
        {
            if (!SessionStateOK()) return View("Index");
            if (!(bool)Session["Admin"]) return PermisoDenegado();
          
            Administrador admin = (Administrador)Session["Usuario"];

			if (!admin.NuevoCliente(cliente))
			{
				TempData["MsgState"] = "alert-danger";
				TempData["Alerta"] = "Cliente No Guardado.";
				TempData["Mensaje"] = "El Username ya existe, intente con otro.";
				
				return View("AltaCliente", cliente);
			}
			else
			{
				TempData["MsgState"] = "alert-success";
				TempData["Alerta"] = "Cliente";
				TempData["Mensaje"] = "Creado Correctamente :D";
			}

			return JsonImport();
        }

        [HttpPost]
        public ActionResult CargarArchivoAdmins(HttpPostedFileBase user_file)
        {
			if (!SessionStateOK()) return View("Index");
			if (!(bool)Session["Admin"]) return PermisoDenegado();
            if (user_file == null) return JsonImport();

			Administrador adm = (Administrador)Session["Usuario"];
            
			try
			{
				adm.CargarAdmins(user_file);

				TempData["MsgState"] = "alert-success";
				TempData["Alerta"] = "Administradores";
				TempData["Mensaje"] = "Cargados Correctamente :D";
			}
			catch (Exception ex)
			{
				TempData["MsgState"] = "alert-danger";
				TempData["Alerta"] = "Administradores No Guardados";
				TempData["Mensaje"] = "Error al cargar el archivo, revise que el archivo o su contenido sean correctos.";
			}

			return JsonImport();
		}

		[HttpPost]
		public ActionResult CargarArchivoClientes(HttpPostedFileBase user_file)
		{
			if (!SessionStateOK()) return View("Index");
			if (!(bool)Session["Admin"]) return PermisoDenegado();
			if (user_file == null) return JsonImport();

			Administrador adm = (Administrador)Session["Usuario"];
			
			try
			{
				adm.CargarClientes(user_file);

				TempData["MsgState"] = "alert-success";
				TempData["Alerta"] = "Clientes";
				TempData["Mensaje"] = "Cargados Correctamente :D";
			}
			catch (Exception ex)
			{
				TempData["MsgState"] = "alert-danger";
				TempData["Alerta"] = "Clientes No Guardados";
				TempData["Mensaje"] = "Error al cargar el archivo, revise que el archivo o su contenido sean correctos.";
			}

			return JsonImport();
		}

		public ActionResult Maps()
		{
			return View();
		}

		[HttpGet]
		public JsonResult GetTransData()
		{
			var transformadores = DAOzona.Instancia.GetTransformadores();
			
			return Json(transformadores, JsonRequestBehavior.AllowGet);
		}

		public ActionResult Reportes()
        { // inteligente vs estandar
			if (!SessionStateOK()) return View("Index");
			if (!(bool)Session["Admin"]) return PermisoDenegado();
            Administrador adm = (Administrador)Session["Usuario"];
            Session["inteligente"] = adm.GetInteligenteVsEstandar()[0];
            Session["estandar"] = adm.GetInteligenteVsEstandar()[1];


            return View("../Reportes/Reportes");
        }

        public ActionResult ReportePorTransformador()
        {
            if (!SessionStateOK()) return View("Index");
            if (!(bool)Session["Admin"]) return PermisoDenegado();
            Administrador adm = (Administrador)Session["Usuario"];
            List<Transformador> list = adm.GetTransformadors();
            Session["Transformadores"] = list;

            return View("../Reportes/ReportePorTransformador", list);
        }

        public ActionResult ReportesPorCliente()
        {
            if (!SessionStateOK()) return View("Index");
            if (!(bool)Session["Admin"]) return PermisoDenegado();
            Administrador adm = (Administrador)Session["Usuario"];
            List<Cliente> clientes = adm.GetClientes();
            Session["clientes"] = clientes;

            return View("../Reportes/ReportesPorCliente", clientes);
        }

        public ActionResult CargarTransformadores()
        {
			if (!SessionStateOK()) return View("Index");
			if (!(bool)Session["Admin"]) return PermisoDenegado();
			
            return View("CargarTransformadores");
        }

        public ActionResult AltaDispositivo()
        {
            if (!SessionStateOK()) return View("Index");
            if (!(bool)Session["Admin"]) return PermisoDenegado();

            return View();
        }

        [HttpPost]
        public ActionResult AltaDispositivo(DispositivoGenerico dispositivoGenerico)
        {
            if (!SessionStateOK()) return View("Index");
            if (!(bool)Session["Admin"]) return PermisoDenegado();
			
            Administrador adm = (Administrador)Session["Usuario"];
			
			if (!adm.NuevoTemplateDisp(dispositivoGenerico))
			{
				TempData["MsgState"] = "alert-danger";
				TempData["Alerta"] = "Dispositivo No Guardado.";
				TempData["Mensaje"] = "El Dispositivo ya Existe.";

				return View("AltaDispositivo", dispositivoGenerico);
			}
			else
			{
				TempData["MsgState"] = "alert-success";
				TempData["Alerta"] = "Dispositivo Generico";
				TempData["Mensaje"] = "Creado Correctamente :D";
			}


			return View("AltaDispositivo");
        }

        public ActionResult ConsumoHogar()
        {
            if (!SessionStateOK()) return View("Index");
            if (!(bool)Session["Admin"]) return PermisoDenegado();

			Administrador adm = (Administrador)Session["Usuario"];

			return View("ConsumoHogar", adm);
        }

        [HttpPost]
        public ActionResult LoadTransformadoresJson(HttpPostedFileBase user_file)
        {
			if (!SessionStateOK()) return View("Index");
			if (!(bool)Session["Admin"]) return PermisoDenegado();

            if (user_file == null) return CargarTransformadores();

            Administrador adm = (Administrador)Session["Usuario"];

			try
			{
				adm.CargarTransformador(user_file);

				TempData["MsgState"] = "alert-success";
				TempData["Alerta"] = "Transformadores";
				TempData["Mensaje"] = "Cargados Correctamente :D";
			}
			catch (Exception ex)
			{
				TempData["MsgState"] = "alert-danger";
				TempData["Alerta"] = "Transformadores No Guardados";
				TempData["Mensaje"] = "Error al cargar el archivo, revise que el archivo o su contenido sean correctos.";
			}
			
			return View("CargarTransformadores");
        }

		[HttpPost]
		public ActionResult ConsultarConsumo(int idCliente)
		{
			if (!SessionStateOK()) return View("Index");
			if (!(bool)Session["Admin"]) return PermisoDenegado();

			var cliente = DAOUsuario.Instancia.GetClienteFromDB(idCliente);

			return View("ConsultaConsumoHogar", cliente);
		}

		[HttpPost]
		public ActionResult ModificarCliente(int idCliente)
		{
			if (!SessionStateOK()) return View("Index");
			if (!(bool)Session["Admin"]) return PermisoDenegado();

			return EditarCliente(idCliente);
		}

		public ActionResult EditarCliente(int idCliente)
		{
			if (!SessionStateOK()) return View("Index");
			if (!(bool)Session["Admin"]) return PermisoDenegado();

			var cliente = DAOUsuario.Instancia.GetClienteFromDB(idCliente);

			return View("EditarCliente", cliente);
		}

		[HttpPost]
		public ActionResult EditarCliente(Cliente modificado)
		{
			if (!SessionStateOK()) return View("Index");
			if (!(bool)Session["Admin"]) return PermisoDenegado();

			var cliente = DAOUsuario.Instancia.GetClienteFromDB(modificado.idUsuario);

			try
			{
				cliente.UpdateMyData(modificado);

				return JsonImport();
			}
			catch (Exception ex)
			{
				TempData["MsgState"] = "alert-danger";
				TempData["Alerta"] = "Datos No Modificados.";
				TempData["Mensaje"] = "Error al Modificar los datos, compruebe los datos ingresados.";
			}

			return View("EditarCliente", modificado);
		}

		public ActionResult ConsumoZonas()
		{
			if (!SessionStateOK()) return View("Index");
			if (!(bool)Session["Admin"]) return PermisoDenegado();

			var lista = DAOzona.Instancia.zonas;

			return View("ConsumoZonas", lista);
		}

		public bool SessionStateOK()
		{
			if (Session["Usuario"] == null) return false;
			if (Session["Admin"] == null) Session["Admin"] = (Session["Usuario"].GetType() == typeof(Administrador));
			return true;			
		}
    }
}
