using Servidor.NOSQL.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Servidor.Models
{
    public class Create_Table : Instruccion
    {
        private string id;
        List<Columna> columnas;
        bool existe;
        bool compuesta;

        public Create_Table(string id, List<Columna> columnas, bool existe, bool compuesta)
        {
            this.id = id;
            this.columnas = columnas;
            this.existe = existe;
            this.compuesta = compuesta;
        }

        public object Recolectar(TablaDeSimbolos ts) { return null; }
        public object Ejecutar(TablaDeSimbolos ts)
        {
            if (Program.sistema.En_uso())
            {
                if (Program.sistema.existTable(id))
                {
                    if (existe == true)
                    {
                        //no hace nada
                    }
                    else
                    {
                        // marcar error
                    }
                }
                else
                {
                    bool validacion_counter_and_PK = true;
                    bool existe_algun_counter = false;
                    List<Columna> columnas_aux = new List<Columna>();
                    //lista auxiliar para poder saber que llaves se van a modificar a primarias

                    //si es compuesta entonces pasamos a convertir

                    if (compuesta)
                    {
                        List<string> llaves_compuestas = new List<string>();
                        //obtenemos las llaves primarias a modificarse.
                        foreach (Columna item in columnas)
                        {
                            if (item.Modify) llaves_compuestas.Add(item.Name);
                        }
                        //ahora pasamos a verificar si existe algun counter en las columnas
                        foreach (Columna item in columnas)
                        {
                            if (item.Type.ToLower().Equals("counter"))
                            {
                                existe_algun_counter = true;
                            }
                        }
                        //si existe algun counter verificamos que este en la lista de llaves primarias
                        if (existe_algun_counter)
                        {
                            //vamos a recorrer todas las llaves compuestas para verificar si son conter todas
                            bool todas_counter = true;
                            foreach (string item in llaves_compuestas)
                            {
                                //recorremos todas las columnas en busca si todas las llaves coinciden con counter
                                foreach (Columna item2 in columnas)
                                {
                                    //analizamos en nombre de la columna con el nombre de la llave compuesta
                                    if (item2.Name.Equals(item))
                                    {
                                        //verificamos si es counter, si no, no tenemos match ejemplo counter, string 
                                        if (!item2.isCounter())
                                        {
                                            todas_counter = false;
                                            break;
                                        }
                                    }
                                }
                                if (!todas_counter) break;
                            }
                            if (todas_counter)
                            {
                                foreach (string item in llaves_compuestas)
                                {
                                    //recorremos todas las columnas para asignar los tipos primary key
                                    foreach (Columna item2 in columnas)
                                    {
                                        //analizamos en nombre de la columna con el nombre de la llave compuesta
                                        if (item2.Name.Equals(item))
                                        {
                                            item2.Pk = true;  
                                        }
                                    }
                                }
                                columnas_aux = columnas;

                            }
                            else
                            {
                                //error ya que no todas cumplen con ser counter al ser compuestas
                                return null;
                            }
                        }//si no existe algun counter entonces solo agregamos las filas
                        else
                        {
                            foreach (string item in llaves_compuestas)
                            {
                                columnas_aux = new List<Columna>();
                                //recorremos todas las columnas para asignar los tipos primary key
                                foreach (Columna item2 in columnas)
                                {
                                    //analizamos en nombre de la columna con el nombre de la llave compuesta
                                    if (item2.Name.Equals(item))
                                    {
                                        item2.Pk = true;
                                    }
                                }
                            }
                            columnas_aux = columnas;
                        }


                    }//si no es compuesta, tenemos que ver los datos
                    else
                    {
                        foreach (Columna item in columnas)
                        {
                            if (item.isCounter() && !item.isPK())
                            {
                                validacion_counter_and_PK = false;
                                //ejecutar error de que se encontro un counter y no es pk
                                break;
                            }

                        }
                        if (validacion_counter_and_PK)
                        {
                            columnas_aux = columnas;
                        }
                        else {
                            //ejecutar resulucion de errores
                            return null;
                        }

                    }
                    Tabla tabla_aux = new Tabla();
                    tabla_aux.Name = id;
                    tabla_aux.Columnas = columnas_aux;
                    tabla_aux.Exportada = false;

                    if (Program.sistema.addTable(tabla_aux))
                    {
                        //Mandamos mensaje que se creo la tabla con exito
                    }
                    else
                    {
                        //mandamos mensaje que no se pudo por que no hay ninguna base de datos en uso.
                        return null;
                    }
                    

          
                }
            }
            else
            {
                //no hay ninguna base de datos seleccionada.
            }

            return null;
        }
    }
}
