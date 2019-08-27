using Servidor.NOSQL.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Servidor.Models.USER_TYPES
{
    public class Alter_Type : Instruccion
    {
        string id;
        List<Atributo> atributos = new List<Atributo>();
        List<object> lst_atributos = new List<object>();
        List<string> salida = new List<string>();
        int line, column;
        bool add = true;

        public Alter_Type(string id, List<object> lst_atributos, int line, int column, bool add)
        {
            this.id = id;
            this.lst_atributos = lst_atributos;
            this.line = line;
            this.column = column;
            this.add = add;
        }

        public Alter_Type(string id, List<Atributo> atributos, int line, int column, bool add)
        {
            this.id = id;
            this.atributos = atributos;
            this.line = line;
            this.column = column;
            this.add = add;
        }

        public object Ejecutar(TablaDeSimbolos ts)
        {
            if (Program.sistema.En_uso())
            {
                if (Program.sistema.existeObjeto(id))
                {
                    if (add)
                    {
                        string id_repetido = "";
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
                            if (is_ok)
                            {
                                is_ok = true;
                                foreach (Atributo item in atributos)
                                {
                                    if (Program.sistema.existAtributos(id.ToLower(), item.Name.ToLower()))
                                    {
                                        is_ok = false;
                                        salida.Add(Program.buildError(getLine(), getColumn(), "Semantico", "El identificador " + item.Name + " ya existe en el objeto " + id + "."));
                                    }
                                }
                                if (is_ok)
                                {
                                    Program.sistema.addAtributo(id, atributos);
                                    salida.Add(Program.buildMessage("Se han realizado los cambios en " + id + "  con exito."));
                                }
                            }
                        }
                        else
                        {
                            salida.Add(Program.buildError(getLine(), getColumn(), "Semantico", "El identificador " + id_repetido + " ya existe en este contexto."));
                        }
                    }
                    else
                    {
                        string id_repetido = "";
                        int repetidos;
                        bool no_hay_repetidos = true;
                        foreach (string item in lst_atributos)
                        {
                            repetidos = 0;
                            foreach (string item2 in lst_atributos)
                            {
                                if (item.Equals(item2))
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
                                    id_repetido = item;
                                    break;
                                }
                            }
                        }
                        if (no_hay_repetidos)
                        {
                            bool is_ok = true;

                            foreach (string item in lst_atributos)
                            {
                                if (!Program.sistema.existAtributos(id.ToLower(), item.ToLower()))
                                {
                                    salida.Add(Program.buildError(getLine(), getColumn(), "Semantico", "El identificador " + item + " no existe en los atributos de " + id + "."));
                                    is_ok = false;
                                }
                            }
                            if (is_ok)
                            {
                                foreach (string item in lst_atributos)
                                {
                                    Program.sistema.deleteAtributo(id, item);
                                }

                                salida.Add(Program.buildMessage("Se han realizado los cambios en " + id + "  con exito."));
                            }
                            //el de delete
                        }
                        else
                        {
                            salida.Add(Program.buildError(getLine(), getColumn(), "Semantico", "El identificador " + id_repetido + " ya existe en este contexto."));
                        }
                    }

                }
                else
                {
                    salida.Add(Program.buildError(getLine(), getColumn(), "Semantico", "El tipo " + id + " no existe en la base de datos."));
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

        public object Recolectar(TablaDeSimbolos ts)
        {
            throw new NotImplementedException();
        }
    }
}
