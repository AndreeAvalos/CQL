using Servidor.NOSQL.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Servidor.Models.USER_TYPES
{
    public class Create_Type : Instruccion
    {
        string id;
        bool existe;
        List<Atributo> atributos = new List<Atributo>();
        List<string> salida = new List<string>();
        int line, column;

        public Create_Type(string id, bool existe, List<Atributo> atributos, int line, int column)
        {
            this.id = id;
            this.existe = existe;
            this.atributos = atributos;
            this.line = line;
            this.column = column;
        }
        public Tipo getType()
        {
            return Tipo.USER_TYPES;
        }

        public object Ejecutar(TablaDeSimbolos ts)
        {
            if (Program.sistema.En_uso())
            {
                if (!Program.sistema.existeObjeto(id))
                {
                    string id_repetido = "";
                    if (existe == true)
                    {
                        //no hace nada
                        salida.Add(Program.buildError(getLine(), getColumn(), "Semantico", id + " TypeAlreadyExists."));
                        // marcar error
                    }
                    else
                    {
                        int repetidos;
                        bool no_hay_repetidos = true;
                        foreach (Atributo item in atributos)
                        {
                            repetidos = 0;
                            foreach (Atributo item2 in atributos)
                            {
                                if (item.Name.Equals(item2.Name))
                                {
                                    repetidos++;
                                    if (repetidos < 2)
                                    {
                                        //no hacemos nada
                                    }
                                    else
                                    {
                                        //mostrar error
                                        no_hay_repetidos = false;
                                    }
                                }
                                if (!no_hay_repetidos)
                                {
                                    id_repetido = item.Name;
                                    break;
                                }
                            }
                        }
                        if (no_hay_repetidos)
                        {
                            //si son primitivos o objetos
                            bool is_primitivo;
                            bool is_objeto;
                            bool is_ok = true;
                            foreach (Atributo item in atributos)
                            {

                                if (item.Collection)
                                {
                                    if (item.Type.ToLower().Equals("set") || item.Type.ToLower().Equals("list"))
                                    {
                                        if (!Program.comprobarPrimitivo(item.Attr1.ToLower()))
                                        {
                                            is_primitivo = false;
                                            if (!Program.sistema.existeObjeto(item.Attr1.ToLower())) is_objeto = false;
                                            else { is_primitivo = true; is_objeto = true; }
                                        }
                                        else
                                        {
                                            is_primitivo = true; is_objeto = true;
                                        }

                                        if (!is_primitivo && !is_objeto)
                                        {
                                            is_ok = false;

                                            //informar que no existe ese tipo de dato
                                            salida.Add(Program.buildError(getLine(), getColumn(), "Semantico", "El tipo " + item.Attr1 + " no es primitivo, ni es parte de los objetos de la base de datos."));
                                        }
                                    }
                                    else
                                    {
                                        if (!Program.comprobarPrimitivo(item.Attr1.ToLower()))
                                        {
                                            is_primitivo = false;
                                            if (!Program.sistema.existeObjeto(item.Attr1.ToLower())) is_objeto = false;
                                            else { is_primitivo = true; is_objeto = true; }
                                        }
                                        else
                                        {
                                            is_primitivo = true; is_objeto = true;
                                        }

                                        if (!is_primitivo && !is_objeto)
                                        {
                                            is_ok = false;

                                            //informar que no existe ese tipo de dato
                                            salida.Add(Program.buildError(getLine(), getColumn(), "Semantico", "El tipo " + item.Attr1 + " no es primitivo, ni es parte de los objetos de la base de datos."));
                                        }
                                        if (!Program.comprobarPrimitivo(item.Attr2.ToLower()))
                                        {
                                            is_primitivo = false;
                                            if (!Program.sistema.existeObjeto(item.Attr2.ToLower())) is_objeto = false;
                                            else { is_primitivo = true; is_objeto = true; }
                                        }
                                        else
                                        {
                                            is_primitivo = true; is_objeto = true;
                                        }

                                        if (!is_primitivo && !is_objeto)
                                        {
                                            is_ok = false;

                                            //informar que no existe ese tipo de dato
                                            salida.Add(Program.buildError(getLine(), getColumn(), "Semantico", "El tipo " + item.Attr2 + " no es primitivo, ni es parte de los objetos de la base de datos."));
                                        }
                                    }

                                }
                                else
                                {
                                    if (!Program.comprobarPrimitivo(item.Type.ToLower()))
                                    {
                                        is_primitivo = false;
                                        if (!Program.sistema.existeObjeto(item.Type.ToLower())) is_objeto = false;
                                        else { is_primitivo = true; is_objeto = true; }
                                    }
                                    else
                                    {
                                        is_primitivo = true; is_objeto = true;
                                    }

                                    if (!is_primitivo && !is_objeto)
                                    {
                                        is_ok = false;

                                        //informar que no existe ese tipo de dato
                                        salida.Add(Program.buildError(getLine(), getColumn(), "Semantico", "El tipo " + item.Type + " no es primitivo, ni es parte de los objetos de la base de datos."));
                                    }
                                }

                            }
                            if (is_ok)
                            {
                                Objeto new_objeto = new Objeto { Name = id, Atributos = atributos };
                                Program.sistema.addObjeto(new_objeto);
                                salida.Add(Program.buildMessage("Objeto " + id + " se creo con exito."));
                            }
                        }
                        else
                        {
                            salida.Add(Program.buildError(getLine(), getColumn(), "Semantico", "El identificador " + id_repetido + " ya existe en este contexto."));
                        }
                    }
                }
                else
                {
                    salida.Add(Program.buildError(getLine(), getColumn(), "Semantico", id + " UserAlreadyExists."));
                }

            }
            else
            {
                //no hay ninguna base de datos seleccionada.
                salida.Add(Program.buildError(getLine(), getColumn(), "Semantico", "BDDontExists."));
            }

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
