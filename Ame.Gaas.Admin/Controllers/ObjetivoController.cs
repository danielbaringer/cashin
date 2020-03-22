using Ame.Gaas.Admin.Models;
using System;
using System.Net.Http;
using System.Web.Mvc;

namespace Ame.Gaas.Admin.Controllers
{

    [AuthSession]
    public class ObjetivoController : Controller
    {

        ObjetivoView objVw { get; set; }

        public ObjetivoController()
        {
            objVw = new ObjetivoView();
        }

        // GET: Objetivo
        public ActionResult Index()
        {


            objVw = objVw.listarObjetivos("");

            //this.dadosMiniJogosAsync();

            return View(objVw);
        }

        

        public ActionResult Create()
        {
            return View(objVw);
        }

        [HttpPost]
        public ActionResult Create(ObjetivoView obj)
        {

            if (ModelState.IsValid) {
                objVw = objVw.salvarDadosObjetivo(obj);
                //return View(objVw);
                return Json(new { success = true, responseText = "Registro ok!" }, JsonRequestBehavior.AllowGet);
            }


            return Json(new { success = false, responseText = "Registro ok!" }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Edit(string id)
        {

            objVw = objVw.pegaDadosParaEdicao(id);

            return View(objVw);
        }

        [HttpPost]
        public ActionResult Edit(ObjetivoView obj)
        {

            if (ModelState.IsValid)
            {
                objVw = objVw.salvarDadosObjetivo(obj);
            }

            return RedirectToAction("Index", "Objetivo");
        }

        [HttpPost]
        public JsonResult AdicionarMiniGame(ObjetivoView jsonObj)
        {

            if (ModelState.IsValid)
            {
                //var respCall = objVw.adicionaMiniGameObjetivo(jsonObj);
            }


            return Json(jsonObj);
        }

    }
}
