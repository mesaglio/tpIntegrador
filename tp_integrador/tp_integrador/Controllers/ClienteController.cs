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
        // GET: Cliente
        public ActionResult Dashboard(Administrador user)
        {
            return View(user);
        }

        public ActionResult GestionDeDispositivos(Administrador user)
        {
            return View(user);
        }
    }
}