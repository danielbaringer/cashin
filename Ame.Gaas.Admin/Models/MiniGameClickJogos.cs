using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ame.Gaas.Admin.Models
{
    public class MiniGameClickJogos
    {

        public string id;
        public string nome;


        public MiniGameClickJogos() { }

        public MiniGameClickJogos(string _id, string _nome)
        {

            this.id = _id;
            this.nome = _nome;

        }

    }

}