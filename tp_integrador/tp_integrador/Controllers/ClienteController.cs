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

            return View(model: a);
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
        public ActionResult SelecTemplate_dis(templateDisp disp)
        {
            if (disp is null)
            {
                return CargarDispositivo();
            }
            else
            {
                disp.consumo = disp.consumo + 1;
                return View("LoadDispositivoJson");
            }
        }

    }
}