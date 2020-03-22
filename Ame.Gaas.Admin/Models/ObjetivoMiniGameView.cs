using ClassLibrary.Gaas.Shared;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;


namespace Ame.Gaas.Admin.Models
{
    public class ObjetivoMiniGameView
    {

        public string id { get; set; }
        public string nome { get; set; }
        public bool ativo { get; set; }
        public int parametroObjCompletado { get; set; }
        public decimal vlrCashIn { get; set; }
        
        private void startThis()
        {

            this.id = new Utils().geraNewGuiid().ToString();
            this.nome = string.Empty;
            this.parametroObjCompletado = 60;
            this.vlrCashIn = 0.01m;
            this.ativo = true;

        }

        public ObjetivoMiniGameView() {

            startThis();

        }

        public ObjetivoMiniGameView(string _nome, int _vlrObjCompletado, decimal _vlrCashIn, bool _ativo)
        {

            startThis();
            this.nome = _nome;
            this.parametroObjCompletado = _vlrObjCompletado;
            this.vlrCashIn = _vlrCashIn;
            this.ativo = _ativo;

        }

    }
}