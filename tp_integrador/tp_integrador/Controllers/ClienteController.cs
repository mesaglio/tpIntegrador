using System.Web;
using System.Web.Mvc;
using tp_integrador.Models;
using tp_integrador.Controllers;
using System;
using System.Collections.Generic;
using System.Text;

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

        public ActionResult Dashboard()
        {
            if ((bool)Session["Admin"]) return PermisoDenegado();

            Cliente user = (Cliente)Session["Usuario"];


            return View(user);
        }
		        
        public ActionResult GestionDeDispositivos()
        {
            if ((bool)Session["Admin"]) return PermisoDenegado();

            Cliente user = (Cliente)Session["Usuario"];

            return View(user);
        }

        /*  public ActionResult CargarTransformadoresPorDefecto()
          {
              if ((bool)Session["Admin"]) return PermisoDenegado();
              //LA zona tiene que venir junto con un json, esta zona la pongo por defecto
              Zona model = new Zona("La Matanza", 100, 5478, 7896);
              model.RellenarTransformadores();
              return View(model);

          }*/





        public ActionResult CargarDispositivo()
        {
            if ((bool)Session["Admin"]) return PermisoDenegado();
            // trae los templates para que el cliente sume un dispositivo
            DAOTemplates a = new DAOTemplates();

            return View("CargarDispositivo",model: a);
        }

        [HttpPost]
        public ActionResult LoadDispositivoJson(HttpPostedFileBase file)
        {
            if ((bool)Session["Admin"]) return PermisoDenegado();

            if (file == null) return CargarDispositivo();

            Cliente unclietne = (Cliente)Session["Usuario"];
            unclietne.CargarDispositivosInteligenes(file);


            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult SelecTemplate_dis(int disp)
        {
            if ((bool)Session["Admin"]) return PermisoDenegado();

            if (disp == 0) return CargarDispositivo();			
            else
            {
                Cliente unclietne = (Cliente)Session["Usuario"];
                unclietne.AgregarDispositivoDeTemplate(disp);
                                
                return View("LoadDispositivoJson");
            }
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult EncenderDispositivo(string postdispositivo)
        {
            Cliente unclietne = (Cliente)Session["Usuario"];
            unclietne.EncenderDispositivo(postdispositivo);
            
            return View("Dashboard", model: unclietne);
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult ApagarDispositivo(string postdispositivo)
        {
            Cliente unclietne = (Cliente)Session["Usuario"];
            unclietne.ApagarDispositivo(postdispositivo);
            
            return View("Dashboard", model: unclietne);
        }

		[HttpPost]		
		public ActionResult CalculoSimplex()
		{
			if ((bool)Session["Admin"]) return PermisoDenegado();

		    Cliente uncliente = (Cliente)Session["Usuario"];
            var sb = uncliente.RunSimplex();

           return Content(sb.ToString());
		}

		[HttpPost]
		[AllowAnonymous]
		public ActionResult AhorroAutomatico()
		{
			Cliente uncliente = (Cliente)Session["Usuario"];
			uncliente.AutoSimplex = !uncliente.AutoSimplex;

			ORM.Instancia.Update(uncliente);
			
			return View("GestionDeDispositivos", model: uncliente);
		}
	}
}