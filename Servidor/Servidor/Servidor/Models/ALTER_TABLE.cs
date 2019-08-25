using Servidor.NOSQL.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Servidor.Models
{
    public class ALTER_TABLE : Instruccion
    {
        string id_tabla;
        bool add_column = true;
        List<Columna> columnas_agregar;
        List<string> columnas_eliminar;

        public ALTER_TABLE(string id_tabla, bool add_column, List<Columna> columnas_agregar)
        {
            this.id_tabla = id_tabla;
            this.add_column = add_column;
            this.columnas_agregar = columnas_agregar;
        }

        public ALTER_TABLE(string id_tabla, bool add_column, List<string> columnas_eliminar)
        {
            this.id_tabla = id_tabla;
            this.add_column = add_column;
            this.columnas_eliminar = columnas_eliminar;
        }

        public object Recolectar(TablaDeSimbolos ts) { return null; }
        public object Ejecutar(TablaDeSimbolos ts)
        {
            if (Program.sistema.existTable(id_tabla.ToLower()))
            {
                //si hay que agregar columnas
                if (add_column)
                {
                    //verificamos si hay algun counter

                    bool is_ok = true;
                    foreach (Columna item in columnas_agregar)
                    {
                        if (item.isCounter())
                        {
                            is_ok = false;
                            //desplegar error
                        }
                    }
                    if (is_ok)
                    {
                        bool es_primitivo = true;
                        bool es_objeto = true;
                        bool existe_error = false;
                        //aqui puede ir la comprobacion de objetos
                        foreach (Columna item in columnas_agregar)
                        {
                            if (!Program.comprobarPrimitivo(item.Type))
                                es_primitivo = false;
                            if (!Program.sistema.existeObjeto(item.Type))
                                es_objeto = false;

                            if (!es_primitivo && !es_objeto)
                            {
                                //informar de error y no agregar
                                existe_error = true;
                            }
                        }
                        if (existe_error == false)
                        {
                            foreach (Columna item in columnas_agregar)
                            {
                                Program.sistema.addColumn(id_tabla.ToLower(), item);
                            }
                        }
                        else {
                            //reportar que no se pudo crear la db
                        }
                        return null;
                    }
                    else
                    {
                        //no insertar 
                        return null;
                    }

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
                        }
                    }

                    if (is_ok)
                    {
                        foreach (string item in columnas_eliminar)
                        {
                            if (Program.sistema.existColumn(id_tabla.ToLower(), item.ToLower())) Program.sistema.dropColumn(id_tabla.ToLower(), item.ToLower());
                        }

                    }

                    return null;
                }

            }
            else
            {
                //error por que no existe la tabla
                return null;

            }
        }
    }
}
