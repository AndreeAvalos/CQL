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

    }
}
