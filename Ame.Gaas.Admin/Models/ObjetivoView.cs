using ClassLibrary.Gaas.Business.Objetivo;
using ClassLibrary.Gaas.Shared;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Data;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Script.Serialization;
using System.Configuration;

namespace Ame.Gaas.Admin.Models
{
    public class ObjetivoView
    {

        public string id { get; set; }
        public string nome { get; set; }
        public int parametroObjCompletado { get; set; }
        public decimal vlrCashIn { get; set; }
        public DateTime dataIniciar { get; set; }
        public DateTime dataExpiracao { get; set; }
        public bool ativo { get; set; }

        public List<ObjetivoMiniGameView> listaMiniGames { get; set; }

        public IEnumerable<ObjetivoView> LObjetivo { get; private set; }

        private Objetivo objBusiness;

        private void startThis() {

            this.id = string.Empty;
            this.nome = string.Empty;
            this.dataIniciar = new Utils().convertDatePtBr(DateTime.Now.ToLocalTime());
            this.dataExpiracao = new Utils().convertDatePtBr(DateTime.Now.ToLocalTime().AddDays(1));
            this.ativo = true;
            this.objBusiness = new Objetivo();
            this.LObjetivo = new List<ObjetivoView>();

            if (this.listaMiniGames == null || this.listaMiniGames.Count <= 0)
            {
                this.listaMiniGames = new List<ObjetivoMiniGameView>();
            }


        }

        public ObjetivoView() { startThis(); }

        public ObjetivoView(string id, string nome, DateTime dataIniciar, DateTime dataExpiracao, bool ativo)
        {

            startThis();

            this.id = id;
            this.nome = nome;
            this.dataIniciar = new Utils().convertDatePtBr(dataIniciar);
            this.dataExpiracao = new Utils().convertDatePtBr(dataExpiracao); ;
            this.ativo = ativo;
            
        }

        public List<ObjetivoMiniGameView> adicionaMiniGameObjetivo(ObjetivoMiniGameView objMiniGame)
        {
            if (this.listaMiniGameObjetivo().Count > 0)
            {
                var findObj = listaMiniGames.Find(m => m.nome.Contains(objMiniGame.nome));

                listaMiniGames.Add(objMiniGame);
            }
            else {

                this.listaMiniGames = new List<ObjetivoMiniGameView>();
                this.listaMiniGames.Add(new ObjetivoMiniGameView(objMiniGame.nome, objMiniGame.parametroObjCompletado, objMiniGame.vlrCashIn, objMiniGame.ativo));
            }

            return this.listaMiniGameObjetivo();
        }

        public List<ObjetivoMiniGameView> listaMiniGameObjetivo()
        {

            if (this.listaMiniGames == null) {
                this.listaMiniGames = new List<ObjetivoMiniGameView>();
            }

            return this.listaMiniGames;

        }

        private HttpResponseMessage obterConteudoMiniGames() {

            HttpResponseMessage resp = new HttpResponseMessage();

            try {

                string url = ConfigurationManager.AppSettings["ClickJogosMiniGamesAPI"];

                using (var client = new HttpClient())
                {

                    var responseTask = client.GetAsync(url, HttpCompletionOption.ResponseHeadersRead);
                    responseTask.Wait();

                    var result = responseTask.Result;
                    if (result.IsSuccessStatusCode)
                    {

                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                        resp = client.GetAsync(url).Result;

                        resp.EnsureSuccessStatusCode();

                    }

                 }


            }
            catch (Exception xp) {
                resp = new HttpResponseMessage();
            }

            return resp;

        }

        private ModeloClickJogosGames dadosMiniJogosAsync()
        {
            ModeloClickJogosGames listaCJ = new ModeloClickJogosGames();

            HttpResponseMessage resp = this.obterConteudoMiniGames();


            using (HttpContent content = resp.Content)
            {
                string responseBody = resp.Content.ReadAsStringAsync().Result;
                ModeloClickJogosGames articles = JsonConvert.DeserializeObject<ModeloClickJogosGames>(responseBody);
                listaCJ = articles;
            }

            return listaCJ;

        }

        public List<ObjetivoMiniGameView> listaMiniGameDisponiveis()
        {

            List<ObjetivoMiniGameView> listaMiniGames = new List<ObjetivoMiniGameView>();

            ModeloClickJogosGames dadosListagem = dadosMiniJogosAsync();


            foreach (MiniGameClickJogos mg in dadosListagem.dados) {


                listaMiniGames.Add(new ObjetivoMiniGameView(mg.nome.Replace(" ","-"), 0, 0, true));


            }

            //MiniGameClickJogos listaCJ = dadosListagem.Result;

            //return new List<ObjetivoMiniGameView>(){ 

            //    new ObjetivoMiniGameView( "Jogo 1", 0, 0.05m, true),
            //    new ObjetivoMiniGameView( "Jogo 2", 0, 0.07m, true),
            //    new ObjetivoMiniGameView( "Jogo 3", 0, 0.00m, true),
            //    new ObjetivoMiniGameView( "Jogo 4", 0, 0.01m, true)

            //};

            return listaMiniGames;

        }

        public List<ObjetivoMiniGameView> removeMiniGameObjetivo(ObjetivoMiniGameView objMiniGame)
        {
            var findObj = listaMiniGames.Find(m => m.nome.Contains(objMiniGame.nome));

            if (findObj == null)
            {
                listaMiniGames.Remove(findObj);
            }

            return listaMiniGames;
        }


        public ObjetivoView salvarDadosObjetivo(ObjetivoView objVw) {

            Dictionary<string, object> dadosParaMiniGames = new Dictionary<string, object>();

            if (objVw != null) {


                foreach (ObjetivoMiniGameView mg in objVw.listaMiniGames)
                {

                    this.adicionaMiniGameObjetivo(mg);

                }

                List<ObjetivoMiniGameView> dadosListaMG = this.listaMiniGameObjetivo();

                if (dadosListaMG != null && dadosListaMG.Count > 0)
                {
                    foreach (ObjetivoMiniGameView mg in dadosListaMG)
                    {
                        dadosParaMiniGames.Add(mg.id, mg);
                    }
                    

                }

                var resp = objBusiness.salvarDadosObj(objVw.id, objVw.nome, objVw.dataIniciar, objVw.dataExpiracao, objVw.ativo, dadosParaMiniGames);

            }



            return this.listarObjetivos("");

        }

        public ObjetivoView pegaDadosParaEdicao(string id)
        {

            ObjetivoView obj = new ObjetivoView();

            try {

                var resp = objBusiness.listarObjs(id);

                foreach (var item in resp)
                {

                    dynamic objVw = item.Value;

                    obj.id = new Utils().convertToString(objVw._id);
                    obj.nome = objVw.nome;
                    obj.dataIniciar = objVw.dataIniciar;
                    obj.dataExpiracao = objVw.dataExpiracao;
                    obj.ativo = (bool)objVw.ativo;

                    foreach (dynamic vlrItem in objVw.listaMiniGames)
                    {

                        dynamic objValue = vlrItem;
                        ObjetivoMiniGameView objMGV = new ObjetivoMiniGameView();

                        objMGV.id = new Utils().convertToString(objValue._id);
                        objMGV.nome = objValue.nome;
                        objMGV.parametroObjCompletado = objValue.targetForPayment;
                        objMGV.vlrCashIn = objValue.CashPayment;
                        objMGV.ativo = (bool)objValue.ativo;

                        obj.listaMiniGames.Add(objMGV);
                    }

                }

                    
            }
            catch (Exception xp) { 
            }

            

            return obj;

        }

        public ObjetivoView listarObjetivos(string id) {

            ObjetivoView obj = new ObjetivoView();
            List<ObjetivoView> listagemObjs = new List<ObjetivoView>();

            var resp = objBusiness.listarObjs(id);

            foreach (var item in resp)
            {

                dynamic objVw = item.Value;
                obj = new ObjetivoView();

                obj.id = new Utils().convertToString(objVw._id);
                obj.nome = objVw.nome;
                obj.dataIniciar = objVw.dataIniciar;
                obj.dataExpiracao = objVw.dataExpiracao;
                obj.ativo = (bool)objVw.ativo;

                foreach (dynamic vlrItem in objVw.listaMiniGames)
                {

                    dynamic objValue = vlrItem;
                    ObjetivoMiniGameView objMGV = new ObjetivoMiniGameView();

                    objMGV.id = new Utils().convertToString(objValue._id);
                    objMGV.nome = objValue.nome;
                    objMGV.parametroObjCompletado = objValue.targetForPayment;
                    objMGV.vlrCashIn = objValue.CashPayment;
                    objMGV.ativo = (bool)objValue.ativo;

                    this.adicionaMiniGameObjetivo(objMGV);
                }

                listagemObjs.Add(obj);

            }

            //NÃO EDITANDO
            if (string.IsNullOrEmpty(id)) {

                //obj = this.pegaDadosParaEdicao(id);
                obj = new ObjetivoView();
            }

            
            obj.LObjetivo = listagemObjs;

            return obj;

        }

        public int retornaQtdeRegsView() {

            if (this.listaMiniGames != null && this.listaMiniGames.Count > 0) {
                return this.listaMiniGames.Count;
            }

            return 0;

        }

    }
}