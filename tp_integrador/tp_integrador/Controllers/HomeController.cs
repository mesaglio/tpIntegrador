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
        
        public ActionResult JsonImport()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        [HttpPost]
        public ActionResult CargarArchivo(HttpPostedFileBase file)
        {
            if (file == null) return View("JsonImport");

            CargarJson cargar = new CargarJson();
            cargar.LoadJson<Cliente>(file.InputStream);
            return View();
        }
    }
}
