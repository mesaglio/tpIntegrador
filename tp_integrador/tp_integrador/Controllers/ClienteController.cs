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

        public ActionResult CargarDispositivo()
        {
            if ((bool)Session["Admin"]) return PermisoDenegado();

            DAO_t_dispositivostemplate a = new DAO_t_dispositivostemplate();

            return View("CargarDispositivo",model: a);
        }

        [HttpPost]
        public ActionResult LoadDispositivoJson(HttpPostedFileBase file)
        {
            if ((bool)Session["Admin"]) return PermisoDenegado();

            if (file == null) return CargarDispositivo();

            CargarJson cargar = new CargarJson();
            cargar.LoadJson<Inteligente>(file.InputStream);

            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult SelecTemplate_dis(int disp)
        {
            if ((bool)Session["Admin"]) return PermisoDenegado();

            if (disp == 0)
            {
                return CargarDispositivo();
            }
            else
            {
                DAO_t_dispositivostemplate a = new DAO_t_dispositivostemplate();
                templateDisp dispositivo = a.Searchtemplatebyid(disp);
                Cliente unclietne = (Cliente)Session["Usuario"];
                if (dispositivo.inteligente == "si") { unclietne.NuevoDispositivoInteligente(dispositivo.getNombreEntero(), dispositivo.consumo); }
                else
                    { unclietne.NuevoDispositivoEstandar(dispositivo.getNombreEntero(), dispositivo.consumo,0); }
                return View("LoadDispositivoJson");
            }
        }


        [HttpPost]
        [AllowAnonymous]
        public ActionResult EncenderDispositivo(string postdispositivo)
        {
            Cliente unclietne = (Cliente)Session["Usuario"];
            foreach (Inteligente undispo in unclietne.DispositivosInteligentes)
            { if (undispo.Nombre == postdispositivo) undispo.Encender(); }

            return View("Dashboard", model: unclietne);
        }
        [HttpPost]
        [AllowAnonymous]
        public ActionResult ApagarDispositivo(string postdispositivo)
        {
            Cliente unclietne = (Cliente)Session["Usuario"];
            foreach (Inteligente undispo in unclietne.DispositivosInteligentes)
            { if (undispo.Nombre == postdispositivo) undispo.Apagar(); }

            return View("Dashboard", model: unclietne);
        }

		[HttpPost]		
		public ActionResult CalculoSimplex()
		{
			if ((bool)Session["Admin"]) return PermisoDenegado();

			Cliente uncliente = (Cliente)Session["Usuario"];

			SIMPLEX sim = new SIMPLEX();
			var respuesta = sim.GetSimplexData(sim.CrearConsulta(uncliente.dispositivos));

			var sb = new StringBuilder();
			sb.AppendLine("<b>Consumo Optimo Para Sus Dispositivos: " + "</b><br/>");
			sb.AppendLine(""+"<br/>");
			sb.AppendLine("<b>Maximo: </b>" + respuesta[0] + "<br/>");
			var cantDisp = uncliente.dispositivos.Count;

			for (int i = 1; i < respuesta.Length; i++)
			{
				sb.AppendLine("<b>" + uncliente.dispositivos[cantDisp-i].Nombre + ": </b>" + respuesta[i] + "<br/>");
			}
			
			return Content(sb.ToString());
		}



	}
}