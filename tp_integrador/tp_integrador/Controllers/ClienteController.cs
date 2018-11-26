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

        public ActionResult GestionarDispositivos()
        {
			if (!SessionStateOK()) return View("Index");
			if ((bool)Session["Admin"]) return PermisoDenegado();

            Cliente user = (Cliente)Session["Usuario"];


            return View("GestionarDispositivos",user);
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

            return View("Simplex",user);
        }       
		  
        public ActionResult CargarArchivoDispositivos()
        {
			if (!SessionStateOK()) return View("Index");			
            if ((bool)Session["Admin"]) return PermisoDenegado();
            // trae los templates para que el cliente sume un dispositivo
            
            return View("CargarArchivoDispositivos",model: false);
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
            // INTELIGENTE = 1
            unclietne.CargarDispositivos(user_file,1);
			
			TempData["Message"] = "Dispositivos Inteligentes Cargados :D";

			return View("CargarArchivoDispositivos", model: true);
        }
        [HttpPost]
        public ActionResult LoadDispositivoJsone(HttpPostedFileBase user_file)
        {
			if (!SessionStateOK()) return View("Index");
			if ((bool)Session["Admin"]) return PermisoDenegado();

            if (user_file == null) return CargarArchivoDispositivos();

            Cliente unclietne = (Cliente)Session["Usuario"];
            // ESTANDAR = 0
            unclietne.CargarDispositivos(user_file,0);
			
			TempData["Message"] = "Dispositivos Estandar Cargados :D";

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

			Cliente uncliente = (Cliente)Session["Usuario"];
			uncliente.CambiarEstado(uncliente.BuscarDispositivo(idD, idC, numero));
			return View("GestionarDispositivos", model: uncliente);
		}

		public ActionResult EditarEstandar(int idD, int idC, int numero)
		{
			if (!SessionStateOK()) return View("Index");

			Cliente uncliente = (Cliente)Session["Usuario"];
			var dispositivo = uncliente.BuscarDispositivo(idD, idC, numero);
			return View(dispositivo);
		}

		[HttpPost]
		public ActionResult EditarEstandar(Estandar disp)
		{
			if (!SessionStateOK()) return View("Index");

			Cliente uncliente = (Cliente)Session["Usuario"];
			
			uncliente.UsoDiario(uncliente.BuscarDispositivo(disp.IdDispositivo, disp.IdCliente, disp.Numero), disp.usoDiario);

			return View("GestionarDispositivos", model: uncliente);
		}

		public ActionResult ConvertirEstandar(int idD, int idC, int numero)
		{
			if (!SessionStateOK()) return View("Index");

			Cliente uncliente = (Cliente)Session["Usuario"];
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

			Cliente uncliente = (Cliente)Session["Usuario"];
			uncliente.CambiarAutoSimplex();			
			
			return View("Simplex", model: uncliente);
		}

		public bool SessionStateOK()
		{
			if (Session["Usuario"] == null) return false;
			if (Session["Admin"] == null) Session["Admin"] = (Session["Usuario"].GetType() == typeof(Administrador));
			return true;			
		}
	}
}