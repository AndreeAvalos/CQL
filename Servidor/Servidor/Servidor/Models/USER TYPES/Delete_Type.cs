using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Servidor.Models.USER_TYPES
{
    public class Delete_Type : Instruccion
    {
        private string name;
        private int linea;
        private int columna;
        List<string> salida = new List<string>();

        public Delete_Type(string name, int linea, int columna)
        {
            this.name = name;
            this.linea = linea;
            this.columna = columna;
        }
        public Tipo getType()
        {
            return Tipo.USER_TYPES;
        }
        public void clearSalida()
        {
            this.salida.Clear();
        }
        public object Ejecutar(TablaDeSimbolos ts)
        {
            if (Program.sistema.En_uso())
            {
                if (Program.sistema.existeObjeto(name))
                {
                    Program.sistema.deleteObjeto(name.ToLower());
                }
                else
                {
                    salida.Add(Program.buildError(getLine(), getColumn(), "Semantico", name + " TypeDontExists."));
                }


            }
            else
            {
                //no hay ninguna base de datos seleccionada.
                salida.Add(Program.buildError(getLine(), getColumn(), "Semantico", "No existe ninguna base de datos en uso."));
            }
            return null;
        }

        public int getColumn()
        {
            return this.columna;
        }

        public int getLine()
        {
            return this.linea;
        }

        public List<string> getSalida()
        {
            return this.salida;
        }

        public object Recolectar(TablaDeSimbolos ts)
        {
            throw new NotImplementedException();
        }
    }
}
