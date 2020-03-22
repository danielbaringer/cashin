using ClassLibrary.Gaas.Shared;
using MongoDB.Bson;
using System;
using System.Collections.Generic;

namespace ClassLibrary.Gaas.Models
{
    public class ObjetivoModel : BaseModel
    {

        public string objetivo { get; set; }
        public DateTime dataIniciar { get; set; }
        public DateTime dataExpiracao { get; set; }
        public List<MiniGameModel> listaMiniGames { get; set; }

        private void startParametros() {

            this.objetivo = String.Empty;
            this.dataIniciar = new Utils().convertDatePtBr(DateTime.Now.ToLocalTime());
            this.dataExpiracao = new Utils().convertDatePtBr(DateTime.Now.ToLocalTime());

            if (this.listaMiniGames == null || this.listaMiniGames.Count <= 0)
            {
                this.listaMiniGames = new List<MiniGameModel>();
            }

        }

        public ObjetivoModel() { startParametros(); }

        public ObjetivoModel(string nome, string objetivo, DateTime dtIniciar, DateTime dtExpiracao, bool ativo, MiniGameModel dadosMiniGame)
        {

            startParametros();

            this.nome = nome;
            this.objetivo = objetivo;
            this.dataIniciar = dtIniciar;
            this.dataExpiracao = dtExpiracao;
            this.ativo = ativo;
            this.listaMiniGames.Add(dadosMiniGame);
        }

        public void adicionaNaListaMiniGame(MiniGameModel mg) {

            this.listaMiniGames.Add(mg);

        }

        public List<MiniGameModel> retornaListaMiniGame()
        {

            //return new List<MiniGameModel>();
            return this.listaMiniGames;

        }

    }
}
