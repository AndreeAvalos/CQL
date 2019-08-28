using Servidor.NOSQL.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Servidor.Models
{
    public class Alter_Table : Instruccion
    {
        readonly string id_tabla;
        readonly bool add_column = true;
        readonly List<Columna> columnas_agregar;
        readonly List<object> columnas_eliminar;
        public List<string> salida = new List<string>();
        private readonly int linea;
        private readonly int columna;

        public List<string> getSalida()
        {

            return salida;
        }

        public Alter_Table(string id_tabla, bool add_column, List<Columna> columnas_agregar, int line, int column)
        {
            this.id_tabla = id_tabla;
            this.add_column = add_column;
            this.columnas_agregar = columnas_agregar;
            this.linea = line;
            this.columna = column;
        }

        public Alter_Table(string id_tabla, bool add_column, List<object> columnas_eliminar, int line, int column)
        {
            this.id_tabla = id_tabla;
            this.add_column = add_column;
            this.columnas_eliminar = columnas_eliminar;
            this.linea = line;
            this.columna = column;
        }

        public object Recolectar(TablaDeSimbolos ts) { return null; }
        public object Ejecutar(TablaDeSimbolos ts)
        {
            if (Program.sistema.En_uso())
            {
                if (Program.sistema.existTable(id_tabla.ToLower()))
                {
                    //si hay que agregar columnas
                    if (add_column)
                    {
                        List<Columna> columnaux = columnas_agregar;

                        int repetidos;
                        bool no_hay_repetidos = true;
                        foreach (Columna item in columnas_agregar)
                        {
                            repetidos = 0;
                            foreach (Columna item2 in columnaux)
                            {
                                if (item.Name.Equals(item2.Name))
                                {
                                    repetidos++;
                                    if (repetidos <= 1)
                                    {
                                        //no hacemos nada
                                    }
                                    else
                                    {
                                        //mostrar error

                                        salida.Add(Program.buildError(getLine(), getColumn(), "Semantico", "La columna " + item.Name + " existe en el mismo contexto "));
                                        no_hay_repetidos = false;
                                    }
                                }
                            }
                        }
                        if (no_hay_repetidos)
                        {

                            //verificamos si existen los tipos declarados
                            //si son primitivos o objetos
                            bool is_primitivo;
                            bool is_objeto;
                            bool is_ok = true;
                            foreach (Columna item in columnas_agregar)
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
                                            salida.Add(Program.buildError(getLine(), getColumn(), "Semantico", "El tipo " + item.Attr1 + " no es primitivo, ni es parte de los objetos de la base de datos."));
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

                                        salida.Add(Program.buildError(getLine(), getColumn(), "Semantico", "El tipo " + item.Type + " no es primitivo, ni es parte de los objetos de la base de datos."));
                                    }
                                }
                            }
                            //si existen los tipos pasar
                            if (is_ok)
                            {
                                //verificamos si existe ya el nombre de la columna que quiere insertar
                                is_ok = true;
                                foreach (Columna item in columnas_agregar)
                                {
                                    if (Program.sistema.existColumn(id_tabla.ToLower(), item.Name.ToLower()))
                                    {
                                        is_ok = false;
                                        //desplegar error

                                        salida.Add(Program.buildError(getLine(), getColumn(), "Semantico", item.Name + "ColumnException."));
                                    }
                                }
                                //si no existe pasamos
                                if (is_ok)
                                {
                                    //buscamos si en los datos existe algun counter
                                    is_ok = true;
                                    foreach (Columna item in columnas_agregar)
                                    {
                                        if (item.isCounter())
                                        {
                                            is_ok = false;
                                            //desplegar error

                                            salida.Add(Program.buildError(getLine(), getColumn(), "Semantico", item.Name + " CounterTypeException."));
                                        }
                                    }
                                    //si no viene ninguno procedemos a insertar
                                    if (is_ok)
                                    {
                                        foreach (Columna item in columnas_agregar)
                                        {

                                            Program.sistema.addColumn(id_tabla.ToLower(), item);
                                        }
                                        salida.Add(Program.buildMessage("Tabla " + id_tabla + " modificada con exito; Total de cambios: " + columnas_agregar.Count + "."));
                                    }
                                    else
                                    {
                                        //exist un tipo de counter y ya que no se puede 
                                    }
                                    return null;
                                }
                                else
                                {
                                    //no ejecutar
                                    return null;
                                }
                            }
                            else
                            {
                                //no insertar 
                                return null;
                            }
                        }
                        else return null;
                    }
                    else
                    {
                        bool is_ok = true;
                        foreach (string item in columnas_eliminar)
                        {
                            if (!Program.sistema.existColumn(id_tabla, item.ToLower()))
                            {
                                //informar que no existe columna
                                is_ok = false;

                                salida.Add(Program.buildError(getLine(), getColumn(), "Semantico", item + " ColumnException."));
                            }
                            else
                            {
                                if (Program.sistema.isPk(id_tabla.ToLower(), item.ToLower()))
                                {
                                    is_ok = false;
                                    salida.Add(Program.buildError(getLine(), getColumn(), "Semantico",item+ " CounterTypeException."));
                                }
                            }
                        }

                        if (is_ok)
                        {
                            bool eliminado;
                            foreach (string item in columnas_eliminar)
                            {
                                eliminado = Program.sistema.dropColumn(id_tabla.ToLower(), item.ToLower());
                                if (!eliminado)
                                {
                                    //por alguna razon interna
                                    salida.Add(Program.buildMessage("Error interno del servidor."));
                                }
                            }
                            salida.Add(Program.buildMessage("Tabla " + id_tabla + " modificada con exito; Total de cambios: " + columnas_eliminar.Count + "."));

                        }

                        return null;
                    }

                }
                else
                {
                    //error por que no existe la tabla

                    salida.Add(Program.buildError(getLine(), getColumn(), "Semantico", id_tabla + " TableDontExists."));
                    return null;

                }
            }
            else
            {
                //no hay ninguna base de datos seleccionada.
                salida.Add(Program.buildError(getLine(), getColumn(), "Semantico", "UseBDException."));
            }
            return null;
        }
        public int getLine()
        {
            return linea;
        }
        public int getColumn()
        {
            return columna;
        }
    }
}
