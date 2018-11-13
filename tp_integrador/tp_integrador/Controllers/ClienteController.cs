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
            
            return View("CargarDispositivo",model: false);
        }

        [HttpPost]
        public ActionResult LoadDispositivoJson(HttpPostedFileBase user_file)
        {
            if ((bool)Session["Admin"]) return PermisoDenegado();

            if (user_file == null) return CargarDispositivo();

            Cliente unclietne = (Cliente)Session["Usuario"];
            // INTELIGENTE = 1
            unclietne.CargarDispositivos(user_file,1);
			
			TempData["Message"] = "Dispositivos Inteligentes Cargados :D";

			return View("CargarDispositivo", model: true);
        }
        [HttpPost]
        public ActionResult LoadDispositivoJsone(HttpPostedFileBase user_file)
        {
            if ((bool)Session["Admin"]) return PermisoDenegado();

            if (user_file == null) return CargarDispositivo();

            Cliente unclietne = (Cliente)Session["Usuario"];
            // ESTANDAR = 0
            unclietne.CargarDispositivos(user_file,0);
			
			TempData["Message"] = "Dispositivos Estandar Cargados :D";

			return View("CargarDispositivo", model: true);
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult SelecTemplate_dis(int disp)
        {
            if ((bool)Session["Admin"]) return PermisoDenegado();
			Cliente unclietne = (Cliente)Session["Usuario"];

			if (disp == 0) return View("Dashboard", model: unclietne);
			else
            {                
                unclietne.AgregarDispositivoDeTemplate(disp);
                                
                return View("Dashboard", model: unclietne);
            }
        }
		       
		public ActionResult EstadoDispositivo(int idD, int idC, int numero)
		{
			Cliente uncliente = (Cliente)Session["Usuario"];
			uncliente.CambiarEstado(uncliente.BuscarDispositivo(idD, idC, numero));
			return View("Dashboard", model: uncliente);
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