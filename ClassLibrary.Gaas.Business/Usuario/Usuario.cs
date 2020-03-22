using ClassLibrary.Gaas.Models;
using ClassLibrary.Gaas.MongoDb;
using ClassLibrary.Gaas.Shared;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using System;
using System.Linq;


namespace ClassLibrary.Gaas.Business.Usuario
{
    public class Usuario
    {
        private MongoDbContext _connMongo;
        private IMongoDatabase _db;

        public Usuario() {

            _connMongo = new MongoDbContext();
            _db = _connMongo.GetMongoDbConn();

        }


        public bool performaLogin(string usr, string senha) {


            try {

                var _usr = _db.GetCollection<UsuarioModel>("Usuario");

                UsuarioModel dadosLogado = (from u in _usr.AsQueryable<UsuarioModel>()
                                            where u.senha.Equals(senha) && u.email.Equals(usr)
                                            select u).FirstOrDefault();

          

                if (dadosLogado != null && dadosLogado.ativo == true)
                {
                    dadosLogado.senha = null;

                    var update = Builders<UsuarioModel>
                                .Update
                                    .Set(u => u.dataCadastro, new Utils().convertDatePtBr(dadosLogado.dataCadastro))
                                    .Set(u => u.dataAlteracao, new Utils().convertDatePtBr(DateTime.Now.ToLocalTime()));


                    var result = _usr.UpdateOne<UsuarioModel>(u => u._id == dadosLogado._id, update);

                    return true;
                }
                else
                {
                    return false;
                }


            }
            catch (Exception xp)
            {
                return false;
            }

            

        }

        public UsuarioModel localizaUsuario(string vlrToFind) {

            UsuarioModel dadosLogado = new UsuarioModel();

            try
            {
                var _usr = _db.GetCollection<UsuarioModel>("Usuario");

                dadosLogado = (from u in _usr.AsQueryable<UsuarioModel>()
                                            where u.documento.Contains(vlrToFind)
                                            select u).FirstOrDefault();

            }
            catch (Exception xp) {
                dadosLogado = new UsuarioModel();
            }

            return dadosLogado;
        }

    }
}
