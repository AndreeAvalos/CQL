using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Servidor.NOSQL.Modelos
{
    public class Fila
    {
        List<Valor> valores;

        public List<Valor> Valores { get => valores; set => valores = value; }

        public string ArmarRespuesta(List<Columna> ids)
        {
            string salida = "";
            foreach (Columna id in ids)
            {
                foreach (Valor item in this.valores)
                {
                    if (id.Name.Equals(item.Id)) salida += item.ArmarRespuesta();
                }
            }
            return salida;

        }
    }
}
