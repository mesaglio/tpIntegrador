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
            DAOUsuario data = new DAOUsuario();
            Usuarios u =  data.InicioSecion(model);
            if (u.usuario == null)
            {
                Session["IDUsuario"] = -1;
                return View(u);
            }
            else
            {
                Session["Usuario"] = u;
                Session["IDUsuario"] = u.idUsuario;
                Session["Admin"] = u.Esadmin();
                u.SetLoginOn();

                return View(u);
            }
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

            CargarJson cargar = new CargarJson();
            cargar.LoadJson<Cliente>(file.InputStream);
            return View();
        }

        public ActionResult Maps() {

            Zona z = new Zona("test",40, 36.81881, 10.16596);
             return View( z.transformadores);
        }
    }
}
