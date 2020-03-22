using ClassLibrary.Gaas.Shared;
using MongoDB.Bson;
using System;

namespace ClassLibrary.Gaas.Models
{
    public class MiniGameModel : BaseModel
    {

        public double objTempoJogando { get; set; }
        public DateTime objDataIniciado { get; set; }
        public DateTime objDataFinalizado { get; set; }
        public int targetForPayment { get; set; }
        public decimal CashPayment { get; set; }
        public bool CashInSent { get; private set; }

        public MiniGameModel()
        {
            this.objTempoJogando = 0;
            this.targetForPayment = 0;
            this.CashPayment = 0.01m;
            this.objDataIniciado = new Utils().convertDatePtBr(DateTime.Now.ToLocalTime());
            this.objDataFinalizado = new Utils().convertDatePtBr(DateTime.Now.ToLocalTime());
        }

        public MiniGameModel(string nome, bool ativo, int permanenciaJogo, decimal cashPayment)
        {
            this.nome = nome;
            this.dataCadastro = new Utils().convertDatePtBr(DateTime.Now.ToLocalTime());
            this.dataAlteracao = new Utils().convertDatePtBr(DateTime.Now.ToLocalTime());
            this.ativo = ativo;
            this.CashPayment = cashPayment;
            this.targetForPayment = permanenciaJogo;
            this.objTempoJogando = 0;
            this.objDataIniciado = new Utils().convertDatePtBr(DateTime.Now.ToLocalTime());
            this.objDataFinalizado = new Utils().convertDatePtBr(DateTime.Now.ToLocalTime());
            this.CashInSent = false;
        }


        public double getTempoJogando() { return this.objTempoJogando; }
        public void setTempoJogando(double _tempoJogando) { this.objTempoJogando = _tempoJogando; }
        public int getPermanenciaJogo() { return this.targetForPayment; }
        public void setPermanenciaJogo(int _permanenciaJogo) { this.targetForPayment = _permanenciaJogo; }
        public decimal getCashPayment() { return this.CashPayment; }
        public void setCashPayment(decimal objCashPayment) { this.CashPayment = objCashPayment; }
        public bool getCashInStatus() { return this.CashInSent; }
        public void setCashInStatus(bool objCashInStatus) { this.CashInSent = objCashInStatus; }

    }
}
