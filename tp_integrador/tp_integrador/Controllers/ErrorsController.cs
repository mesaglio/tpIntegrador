using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace tp_integrador.Controllers
{
    public class ErrorsController : Controller
    {
        // GET: Errors
        public ActionResult Error404()
        {
            ActionResult result;

        object model = Request.Url.PathAndQuery;

        if (!Request.IsAjaxRequest())
            result = View(model);
        else
            result = PartialView("_NotFound", model);

        return result;
    }
}
}