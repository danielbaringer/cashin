using System;

namespace ClassLibrary.Gaas.Shared
{
    public class Utils
    {

        public Utils() { }

        public Guid geraNewGuiid() {

            return Guid.NewGuid();
        }

        public object convertToString(object valor) {

            try {
                return valor.ToString();
            } catch (Exception ex){
                return valor;
            }

        }
        public DateTime convertDatePtBr(DateTime dtaParametro) {

            DateTime dta = dtaParametro.ToLocalTime();
            DateTime dtaStr = new DateTime().ToLocalTime();


            try
            {
                dtaStr = DateTime.Parse(dta.ToString("dd/MM/yyyy HH:mm:ss")).ToLocalTime();
            }
            catch (Exception xp) { 
            }

           
            return dtaStr;

        }



    }
}
