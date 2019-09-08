using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Servidor.Models.CASTEOS
{
    public class Casteo
    {
        public object castear(Tipo tipo, object valor)
        {
            try
            {
                if (tipo == Tipo.ENTERO)
                {
                    return Convert.ToInt32(valor);
                }

            }
            catch (Exception)
            {

                return null;
            }
            return null;
        }
        public bool comprobarCasteo(Tipo tipo, object valor)
        {

            try
            {
                if (tipo == Tipo.ENTERO)
                {
                    Convert.ToInt32(valor);
                    return true;
                }
                else if (tipo == Tipo.DECIMAL)
                {
                    Convert.ToDouble(valor);
                    return true;
                }
                else if (tipo == Tipo.CADENA) {
                    Convert.ToString(valor);
                    return true;
                }

            }
            catch (Exception)
            {

                return false;
            }
            return false;
        }
    }
}
