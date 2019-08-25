using Irony.Parsing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Servidor.Models
{
    public class Truncate_Table : Instruccion
    {
        string id_tabla;
        private ParseTreeNodeList lst_nodos;
        public List<string> salida = new List<string>();

        public Truncate_Table(string id_tabla, ParseTreeNodeList nodo)
        {
            this.id_tabla = id_tabla;
            this.lst_nodos = nodo;
        }
        public List<string> getSalida()
        {

            return salida;
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
                    else
                    {
                        // informar que es un error interno;
                    }
                }
                else {

                    Console.WriteLine("holaMundo");
                    List<int> LC = obtenerLC(id_tabla);
                    if ( LC != null) {
                        salida.Add(Program.buildError(LC.ElementAt(0),LC.ElementAt(1),"Semantico", "La tabla que desea truncar no existe en la base de datos actual."));
                    }

                }
            }

            return null;
        }

        private List<int> obtenerLC(string token) {
            List<int> linea_columna = new List<int>();
            foreach (ParseTreeNode item in lst_nodos)
            {
                if (item.Token.Text.ToLower().Equals(token.ToLower())) {
                    linea_columna.Add(item.Token.Location.Line);
                    linea_columna.Add(item.Token.Location.Column);
                    return linea_columna;
                }
            }
            return null;

        }
    }
}
