using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Servidor.Models
{
    public class Drop_Table : Instruccion
    {
        string id_tabla;
        bool existe;

        public Drop_Table(string id_tabla, bool existe)
        {
            this.id_tabla = id_tabla;
            this.existe = existe;
        }

        public object Recolectar(TablaDeSimbolos ts) { return null; }
        public object Ejecutar(TablaDeSimbolos ts)
        {
            if (Program.sistema.En_uso())
            {
                if (!Program.sistema.existTable(id_tabla.ToLower()))
                {
                    if (existe == true)
                    {
                        //no hace nada
                    }
                    else
                    {
                        // marcar error
                    }
                }
                else
                {
                    bool eliminado = Program.sistema.dropTable(id_tabla.ToLower());
                    if (eliminado)
                    {
                        //mandar mensaje de eliminacion
                    }
                    else {
                        // mandar mensaje que hubo un error interno.
                    }

                }
            }

            return null;
        }
    }
}
