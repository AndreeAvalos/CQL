﻿using Servidor.NOSQL.Modelos;
using System;
using System.Collections.Generic;

namespace Servidor.Models.FCL
{
    public class LOG : Instruccion
    {
        int line, column;
        List<string> salida = new List<string>();
        List<Valor> valores;

        public LOG(int line, int column, List<Valor> valores)
        {
            this.line = line;
            this.column = column;
            this.valores = valores;
        }
        public void clearSalida()
        {
            this.salida.Clear();
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
                        try
                        {
                            salida2 += ((Operacion)ts.getValor(item.Val.ToString())).Ejecutar(ts);
                        }
                        catch (Exception)
                        {

                            salida2 += ts.getValor(item.Val.ToString());
                        }
                    }
                    else
                    {
                        salida.Add(Program.buildError(getLine(), getColumn(), "Semantico", "Variable " + item.Val + " no se ha declarado."));
                        is_ok = false;
                    }
                    if (!is_ok) break;
                }
                else if (item.Sub_tipo == Tipo.VARIABLE_ATRIBUTOS)
                {
                    Variable aux_var = (Variable)item.Val;
                    if (ts.existID(aux_var.Id))
                    {
                        object result = ts.getValorByAttr(aux_var.Id, aux_var.Atributos);
                        if (result != null)
                            salida2 += result.ToString();
                        else
                            salida.Add(Program.buildError(getLine(), getColumn(), "Semantico", "NO EXISTE ATRIBUTO"));
                    }
                    else
                    {
                        salida.Add(Program.buildError(getLine(), getColumn(), "Semantico", "Variable " + item.Val + " no se ha declarado."));
                        is_ok = false;
                    }
                    if (!is_ok) break;
                }
                else if (item.Sub_tipo == Tipo.VARIABLE_METODOS)
                {
                    Variable aux_var = (Variable)item.Val;
                    if (ts.existID(aux_var.Id))
                    {
                        
                        Tipo tipo = ts.getType(aux_var.Id);
                        object aux = Program.getValor(tipo, aux_var.Id, aux_var.Valor, ts);
                        if (aux != null) salida2+= aux;
                        else
                        {
                            salida.Add(Program.buildError(getLine(), getColumn(), "Semantico", " IndexOutException."));
                            is_ok = false;
                        }

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
            return this.column;
        }

        public int getLine()
        {
            return this.line;
        }

        public List<string> getSalida()
        {
            return this.salida;
        }

        public Tipo getType()
        {
            return Tipo.LOG;
        }

        public object Recolectar(TablaDeSimbolos ts)
        {
            return null;
        }
    }
}
