using Ame.Gaas.Admin.Models;
using ClassLibrary.Gaas.Shared;
using System.Web.Mvc;

namespace Ame.Gaas.Admin.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            Session.Clear();
            return View();
        }

        [HttpPost]
        public ActionResult Index(UsuarioView usr)
        {
            Session.Clear();
            bool resp = new UsuarioView().autenticaUsr(usr.email, usr.senha);

            if (resp)
            {

                Session["GuidSessao"] = new Utils().geraNewGuiid().ToString();
                return RedirectToAction("Dashboard");
            }

            return View(usr);
        }

        [AuthSession]
        public ActionResult Dashboard()
        {

            return View();
        }

    }
}