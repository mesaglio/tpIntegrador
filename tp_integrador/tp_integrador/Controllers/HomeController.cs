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

			return View("Administrador");
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
        public ActionResult JsonImport()
        {
			if (!SessionStateOK()) return View("Index");
			if (!(bool)Session["Admin"]) return PermisoDenegado();
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult AltaAdmin()
        {
            if (!SessionStateOK()) return View("Index");
            if (!(bool)Session["Admin"]) return PermisoDenegado();
            
            return View();
        }

        public ActionResult AltaCliente()
        {
            if (!SessionStateOK()) return View("Index");
            if (!(bool)Session["Admin"]) return PermisoDenegado();

            return View();
        }

        [HttpPost]
        public ActionResult NuevoUsuario(Administrador administrador)
        {
            if (!SessionStateOK()) return View("Index");
            if (!(bool)Session["Admin"]) return PermisoDenegado();
            Administrador adm = (Administrador)Session["Usuario"];
            //crear el administrador
            return View();
        }

        [HttpPost]
        public ActionResult NuevoUsuario(Cliente cliente)
        {
            if (!SessionStateOK()) return View("Index");
            if (!(bool)Session["Admin"]) return PermisoDenegado();
            Administrador adm = (Administrador)Session["Usuario"]; 
            // crear el cliente
            return View();
        }

        [HttpPost]
        public ActionResult CargarArchivoAdmins(HttpPostedFileBase user_file)
        {
			if (!SessionStateOK()) return View("Index");
			if (!(bool)Session["Admin"]) return PermisoDenegado();
            if (user_file == null) return View("JsonImport");

            Administrador adm = (Administrador)Session["Usuario"];
            adm.CargarAdmins(user_file);

			return View("JsonImport");
        }

		[HttpPost]
		public ActionResult CargarArchivoClientes(HttpPostedFileBase user_file)
		{
			if (!SessionStateOK()) return View("Index");
			if (!(bool)Session["Admin"]) return PermisoDenegado();
			if (user_file == null) return View("JsonImport");

			Administrador adm = (Administrador)Session["Usuario"];
			adm.CargarClientes(user_file);

			return View("JsonImport");
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

        [HttpPost]
        public ActionResult LoadTransformadoresJson(HttpPostedFileBase file)
        {
			if (!SessionStateOK()) return View("Index");
			if (!(bool)Session["Admin"]) return PermisoDenegado();

            if (file == null) return CargarTransformadores();

            Administrador adm = (Administrador)Session["Usuario"];
            adm.CargarTransformador(file);

            return View();
        }

		public bool SessionStateOK()
		{
			if (Session["Usuario"] == null) return false;
			if (Session["Admin"] == null) Session["Admin"] = (Session["Usuario"].GetType() == typeof(Administrador));
			return true;			
		}
    }
}
