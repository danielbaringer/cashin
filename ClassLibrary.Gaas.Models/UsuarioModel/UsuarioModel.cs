using MongoDB.Bson;
using System;

namespace ClassLibrary.Gaas.Models
{
    public class UsuarioModel : BaseModel
    {

        public string documento { get; set; }
        public string email { get; set; }
        public string senha { get; set; }

        public UsuarioModel() {  }

        public UsuarioModel(string _id, string _nome, string _documento, string _email, string _senha, bool _ativo)
        {
            this._id = _id;
            this.nome = _nome;
            this.documento = _documento;
            this.email = _email;
            this.senha = _senha;
            this.ativo = _ativo;
        }




    }
}
