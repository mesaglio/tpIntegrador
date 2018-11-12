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
					MvcApplication.Daobjeto.CargarUsuario((Administrador)Session["Usuario"]);
					Session["Admin"] = true;
					return View("Administrador");
				}
				else
				{
					MvcApplication.Daobjeto.CargarUsuario((Cliente)Session["Usuario"]);
					Session["Admin"] = false;
					return View("../Cliente/Cliente");
				}
			}
		}

		public ActionResult Administrador()
		{
			return View("Administrador");
		}

		public ActionResult Cliente()
		{
			return View("../Cliente/Cliente");
		}

		public ActionResult Logout()
        {
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
            if (!(bool)Session["Admin"]) return PermisoDenegado();
            ViewBag.Message = "Your contact page.";

            return View();
        }

        [HttpPost]
        public ActionResult CargarArchivo(HttpPostedFileBase file)
        {
            if (!(bool)Session["Admin"]) return PermisoDenegado();
            if (file == null) return View("JsonImport");

            Administrador adm = (Administrador)Session["Usuario"];
            adm.CargarClienter(file);

            return View();
        }

		public ActionResult Maps()
		{
			return View();
		}

		[HttpGet]
		public JsonResult GetTransData()
		{
			var transformadores = DAOzona.Instancia.GetTransformadores();
			return Json(transformadores, "aplication/json", System.Text.Encoding.UTF8, JsonRequestBehavior.AllowGet);
		}

		public ActionResult Reporte()
        {
            if (!(bool)Session["Admin"]) return PermisoDenegado();
			
            return View("../Reportes/Reportes");
        }

        public ActionResult CargarTransformadores()
        {
            if (!(bool)Session["Admin"]) return PermisoDenegado();
			
            return View("CargarTransformadores");
        }

        [HttpPost]
        public ActionResult LoadTransformadoresJson(HttpPostedFileBase file)
        {
            if (!(bool)Session["Admin"]) return PermisoDenegado();

            if (file == null) return CargarTransformadores();

            Administrador adm = (Administrador)Session["Usuario"];
            adm.CargarTransformador(file);

            return View();
        }

    }
}
