using System.Collections.Generic;

namespace ClassLibrary.Gaas.Models
{
    public abstract class ConstrutorObjetivoBase : BaseModel
    {


        public UsuarioModel ObjUsr { get; private set; }
        public ObjetivoModel ObjParametros { get; private set;  }
        public List<MiniGameModel> ListaMiniGames { get; private set; }

        public ConstrutorObjetivoBase()
        {

            this.ObjUsr = new UsuarioModel();
            this.ObjParametros = new ObjetivoModel();
            this.ListaMiniGames = new List<MiniGameModel>();

        }



        public ConstrutorObjetivoBase(bool statusCashIn, UsuarioModel dadosUser, ObjetivoModel dadosObj, MiniGameModel dadosMiniGame)
        {


            this.ObjUsr = dadosUser;
            this.ObjParametros = dadosObj;
            this.ListaMiniGames.Add(dadosMiniGame);

        }
        public UsuarioModel getUsuarioGame() { return ObjUsr; }
        public void setUsuarioGame(UsuarioModel objUser) { this.ObjUsr = objUser; }
        public ObjetivoModel getObjetivoGame() { return ObjParametros; }
        public void setObjetivoGame(ObjetivoModel objModel) { this.ObjParametros = objModel; }

        public List<MiniGameModel> listarMiniGames() { return ListaMiniGames; }
        public void addMiniGames(MiniGameModel novoObjetivoMiniGame) { this.ListaMiniGames.Add(novoObjetivoMiniGame); }


    }
}
