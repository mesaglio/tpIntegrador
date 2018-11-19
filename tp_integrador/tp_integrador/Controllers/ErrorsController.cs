using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using tp_integrador.Models;

namespace tp_integrador.Controllers
{
    public class ErrorsController : Controller
    {
        // GET: Errors
        public ActionResult Error404()
        {
			object model = Request.Url.PathAndQuery;

			if (!Request.IsAjaxRequest()) return View(model);
			else return PartialView("_NotFound", model);
        }

		public ActionResult GoBack()
		{
			if (!SessionStateOK()) return View("Index");
			if ((Boolean)Session["Admin"]) return View("Administrador");
			else return View("../Cliente/Cliente", model: (Cliente)Session["Usuario"]);
			
		}

		public bool SessionStateOK()
		{
			if (Session["Usuario"] == null) return false;
			if (Session["Admin"] == null) Session["Admin"] = (Session["Usuario"].GetType() == typeof(Administrador));
			return true;
		}

	}
}