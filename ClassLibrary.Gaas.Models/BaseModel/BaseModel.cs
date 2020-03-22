using ClassLibrary.Gaas.MongoDb;
using MongoDB.Bson;
using System;
using ClassLibrary.Gaas.Shared;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;

namespace ClassLibrary.Gaas.Models
{
    public abstract class BaseModel
    {
        [BsonId(IdGenerator = typeof(StringObjectIdGenerator))]
        [BsonRepresentation(BsonType.ObjectId)]
        public string _id;
        public string nome;
        public DateTime dataCadastro;
        public DateTime dataAlteracao;
        public bool ativo;

        public BaseModel() {


            this._id = new MongoDbContext().novoObjectIdGuiid().ToString();
            this.dataCadastro = new Utils().convertDatePtBr(DateTime.Now.ToLocalTime());
            this.dataAlteracao = new Utils().convertDatePtBr(DateTime.Now.ToLocalTime());
            this.ativo = true;
        }

        public string getObjectId() { return this._id;  }

    }
}
