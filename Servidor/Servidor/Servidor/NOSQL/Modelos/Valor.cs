using Servidor.Analizador.CHISON;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Servidor.NOSQL.Modelos
{
    public class Valor
    {
        string id;
        object val;

        public Valor(string id, object val)
        {
            this.id = id;
            this.Val = val;
        }

        public string Id { get => id; set => id = value; }
        public object Val { get => val; set => val = value; }

        public string ArmarRespuesta() {

            try
            {
                List<Tipo_Objeto> lst = (List<Tipo_Objeto>)val;
                return "<td>DATOS_OBJETO</td>";
            }
            catch (Exception)
            {

                try
                {
                    List<string> lst = (List<string>)val;
                    return "<td>DATOS_LISTA</td>";
                }
                catch (Exception)
                {

                    return "<td>"+this.val+"</td>";
                }
            }

        }
    }
}
