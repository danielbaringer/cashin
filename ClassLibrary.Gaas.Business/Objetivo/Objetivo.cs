using ClassLibrary.Gaas.Models;
using ClassLibrary.Gaas.MongoDb;
using ClassLibrary.Gaas.Shared;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ClassLibrary.Gaas.Business.Objetivo
{
    public class Objetivo
    {

        private MongoDbContext _connMongo;
        private IMongoDatabase _db;

        public Objetivo()
        {

            _connMongo = new MongoDbContext();
            _db = _connMongo.GetMongoDbConn();

        }

        public bool salvarDadosObj(string _id, string  _nome, DateTime _dataIniciar, DateTime _dataExpiracao, bool ativo, Dictionary<string, object> _listaMiniGames) {

            ObjectId objToFind = new ObjectId();
            ObjetivoModel objModel = new ObjetivoModel();

            try
            {

                var _obj = _db.GetCollection<ObjetivoModel>("Objetivo");

                if (_id != null && _id != "") {
                    objToFind = new ObjectId(_id);

                    objModel = _obj.AsQueryable().Where(c => c._id.Equals(objToFind)).FirstOrDefault<ObjetivoModel>();
                }


                if (objModel != null && objModel._id.Contains(_id))
                {

                    var update = Builders<ObjetivoModel>
                                    .Update
                                        .Set(u => u.nome, _nome)
                                        .Set(u => u.dataCadastro, objModel.dataCadastro)
                                        .Set(u => u.dataAlteracao, new Utils().convertDatePtBr(DateTime.Now.ToLocalTime()))
                                        .Set(u => u.dataIniciar, new Utils().convertDatePtBr(_dataIniciar))
                                        .Set(u => u.dataExpiracao, new Utils().convertDatePtBr(_dataExpiracao))
                                        .Set(u => u.ativo, ativo);

                    var result = _obj.UpdateOne<ObjetivoModel>(u => u._id.Equals(objToFind), update);


                    return true;
                }
                else {


                    objModel.nome = _nome;
                    objModel.dataIniciar = _dataIniciar;
                    objModel.dataExpiracao = _dataExpiracao;
                    objModel.ativo = ativo;

                    foreach (dynamic dmg in _listaMiniGames) {

                        dynamic mg = dmg.Value;
                        MiniGameModel dadosMg = new MiniGameModel(mg.id + " - " + mg.nome, true, mg.parametroObjCompletado,  mg.vlrCashIn);

                        objModel.adicionaNaListaMiniGame(dadosMg);
                    }

                    objModel.listaMiniGames = objModel.retornaListaMiniGame();

                    _obj.InsertOne(objModel);

                    return true;

                }



            }
            catch (Exception xp)
            {
                return false;
            }

        }

        public dynamic localizarObjetivoId(string strBusca)
        {

            dynamic dadosObjetivo = new object();

            try
            {

                var _obj = _db.GetCollection<ObjetivoMiniGameModel>("ObjetivoIniciado");

                ObjectId dadosObjId = new ObjectId(strBusca);

                ObjetivoMiniGameModel dadosObj = (from obj in _obj.AsQueryable<ObjetivoMiniGameModel>()
                                                  where obj.ativo.Equals(true)
                                                  && obj._id.Equals(dadosObjId)
                                                  select obj).FirstOrDefault();

                if (dadosObj != null)                
                    dadosObjetivo = dadosObj;

            }
            catch (Exception xp)
            {
            }


            return dadosObjetivo;

        }

        public Dictionary<string, object> localizarObjetivo(string strBusca) {

            Dictionary<string, object> dadosObjetivo = new Dictionary<string, object>();

            try {

                var _obj = _db.GetCollection<ObjetivoModel>("Objetivo");

                List<ObjetivoModel> dadosObj = (from obj in _obj.AsQueryable<ObjetivoModel>()
                                          where obj.ativo.Equals(true)
                                          && DateTime.Now.ToLocalTime() >= obj.dataIniciar
                                          && DateTime.Now.ToLocalTime() <= obj.dataExpiracao
                                                orderby obj.dataExpiracao ascending
                                          select obj).ToList();

                if (dadosObj != null && dadosObj.Count > 0) {


                    foreach (ObjetivoModel obj in dadosObj) {


                        //if(DateTime.Now >= obj.dataIniciar && DateTime.Now <= obj.dataExpiracao) {


                            List<MiniGameModel> listageGames = obj.listaMiniGames;

                            foreach (MiniGameModel mg in listageGames)
                            {

                                if (mg.nome.Contains(strBusca)) {

                                    dadosObjetivo.Add(obj._id.ToString(), obj);

                                }

                            }



                        //}


                    }


                }

            }
            catch (Exception xp)
            {
                return new Dictionary<string, object>();
            }


            return dadosObjetivo;

        }


        public Dictionary<string, object> listarObjs(string id="") {

            Dictionary<string, object> dadosObjetivo = new Dictionary<string, object>();

            try
            {

                var _obj = _db.GetCollection<ObjetivoModel>("Objetivo");
                List<ObjetivoModel> listaDeObjs;

                if (string.IsNullOrEmpty(id))
                {
                    listaDeObjs = _obj.AsQueryable().ToList<ObjetivoModel>();
                }
                else {

                    ObjectId objToFind = new ObjectId(id);

                    listaDeObjs = _obj.AsQueryable().Where(c => c._id.Equals(objToFind)).ToList<ObjetivoModel>();
                }

                    

                foreach (ObjetivoModel obj in listaDeObjs) {

                    dadosObjetivo.Add(obj._id.ToString(), obj);

                }


                return dadosObjetivo;


            }
            catch (Exception xp)
            {
                return new Dictionary<string, object>();
            }
        }

        

    }
}
