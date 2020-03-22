using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ame.Gaas.Admin.Models
{
    public class ModeloClickJogosGames
    {

        public string previous;
        public string next;
        public string total;
        public List<MiniGameClickJogos> dados = new List<MiniGameClickJogos>();
    }
}