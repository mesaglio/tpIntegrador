using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using tp_integrador.Models;

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
                return View(u);
            else
            {
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
    }
}