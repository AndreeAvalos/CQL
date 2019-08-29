﻿using Servidor.NOSQL.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Servidor.Models.FCL
{
    public class LOG : Instruccion
    {
        int line, column;
        List<string> salida;
        List<Valor> valores;

        public LOG(int line, int column, List<Valor> valores)
        {
            this.line = line;
            this.column = column;
            this.salida = new List<string>();
            this.valores = valores;
        }

        public object Ejecutar(TablaDeSimbolos ts)
        {
            bool is_ok = true;
            string salida2 = "";
            foreach (Valor item in valores)
            {
                if (item.Sub_tipo == Tipo.VARIABLE)
                {
                    if (ts.existID(item.Val.ToString()))
                    {
                        salida2 += ts.getValor(item.Val.ToString());
                    }
                    else
                    {
                        salida.Add(Program.buildError(getLine(), getColumn(), "Semantico", "Variable " + item.Val + " no se ha declarado."));
                        is_ok = false;
                    }
                    if (!is_ok) break;
                }
                else
                {
                    salida2 += item.Val.ToString();

                }
            }
            if (is_ok)
                salida.Add(Program.buildMessage(salida2));
            return null;
        }

        public int getColumn()
        {
            return this.line;
        }

        public int getLine()
        {
            return this.column;
        }

        public List<string> getSalida()
        {
            return this.salida;
        }

        public object Recolectar(TablaDeSimbolos ts)
        {
            return null;
        }
    }
}
