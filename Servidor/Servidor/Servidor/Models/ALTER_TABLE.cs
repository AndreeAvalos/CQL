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
        List<object> columnas_eliminar;

        public ALTER_TABLE(string id_tabla, bool add_column, List<Columna> columnas_agregar)
        {
            this.id_tabla = id_tabla;
            this.add_column = add_column;
            this.columnas_agregar = columnas_agregar;
        }

        public ALTER_TABLE(string id_tabla, bool add_column, List<object> columnas_eliminar)
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
                    List<Columna> columnaux = columnas_agregar;

                    int repetidos = 0;
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
                                    no_hay_repetidos = false;
                                }
                            }
                        }
                    }
                    if (no_hay_repetidos)
                    {

                        //verificamos si existen los tipos declarados
                        //si son primitivos o objetos
                        bool is_primitivo = true;
                        bool is_objeto = true;
                        bool is_ok = true;
                        foreach (Columna item in columnas_agregar)
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

                            if (!is_primitivo && !is_objeto) is_ok = false;
                            else
                            {
                                //informar que no existe ese tipo de dato
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
                                    }
                                }
                                //si no viene ninguno procedemos a insertar
                                if (is_ok)
                                {
                                    foreach (Columna item in columnas_agregar)
                                    {
                                        Program.sistema.addColumn(id_tabla.ToLower(), item);
                                    }
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
                        }
                        else
                        {
                            if (Program.sistema.isPk(id_tabla.ToLower(), item.ToLower())) is_ok = false;
                            else
                            {
                                //informar que la llave es primaria, por lo tanto no se puede eliminar
                            }
                        }
                    }

                    if (is_ok)
                    {
                        bool eliminado = false;
                        foreach (string item in columnas_eliminar)
                        {
                            eliminado = Program.sistema.dropColumn(id_tabla.ToLower(), item.ToLower());
                            if (!eliminado)
                            {
                                //por alguna razon interna
                            }
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
