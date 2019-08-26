using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Servidor.NOSQL.Modelos
{
    public class Fila
    {
        List<Valor> valores;

        public List<Valor> Valores { get => valores; set => valores = value; }


        public string writeImportFile()
        {

            string text = "<\n";

            for (int i = 0; i < valores.Count; i++)
            {
                if (i == valores.Count - 1)
                {
                    text += valores.ElementAt(i).execCommit(1) + "\n";
                }
                else
                {
                    text += valores.ElementAt(i).execCommit(1) + ",\n";
                }
            }
            text += ">";
            return text;
        }
        public string execCommit(int num_tabs)
        {
            string tabs = Program.getTabulaciones(num_tabs);
            string salida = "";
            for (int i = 0; i < valores.Count; i++)
            {
                if (i == valores.Count - 1)
                {


                }

            }

            return salida;
        }


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
