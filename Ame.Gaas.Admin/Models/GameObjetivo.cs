using ClassLibrary.Gaas.Business.ObjetivoMiniGame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ame.Gaas.Admin.Models
{
    public class GameObjetivo
    {

        public string usuario { get; set; }
        public string game { get; set; }

        private ObjetivoMiniGame obj;

        public GameObjetivo() { this.obj = new ObjetivoMiniGame(); }

        public bool registraInicioObjetivoGame(GameObjetivo dadosObj) {

            if (obj.informarInicioObj(dadosObj.usuario, dadosObj.game)) {
                return true;
            }

            return false;

        }

        public bool registraProgressoObjetivoGame(GameObjetivo dadosObj)
        {

            //if (obj.informarInicioObj(dadosObj.usuario, dadosObj.game))
            //{
            //    return true;
            //}

            return true;

        }

    }
}