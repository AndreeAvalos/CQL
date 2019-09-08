using Servidor.Analizador.CHISON;
using Servidor.NOSQL.Modelos;
using System;
using System.Collections.Generic;

namespace Servidor.Models.FCL
{
    public class Asignacion_Variable : Instruccion
    {
        Tipo real_type;
        string type;
        bool global;
        List<Variable> variables_asignar;
        int line, column;
        List<string> salida = new List<string>();

        public Asignacion_Variable(Tipo real_type, string type, bool global, List<Variable> variables_asignar, int line, int column)
        {
            this.real_type = real_type;
            this.type = type;
            this.global = global;
            this.variables_asignar = variables_asignar;
            this.line = line;
            this.column = column;
        }
        public void clearSalida()
        {
            this.salida.Clear();
        }

        public object Ejecutar(TablaDeSimbolos ts)
        {
            if (!global)
            {
                foreach (Variable item in variables_asignar)
                {
                    if (!ts.existID_AA(item.Id.ToLower()))
                    {
                        Simbolo new_simbol = new Simbolo(real_type, item.Id);
                        new_simbol.Tipo_asignado = type;
                        string name = item.Valor.GetType().Name;
                        if (item.Instanciada)
                        {
                            if (real_type == Tipo.USER_TYPES)
                            {
                                if (name.Equals("Tipo_Objeto"))
                                {
                                    Tipo_Objeto objext = (Tipo_Objeto)item.Valor;
                                    List<Tipo_Objeto> objeto = (List<Tipo_Objeto>)Program.sistema.buildObject(objext.Name.ToLower());
                                    new_simbol.Valor = objeto;
                                }
                                else
                                {
                                    List<Tipo_Collection> objeto = (List<Tipo_Collection>)item.Valor;
                                    new_simbol.Valor = Program.sistema.getValor(type.ToLower(), objeto, ts);
                                }
                            }
                            else if (real_type == Tipo.NUMERO || real_type == Tipo.ENTERO)
                            {
                                if (name.Equals("Variable"))
                                {
                                    Variable aux_var = (Variable)item.Valor;

                                    object valor = ts.getValorByAttr(aux_var.Id, aux_var.Atributos);
                                    new_simbol.Valor = valor;
                                }
                                else
                                {
                                    Operacion valor = (Operacion)item.Valor;
                                    try
                                    {
                                        new_simbol.Valor = valor.Ejecutar(ts);
                                        if (new_simbol.Valor == null) new_simbol.Valor = item.Valor;
                                    }
                                    catch (Exception)
                                    {
                                        new_simbol.Valor = valor;
                                    }
                                }
                            }
                            else if (real_type == Tipo.MAP)
                            {
                                Map objeto = (Map)item.Valor;
                                new_simbol.Valor = objeto;
                            }
                            else if (real_type == Tipo.LIST)
                            {
                                Lista objeto = (Lista)item.Valor;
                                new_simbol.Valor = objeto;
                            }
                        }
                        else
                        {
                            if (real_type == Tipo.NUMERO) new_simbol.Valor = 0;
                            if (real_type == Tipo.USER_TYPES || real_type == Tipo.MAP || real_type == Tipo.LIST) new_simbol.Valor = null;

                        }
                        ts.AddLast(new_simbol);
                    }
                    else
                    {
                        salida.Add(Program.buildError(getLine(), getColumn(), "Semantico", item.Id + " ObjectAlreadyExists3"));
                    }

                }
            }
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
            return Tipo.VARIABLE;
        }

        public object Recolectar(TablaDeSimbolos ts)
        {
            if (global)
            {
                foreach (Variable item in variables_asignar)
                {
                    if (!ts.existID_AA(item.Id.ToLower()))
                    {
                        Simbolo new_simbol = new Simbolo(real_type, item.Id)
                        {
                            Tipo_asignado = type
                        };
                        string name = item.Valor.GetType().Name;
                        if (item.Instanciada)
                        {
                            if (real_type == Tipo.USER_TYPES)
                            {
                                if (name.Equals("Tipo_Objeto"))
                                {
                                    Tipo_Objeto objext = (Tipo_Objeto)item.Valor;
                                    List<Tipo_Objeto> objeto = (List<Tipo_Objeto>)Program.sistema.buildObject(objext.Name.ToLower());
                                    new_simbol.Valor = objeto;
                                }
                                else
                                {
                                    List<Tipo_Collection> objeto = (List<Tipo_Collection>)item.Valor;
                                    new_simbol.Valor = Program.sistema.getValor(type.ToLower(), objeto, ts);
                                }
                            }
                            else if (real_type == Tipo.NUMERO || real_type == Tipo.ENTERO)
                            {
                                if (name.Equals("Variable"))
                                {
                                    Variable aux_var = (Variable)item.Valor;

                                    object valor = ts.getValorByAttr(aux_var.Id, aux_var.Atributos);
                                    new_simbol.Valor = valor;
                                }
                                else
                                {
                                    Operacion valor = (Operacion)item.Valor;
                                    try
                                    {
                                        new_simbol.Valor = valor.Ejecutar(ts);
                                        if (new_simbol.Valor == null) new_simbol.Valor = item.Valor;
                                    }
                                    catch (Exception)
                                    {
                                        new_simbol.Valor = valor;
                                    }
                                }
                            }
                            else if (real_type == Tipo.MAP)
                            {
                                Map objeto = (Map)item.Valor;
                                new_simbol.Valor = objeto;
                            }
                            else if (real_type == Tipo.LIST)
                            {
                                Lista objeto = (Lista)item.Valor;
                                new_simbol.Valor = objeto;
                            }
                        }
                        else
                        {
                            if (real_type == Tipo.NUMERO) new_simbol.Valor = 0;
                            if (real_type == Tipo.USER_TYPES || real_type == Tipo.MAP || real_type == Tipo.LIST) new_simbol.Valor = null;

                        }
                        ts.AddLast(new_simbol);
                    }
                    else
                    {
                        salida.Add(Program.buildError(getLine(), getColumn(), "Semantico", item.Id + " ObjectAlreadyExists4"));
                    }

                }
            }
            //else salida.Add(Program.buildError(getLine(), getColumn(), "Semantico", item.Id + " ObjectAlreadyExists"));
            return null;

        }
    }
}
