using ClassLibrary.Gaas.Models;
using ClassLibrary.Gaas.MongoDb;
using ClassLibrary.Gaas.Shared;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary.Gaas.Business.ObjetivoMiniGame
{
    public class ObjetivoMiniGame
    {


        private ObjetivoMiniGameModel objModel;
        private MongoDbContext _connMongo;
        private IMongoDatabase _db;

        private void startModelo()
        {

            _connMongo = new MongoDbContext();
            _db = _connMongo.GetMongoDbConn();
            objModel = new ObjetivoMiniGameModel();
        }

        public ObjetivoMiniGame() {

            startModelo();


        }

        public bool informarInicioObj(string infoDoc, string infoObj)
        {


            try
            {
                return this.persistirBancoDeDados(infoDoc, infoObj);

            }
            catch (Exception xp)
            {
                return false;
            }



        }

        private  bool persistirBancoDeDados(string infoDoc, string infoObj)
        {

            string codMiniGame = string.Empty;
            bool resp = false;

            try
            {

                startModelo();
                var _obj = _db.GetCollection<ObjetivoMiniGameModel>("ObjetivoIniciado");

                ObjetivoModel dadosObjModel = this.findObjetivoPrincipal(infoObj);
                
                if (dadosObjModel != null) {

                    string nomeObj = infoDoc + "-" + infoObj;
                    ObjetivoMiniGameModel objFound = this.findObjMiniGameInUser(infoDoc, infoObj);

                    if (objFound == null)
                    {
                        objModel.nome = nomeObj;
                        objModel.setUsuarioGame(new UsuarioModel { nome = infoDoc , documento = infoDoc});
                        objModel.setObjetivoGame(dadosObjModel);

                        dynamic dynList = dadosObjModel.retornaListaMiniGame();

                        foreach (dynamic objList in dynList) {


                            dynamic obj = objList;
                            objModel.addMiniGames(objList);

                        }

                        codMiniGame = objModel._id.ToString();
                        _obj.InsertOne(objModel);

                        resp = true;
                    }


                    var posicaoLista = (objFound != null ? objFound.ListaMiniGames.FindIndex(f => f.nome.Contains(infoObj)) : 0);
                    if (objFound != null && objFound.ativo && resp == false && posicaoLista >= 0)
                    {

                        int posicao = posicaoLista; // objFound.ListaMiniGames.FindIndex(p => p.nome.Contains(infoObj));

                        codMiniGame = objFound._id.ToString();

                        var listaAtualMiniGames = objFound.listarMiniGames();


                        MiniGameModel objTrataValores = listaAtualMiniGames.ElementAt(posicao);

                        if (objTrataValores.ativo && objTrataValores.CashInSent.Equals(false))
                        {


                            string objId = (string)objTrataValores._id;
                            double secondsTarget = objTrataValores.targetForPayment;
                            double diffInSeconds = this.validaTempoJogando(objTrataValores);


                            var update = Builders<ObjetivoMiniGameModel>
                                    .Update
                                        .Set(u => u.dataCadastro, objFound.dataCadastro)
                                        .Set(u => u.dataAlteracao, new Utils().convertDatePtBr(DateTime.Now.ToLocalTime()))
                                        .Set(u => u.ObjUsr.dataCadastro, objFound.ObjUsr.dataCadastro)
                                        .Set(u => u.ObjUsr.dataAlteracao, new Utils().convertDatePtBr(DateTime.Now.ToLocalTime()))
                                        .Set(u => u.ObjParametros.dataCadastro, objFound.ObjParametros.dataCadastro)
                                        .Set(u => u.ObjParametros.dataAlteracao, new Utils().convertDatePtBr(DateTime.Now.ToLocalTime()))
                                        .Set(u => u.ListaMiniGames.ElementAt(posicao).dataCadastro, objFound.ListaMiniGames.ElementAt(posicao).dataCadastro)
                                        .Set(u => u.ListaMiniGames.ElementAt(posicao).dataAlteracao, new Utils().convertDatePtBr(DateTime.Now.ToLocalTime()))
                                        .Set(u => u.ListaMiniGames.ElementAt(posicao).objDataIniciado, objFound.ListaMiniGames.ElementAt(posicao).objDataIniciado)
                                        .Set(u => u.ListaMiniGames.ElementAt(posicao).objDataFinalizado, new Utils().convertDatePtBr(DateTime.Now.ToLocalTime()))
                                        .Set(u => u.ListaMiniGames.ElementAt(posicao).objTempoJogando, diffInSeconds);

                            var result = _obj.UpdateOne<ObjetivoMiniGameModel>(u => u._id == objFound._id, update);



                            resp = true;


                        }


                    }
                    else if (resp == false && objFound != null && objFound.ativo)
                    {


                        int posicao = posicaoLista; // objFound.ListaMiniGames.FindIndex(p => p.nome.Contains(infoObj));

                        if (posicao >= 0)
                        {


                            codMiniGame = objFound._id.ToString();

                            MiniGameModel objTrataValores = objFound.listarMiniGames().ElementAt(posicao);

                            if (objTrataValores != null && objTrataValores.ativo && objTrataValores.CashInSent.Equals(false))
                            {

                                string objId = (string)objTrataValores._id;
                                double secondsTarget = objTrataValores.targetForPayment;
                                double diffInSeconds = this.validaTempoJogando(objTrataValores);


                                var update = Builders<ObjetivoMiniGameModel>
                                        .Update
                                            .Set(u => u.dataCadastro, objFound.dataCadastro)
                                            .Set(u => u.dataAlteracao, new Utils().convertDatePtBr(DateTime.Now.ToLocalTime()))
                                            .Set(u => u.ObjUsr.dataCadastro, objFound.ObjUsr.dataCadastro)
                                            .Set(u => u.ObjUsr.dataAlteracao, new Utils().convertDatePtBr(DateTime.Now.ToLocalTime()))
                                            .Set(u => u.ObjParametros.dataCadastro, objFound.ObjParametros.dataCadastro)
                                            .Set(u => u.ObjParametros.dataAlteracao, new Utils().convertDatePtBr(DateTime.Now.ToLocalTime()))
                                            .Set(u => u.ListaMiniGames.ElementAt(posicao).dataCadastro, objFound.ListaMiniGames.ElementAt(posicao).dataCadastro)
                                            .Set(u => u.ListaMiniGames.ElementAt(posicao).dataAlteracao, new Utils().convertDatePtBr(DateTime.Now.ToLocalTime()))
                                            .Set(u => u.ListaMiniGames.ElementAt(posicao).objDataIniciado, objFound.ListaMiniGames.ElementAt(posicao).objDataIniciado)
                                            .Set(u => u.ListaMiniGames.ElementAt(posicao).objDataFinalizado, new Utils().convertDatePtBr(DateTime.Now.ToLocalTime()))
                                            .Set(u => u.ListaMiniGames.ElementAt(posicao).objTempoJogando, diffInSeconds);

                                var result = _obj.UpdateOne<ObjetivoMiniGameModel>(u => u._id == objFound._id, update);

                                resp = true;

                            }

                            
                        }
                        else
                        {

                            var listaAtualMiniGames = dadosObjModel.retornaListaMiniGame(); // objFound.listarMiniGames();

                            MiniGameModel novoMiniGame = new MiniGameModel();

                            foreach (dynamic objList in listaAtualMiniGames)
                            {

                                novoMiniGame = new MiniGameModel { nome = objList.nome, ativo = true, targetForPayment = objList.targetForPayment, CashPayment = objList.CashPayment };
                                objModel.addMiniGames(novoMiniGame);
                            }

                            List<MiniGameModel> novaListaMiniGames = objModel.listarMiniGames();

                            var update = Builders<ObjetivoMiniGameModel>
                                    .Update
                                        .Set(u => u.dataCadastro, objFound.dataCadastro)
                                        .Set(u => u.dataAlteracao, new Utils().convertDatePtBr(DateTime.Now.ToLocalTime()))
                                        .Set(u => u.ObjUsr.dataCadastro, objFound.ObjUsr.dataCadastro)
                                        .Set(u => u.ObjUsr.dataAlteracao, objFound.ObjUsr.dataCadastro)
                                        .Set(u => u.ObjParametros.dataCadastro, objFound.ObjParametros.dataCadastro)
                                        .Set(u => u.ObjParametros.dataAlteracao, new Utils().convertDatePtBr(DateTime.Now.ToLocalTime()))
                                        .Push(p => p.ListaMiniGames, novoMiniGame);

                            var result = _obj.UpdateOne<ObjetivoMiniGameModel>(u => u._id == objFound._id, update);
                            
                            resp = true;

                        }


                    }

                    if (resp == true)
                        this.validaDefineObjetivoConcluido(infoDoc, codMiniGame);

                }


                return resp;

            }
            catch (Exception xp)
            {
                return false;
            }

        }

        public ObjetivoMiniGameModel findObjMiniGameInUser(string document, string miniGame)
        {


            startModelo();
            bool resp = false;
            var _obj = _db.GetCollection<ObjetivoMiniGameModel>("ObjetivoIniciado");

            ObjetivoMiniGameModel dadosBusca = (from e in _obj.AsQueryable<ObjetivoMiniGameModel>()
                    where DateTime.Now.ToLocalTime() >= e.ObjParametros.dataIniciar
                    && DateTime.Now.ToLocalTime() <= e.ObjParametros.dataExpiracao
                    && e.ObjUsr.documento.Contains(document)
                    orderby e.ObjParametros.dataExpiracao ascending
                    select e).FirstOrDefault();


            if (dadosBusca != null) {

                List<MiniGameModel> listaMG = dadosBusca.listarMiniGames();


                if (listaMG.Count > 0)
                {
                    resp = true;
                }
                //foreach (MiniGameModel mgm in listaMG) {

                //    if (mgm.nome.Contains(miniGame)) {
                //        resp = true;
                //    }

                //}

            }

            return (resp ? dadosBusca : null);

        }

        private double validaTempoJogando(MiniGameModel objValores)
        {

            double diffInSeconds = 0.0;
            MiniGameModel objTrataValores = objValores;

            string objId = (string)objTrataValores._id;
            double secondsTarget = objTrataValores.targetForPayment;
            diffInSeconds = (new Utils().convertDatePtBr(DateTime.Now.ToLocalTime()) - objTrataValores.objDataFinalizado.ToLocalTime()).TotalSeconds;

            if (diffInSeconds <= 60)
            {
                diffInSeconds = objTrataValores.objTempoJogando + (new Utils().convertDatePtBr(DateTime.Now.ToLocalTime()) - objTrataValores.objDataFinalizado.ToLocalTime()).TotalSeconds;
            }
            else
            {
                diffInSeconds = objTrataValores.objTempoJogando;
            }

            return diffInSeconds;

        }

        private ObjetivoModel findObjetivoPrincipal(string stringFind)
        {

            bool respFound = false;
            ObjetivoModel objetivoDados = new ObjetivoModel();
            Dictionary<string, object> dadosFound = new Objetivo.Objetivo().localizarObjetivo(stringFind);

            foreach (dynamic dados in dadosFound) {

                dynamic objVw = dados.Value;

                objetivoDados._id = objVw._id;
                objetivoDados.nome = objVw.nome;
                objetivoDados.objetivo = objVw.nome; // objVw.objetivo;
                objetivoDados.dataIniciar = objVw.dataIniciar;
                objetivoDados.dataExpiracao = objVw.dataExpiracao;

                foreach (dynamic list in objVw.listaMiniGames) {

                    if (list.nome.Contains(stringFind)) {

                        objetivoDados.adicionaNaListaMiniGame(new MiniGameModel(list.nome, true, list.targetForPayment, list.CashPayment));
                    }

                }
                
                //objetivoDados.listaMiniGames = objVw.listaMiniGames;
                //miniGameDados.setObjetivoGame(new ObjetivoModel { _id = objVw._id ,nome = objVw.nome , objetivo = objVw.objetivo, dataIniciar = objVw.dataIniciar, dataExpiracao = objVw.dataExpiracao, ativo = objVw.ativo });
                objetivoDados.ativo = (bool)objVw.ativo;

                respFound = true;

            }

            return (respFound ? objetivoDados : null);

        }

        private void validaDefineObjetivoConcluido(string infoDoc, string codMiniGame)
        {

            int posicao = 0;
            var _obj = _db.GetCollection<ObjetivoMiniGameModel>("ObjetivoIniciado");

            try
            {

                ObjetivoMiniGameModel objFound = this.findObjMiniGameInUser(infoDoc, codMiniGame);


                dynamic dadosObjFound = new Objetivo.Objetivo().localizarObjetivoId((objFound._id.ToString()));

                if (dadosObjFound != null)
                {

                    foreach (dynamic dadosMG in dadosObjFound.ListaMiniGames) {

                        dynamic obj = dadosMG;

                        string objId = (string) dadosObjFound._id;
                        double secondsTarget = obj.targetForPayment;
                        double tempoJogando = obj.objTempoJogando;

                        if (tempoJogando >= secondsTarget)
                        {

                            //Enviar pagamento


                            var update = Builders<ObjetivoMiniGameModel>
                                        .Update
                                            .Set(u => u.dataAlteracao, new Utils().convertDatePtBr(DateTime.Now.ToLocalTime()))
                                            .Set(u => u.ObjUsr.dataAlteracao, new Utils().convertDatePtBr(DateTime.Now.ToLocalTime()))
                                            .Set(u => u.ObjParametros.dataAlteracao, new Utils().convertDatePtBr(DateTime.Now.ToLocalTime()))
                                            .Set(u => u.ListaMiniGames.ElementAt(posicao).ativo, false)
                                            .Set(u => u.ListaMiniGames.ElementAt(posicao).CashInSent, true)
                                            .Set(u => u.ListaMiniGames.ElementAt(posicao).dataAlteracao, new Utils().convertDatePtBr(DateTime.Now.ToLocalTime()));

                            var result = _obj.UpdateOne<ObjetivoMiniGameModel>(u => u._id.Equals(objFound._id), update);

                            
                        }

                        posicao += 1;

                    }

                }

            }
            catch (Exception xp)
            {


            }


        }


        public Dictionary<string, object> listarMiniGamesObj() {

            Dictionary<string, object> listagem = new Dictionary<string, object>();


            for (int i=0; i<=10;i++) {

                var novoId = new Utils().convertToString(new MongoDbContext().novoObjectIdGuiid()).ToString();
                listagem.Add(novoId,new ObjetivoMiniGameModel());


            }

            return listagem;

        }





    }
}
