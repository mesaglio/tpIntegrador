using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using tp_integrador.Models;


namespace tp_integrador.Controllers
{
    public class ClienteController : Controller
    {
        private Cliente Tocliente(Usuarios user) => (Cliente)user;

        // GET: Cliente
        [HttpPost]
        [AllowAnonymous]
        public ActionResult Dashboard(Usuarios user)
        {
            Tocliente(user);
   
            return View(user);
        }
        public ActionResult Dashboard()
        {

            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult GestionDeDispositivos(Usuarios user)
        {
            Tocliente(user);

            return View(user);
        }
    }
}