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

        public ActionResult CargarDispositivo()
        {
            DAO_t_dispositivostemplate a = new DAO_t_dispositivostemplate();

            return View("CargarDispositivo",model: a);
        }

        [HttpPost]
        public ActionResult LoadDispositivoJson(HttpPostedFileBase file)
        {
            if (file == null) return CargarDispositivo();

            CargarJson cargar = new CargarJson();
            cargar.LoadJson<Inteligente>(file.InputStream);

            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        public ActionResult SelecTemplate_dis(int disp)
        {
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