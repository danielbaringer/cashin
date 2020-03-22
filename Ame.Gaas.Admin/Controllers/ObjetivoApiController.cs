using Ame.Gaas.Admin.Filters;
using Ame.Gaas.Admin.Models;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;

namespace Ame.Gaas.Admin.Controllers
{
    
    [BasicAuthenticationAttribute]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    [RoutePrefix("api/obj")]
    public class ObjetivoApiController : ApiController
    {

        GameObjetivo objGame { get; set; }

        public ObjetivoApiController()
        {
            objGame = new GameObjetivo();
        }


        [HttpGet]
        [Route("informarobjinicio/{cpfGame}/{miniGame}")]
        public HttpResponseMessage informarInicioObjetivo(string cpfGame, string miniGame)
        {

            objGame.usuario = cpfGame;
            objGame.game = miniGame;

            if (objGame.registraInicioObjetivoGame(objGame))
            {
                return Request.CreateResponse(HttpStatusCode.OK, objGame);
            }
            return Request.CreateResponse(HttpStatusCode.BadRequest, objGame);

            //return Request.CreateResponse(HttpStatusCode.OK, objGame);

        }

        [HttpPost]
        [Route("informarObjetivo")]
        public HttpResponseMessage InformarObjetivo([FromBody] GameObjetivo miniGameObj)
        {

            //return Request.CreateResponse(HttpStatusCode.OK, miniGameObj);

            //objGame.usuario = miniGameObj.usuario;
            //objGame.game = miniGameObj.game;

            //if (objGame.registraInicioObjetivoGame(objGame))
            //{
            //    return Request.CreateResponse(HttpStatusCode.OK, objGame);
            //}
            //return Request.CreateResponse(HttpStatusCode.BadRequest, objGame);

        
           return Request.CreateResponse(HttpStatusCode.OK, miniGameObj);

        }


        [HttpGet]
        [Route("informarobjprogresso/{cpfGame}/{miniGame}")]
        public HttpResponseMessage informarProgressoObjetivo(string cpfGame, string miniGame)
        {

            objGame.usuario = cpfGame;
            objGame.game = miniGame;

            if (objGame.registraProgressoObjetivoGame(objGame))
            {
                return Request.CreateResponse(HttpStatusCode.OK, objGame);
            }
            return Request.CreateResponse(HttpStatusCode.BadRequest, objGame);
        }

    }
}
