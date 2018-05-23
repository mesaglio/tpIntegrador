using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace tp_integrador.Controllers
{
    public class ClienteController : Controller
    {
        // GET: Cliente
        public ActionResult Dashboard()
        {
            return View();
        }

        public ActionResult GestionDeDispositivos()
        {
            return View();
        }
    }
}