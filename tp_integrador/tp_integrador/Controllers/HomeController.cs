﻿using System;
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

        [HttpPost]
        public ActionResult CargarArchivo(HttpPostedFileBase file)
        {
			if (!SessionStateOK()) return View("Index");
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
			
			return Json(transformadores, JsonRequestBehavior.AllowGet);
		}

		public ActionResult Reporte()
        {
			if (!SessionStateOK()) return View("Index");
			if (!(bool)Session["Admin"]) return PermisoDenegado();
			
            return View("../Reportes/Reportes");
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
