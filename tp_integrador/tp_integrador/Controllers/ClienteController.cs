﻿using System.Web;
using System.Web.Mvc;
using tp_integrador.Models;
using tp_integrador.Controllers;
using System;

namespace tp_integrador.Controllers
{
    public class ClienteController : Controller
    {
        private Cliente Tocliente(Usuarios user) => (Cliente)user;
        private ActionResult PermisoDenegado()
        {
            return new HttpNotFoundResult();
        }




        // GET: Cliente
        public ActionResult Dashboard()
        {
            if ((bool)Session["Admin"]) return PermisoDenegado();

            // Tocliente(user);
            Cliente user = (Cliente)Session["Usuario"];


            return View(user);
        }



        [HttpPost]
        [AllowAnonymous]
        public ActionResult GestionDeDispositivos(Usuarios user)
        {
            if ((bool)Session["Admin"]) return PermisoDenegado();

            Tocliente(user);

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
                return View("LoadDispositivoJson");
            }
        }

    }
}