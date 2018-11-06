using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using tp_integrador.Models;

namespace tp_integrador
{
    public class MvcApplication : System.Web.HttpApplication
    {
        public static DAOUsuario Daobjeto;
        public static DAOzona GlobalZonas;

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            Daobjeto = new DAOUsuario();
			DAOzona.Instancia.InitialLoad();
			DAODispositivo.Instancia.InitialLoad();
			DAOSensores.Instancia.InitialLoad();
		}
    }

    public class Global : System.Web.HttpApplication
    {
            Usuarios user = new Usuarios();
            protected void Application_Start(object sender, EventArgs e) { }
            protected void Session_Start(object sender, EventArgs e)
            {
                Session["User"] = user;
            }
        }
}