using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Servidor.Models
{
    public class Truncate_Table : Instruccion
    {
        string id_tabla;

        public Truncate_Table(string id_tabla)
        {
            this.id_tabla = id_tabla;
        }

        public object Recolectar(TablaDeSimbolos ts) { return null; }
        public object Ejecutar(TablaDeSimbolos ts)
        {
            if (Program.sistema.En_uso())
            {
                if (Program.sistema.existTable(id_tabla.ToLower()))
                {
                    bool eliminado = Program.sistema.truncateTable(id_tabla.ToLower());
                    if (eliminado)
                    {
                        //informar que se elimino con exito
                    }
                    else {
                        // informar que es un error interno;
                    }
                }
            }

            return null;
        }
    }
}
