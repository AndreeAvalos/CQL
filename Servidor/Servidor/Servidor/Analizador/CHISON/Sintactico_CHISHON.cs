using Irony.Parsing;
using Servidor.NOSQL.Estructuras;
using Servidor.NOSQL.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Servidor.Analizador.CHISON
{
    public class Sintactico_CHISHON
    {
        public List<string> salida = new List<string>();
        public Manejo db_nosql;

        #region Auxiliares

        #endregion

        public bool Validar(String entrada, Grammar gramatica)
        {
            LanguageData lenguaje = new LanguageData(gramatica);
            Parser parser = new Parser(lenguaje);
            ParseTree arbol = parser.Parse(entrada);
            if (arbol.Root != null) return true;
            else return false;
        }

        public ParseTreeNode Analizar(String entrada, Grammar gramatica)
        {
            LanguageData lenguaje = new LanguageData(gramatica);
            Parser parser = new Parser(lenguaje);
            ParseTree arbol = parser.Parse(entrada);
            ParseTreeNode raiz = arbol.Root;

            //instanciamos un objeto para la base de datos no-sql
            db_nosql = new Manejo();

            Instrucciones(raiz.ChildNodes.ElementAt(0).ChildNodes.ElementAt(2));


            return arbol.Root.ChildNodes.ElementAt(0);
        }

        private void Instrucciones(ParseTreeNode nodo)
        {
            if (nodo.ChildNodes.Count == 3)
            {
                Instrucciones(nodo.ChildNodes.ElementAt(0));
                Instruccion(nodo.ChildNodes.ElementAt(2));
            }
            else Instruccion(nodo.ChildNodes.ElementAt(0));

        }

        private void Instruccion(ParseTreeNode nodo)
        {

            string produccion = nodo.ChildNodes.ElementAt(0).ToString();

            switch (produccion)
            {
                case "USERS":
                    db_nosql.Usuarios = (List<Usuario>)Ejecutar(nodo.ChildNodes.ElementAt(0).ChildNodes.ElementAt(3));
                    salida.Add("Se agregaron usuarios al sistema ");
                    break;
                case "DATABASES":
                    db_nosql.Databases = (List<Database>)Ejecutar(nodo.ChildNodes.ElementAt(0).ChildNodes.ElementAt(3));
                    salida.Add("Se agrego la base de datos al sistema ");
                    break;
            }


        }

        private object Ejecutar(ParseTreeNode nodo)
        {

            string produccion = nodo.Term.Name.ToString();

            switch (produccion)
            {
                case "DATABASES2":
                    if (nodo.ChildNodes.Count == 3)
                    {
                        List<Database> bases = (List<Database>)Ejecutar(nodo.ChildNodes.ElementAt(0));
                        bases.Add((Database)Ejecutar(nodo.ChildNodes.ElementAt(2)));
                        return bases;
                    }
                    else if (nodo.ChildNodes.Count == 1)
                    {
                        List<Database> bases = new List<Database>
                        {
                            (Database) Ejecutar(nodo.ChildNodes.ElementAt(0))
                        };
                        return bases;
                    }
                    else
                    {
                        return new List<Database>();
                    }

                case "DATABASE":
                    List<Tipo_Objeto> lst_objetos = (List<Tipo_Objeto>) Ejecutar(nodo.ChildNodes.ElementAt(1));
                    Database new_db = new Database();

                    foreach (Tipo_Objeto item in lst_objetos)
                    {
                        if (item.Name.ToLower().Equals("\"name\"")) new_db.Name = item.Valor.ToString();
                        else if (item.Name.ToLower().Equals("\"data\"")) {

                            foreach (Tipo_Objeto item2 in (List<Tipo_Objeto>) item.Valor)
                            {
                                if (item2.Name.Equals("table")) new_db.Tablas.Add((Tabla)item2.Valor);
                            }


                        }
                    }
                    return new_db;

                case "DATABASE2":
                    if (nodo.ChildNodes.Count == 3)
                    {
                        List<Tipo_Objeto> tipo_Objetos = (List<Tipo_Objeto>) Ejecutar(nodo.ChildNodes.ElementAt(0));
                        tipo_Objetos.Add((Tipo_Objeto) Ejecutar(nodo.ChildNodes.ElementAt(2)));
                        return tipo_Objetos;
                    }
                    else
                    {
                        List<Tipo_Objeto> objetos = new List<Tipo_Objeto>
                        {
                            (Tipo_Objeto)Ejecutar(nodo.ChildNodes.ElementAt(0))
                        };

                        return objetos;
                    }

                case "DATABASE3":
                    return (Tipo_Objeto) Ejecutar(nodo.ChildNodes.ElementAt(0));

                case "DATA":
                    Tipo_Objeto objeto1 = new Tipo_Objeto(nodo.ChildNodes[0].Term.Name.ToString(), Ejecutar(nodo.ChildNodes.ElementAt(3)));
                    return objeto1;

                case "DATA5":
                    if (nodo.ChildNodes.Count == 3)
                    {
                        List<Tipo_Objeto> lst_general = (List<Tipo_Objeto>)Ejecutar(nodo.ChildNodes.ElementAt(0));
                        lst_general.Add((Tipo_Objeto) Ejecutar(nodo.ChildNodes.ElementAt(2)));
                        return lst_general;
                    }
                   /* else if (nodo.ChildNodes.Count == 5) {


                    }*/
                    else
                    {
                        List<Tipo_Objeto> lst_general = new List<Tipo_Objeto>();
                        lst_general.Add((Tipo_Objeto) Ejecutar(nodo.ChildNodes.ElementAt(0)));
                        return lst_general;
                    }

                case "DATA2":

                    Tabla tabla_aux = new Tabla();
                    Objeto objeto_aux = new Objeto();
                    Procedure procedure_aux = new Procedure();
                    string name = "", cql_type = "";
                    object columna = null , data = null , attrs = null , parametros = null;
                    List<Tipo_Objeto> lst = (List<Tipo_Objeto>)Ejecutar(nodo.ChildNodes.ElementAt(1));
                    foreach (Tipo_Objeto item in lst)
                    {
                        if (item.Name.ToLower().Equals("\"name\"")) name = item.Valor.ToString();
                        else if (item.Name.ToLower().Equals("\"cql-type\"")) cql_type = item.Valor.ToString();
                        else if (item.Name.ToLower().Equals("\"columns\"")) columna = item.Valor;
                        else if (item.Name.ToLower().Equals("\"data\"")) data = item.Valor;
                        else if (item.Name.ToLower().Equals("\"attrs\"")) attrs = item.Valor;
                        else if (item.Name.ToLower().Equals("\"parameters\"")) parametros = item.Valor;
                    }

                    if (cql_type.ToLower().Equals("table"))
                    {
                        tabla_aux = new Tabla();
                        tabla_aux.Name = name;
                        if (columna != null) tabla_aux.Columnas = (List<Columna>) columna;
                        else tabla_aux.Columnas = new List<Columna>();

                        /*try
                        {
                            List<Fila> filas = (List <Fila>) data;
                            tabla_aux.Filas = filas;
                        }
                        catch (Exception)
                        {
                            Tipo_Objeto aux2 = (Tipo_Objeto) data;
                            tabla_aux.Link = aux2.Link;
                            tabla_aux.Exportada = aux2.Export;
                            tabla_aux.Filas = (List<Fila>) aux2.Valor;
                        }*/
                        return new Tipo_Objeto("table", tabla_aux); 
                    }
                    return null;
                    /*else if (cql_type.ToLower().Equals("\"object\""))
                    {

                    }
                    else if (cql_type.ToLower().Equals("\"procedure\"")) {

                    }*/


                case "DATA3":
                    if (nodo.ChildNodes.Count == 3)
                    {
                        List<Tipo_Objeto> lst_general = (List<Tipo_Objeto>)Ejecutar(nodo.ChildNodes.ElementAt(0));
                        lst_general.Add((Tipo_Objeto) Ejecutar(nodo.ChildNodes.ElementAt(2)));
                        return lst_general;
                    }
                    else
                    {
                        List<Tipo_Objeto> lst_general = new List<Tipo_Objeto>();
                        lst_general.Add((Tipo_Objeto) Ejecutar(nodo.ChildNodes.ElementAt(0)));
                        return lst_general;
                    }

                case "DATA4":
                    return (Tipo_Objeto) Ejecutar(nodo.ChildNodes.ElementAt(0));

                case "TABLA":
                    Tipo_Objeto auxxx = (Tipo_Objeto) Ejecutar(nodo.ChildNodes.ElementAt(0));
                    return auxxx;

                case "COLUMNS":
                    return new Tipo_Objeto(nodo.ChildNodes[0].Term.Name.ToString(), Ejecutar(nodo.ChildNodes.ElementAt(3)));

                case "COLUMNS4":

                    if (nodo.ChildNodes.Count == 3)
                    {
                        List<Columna> columnas = (List<Columna>)Ejecutar(nodo.ChildNodes.ElementAt(0));
                        columnas.Add((Columna) Ejecutar(nodo.ChildNodes.ElementAt(2)));
                        return columnas;
                    }
                    else
                    {
                        List<Columna> columnas = new List<Columna>();
                        columnas.Add((Columna)Ejecutar(nodo.ChildNodes.ElementAt(0)));
                        return columnas;
                    }

                case "COLUMNS2":

                    Columna new_column = new Columna();
                    foreach (Tipo_Objeto item in (List<Tipo_Objeto>)Ejecutar(nodo.ChildNodes.ElementAt(1)))
                    {
                        if (item.Name.ToLower().Equals("\"name\"")) new_column.Name = item.Valor.ToString();
                        else if (item.Name.ToLower().Equals("\"type\"")) new_column.Type = item.Valor.ToString();
                        else if (item.Name.ToLower().Equals("\"pk\"")) {
                            if (item.Valor.ToString().ToLower().Equals("false")) new_column.Pk = false;
                            else if (item.Valor.ToString().ToLower().Equals("true")) new_column.Pk = true;
                            else {
                                //algun error
                            }
                        }
                    }
                    return new_column;

                case "COLUMNS3":
                    if (nodo.ChildNodes.Count == 3)
                    {
                        List<Tipo_Objeto> tipo_Objetos = (List<Tipo_Objeto>)Ejecutar(nodo.ChildNodes.ElementAt(0));
                        tipo_Objetos.Add((Tipo_Objeto)Ejecutar(nodo.ChildNodes.ElementAt(2)));
                        return tipo_Objetos;
                    }
                    else
                    {
                        List<Tipo_Objeto> objetos = new List<Tipo_Objeto>();
                        objetos.Add((Tipo_Objeto)Ejecutar(nodo.ChildNodes.ElementAt(0)));
                        return objetos;
                    }
                case "COLUMN":
                    return Ejecutar(nodo.ChildNodes.ElementAt(0));

                case "TYPE":
                    return new Tipo_Objeto(nodo.ChildNodes[0].Term.Name.ToString(), Ejecutar(nodo.ChildNodes.ElementAt(2)).ToString());
                case "PK":
                    return new Tipo_Objeto(nodo.ChildNodes[0].Term.Name.ToString(), Ejecutar(nodo.ChildNodes.ElementAt(2)).ToString());
                case "BOOL":
                    return nodo.ChildNodes[0].Term.Name;


                case "USERS2":
                    if (nodo.ChildNodes.Count == 3)
                    {
                        List<Usuario> usuarios = (List<Usuario>)Ejecutar(nodo.ChildNodes.ElementAt(0));
                        usuarios.Add((Usuario)Ejecutar(nodo.ChildNodes.ElementAt(2)));
                        return usuarios;
                    }
                    else if (nodo.ChildNodes.Count == 1)
                    {
                        List<Usuario> users = new List<Usuario>();
                        users.Add((Usuario)Ejecutar(nodo.ChildNodes.ElementAt(0)));
                        return users;
                    }
                    else
                    {
                        return new List<Usuario>();
                    }
                case "DATA_DATA":
                    return new Tipo_Objeto(nodo.ChildNodes[0].Term.Name.ToString(), null);


                case "USERS3":
                    Usuario new_user = new Usuario();

                    foreach (Tipo_Objeto item in (List<Tipo_Objeto>)Ejecutar(nodo.ChildNodes.ElementAt(1)))
                    {
                        if (item.Name.ToLower().Equals("\"name\"")) new_user.Name = item.Valor.ToString();
                        else if (item.Name.ToLower().Equals("\"password\"")) new_user.Password = item.Valor.ToString();
                        else if (item.Name.ToLower().Equals("\"permissions\"")) new_user.Permisos = (List<Permiso>)item.Valor;
                    }
                    return new_user;

                case "USERS4":
                    if (nodo.ChildNodes.Count == 3)
                    {
                        List<Tipo_Objeto> tipo_Objetos = (List<Tipo_Objeto>)Ejecutar(nodo.ChildNodes.ElementAt(0));
                        tipo_Objetos.Add((Tipo_Objeto)Ejecutar(nodo.ChildNodes.ElementAt(2)));
                        return tipo_Objetos;
                    }
                    else
                    {
                        List<Tipo_Objeto> objetos = new List<Tipo_Objeto>();
                        objetos.Add((Tipo_Objeto)Ejecutar(nodo.ChildNodes.ElementAt(0)));
                        return objetos;
                    }

                case "USER":
                    return Ejecutar(nodo.ChildNodes.ElementAt(0));
                case "CQL_TYPE":
                    return new Tipo_Objeto(nodo.ChildNodes[0].Term.Name.ToString(), Ejecutar(nodo.ChildNodes.ElementAt(2)).ToString());
                case "NAME":
                    Tipo_Objeto objeto = new Tipo_Objeto(nodo.ChildNodes[0].Term.Name.ToString(), Ejecutar(nodo.ChildNodes.ElementAt(2)).ToString());
                    return objeto;
                case "PASSWORD":
                    Tipo_Objeto objeto2 = new Tipo_Objeto(nodo.ChildNodes[0].Term.Name.ToString(), Ejecutar(nodo.ChildNodes.ElementAt(2)).ToString());
                    return objeto2;
                case "PERMISSIONS":
                    Tipo_Objeto objeto3 = new Tipo_Objeto(nodo.ChildNodes[0].Term.Name.ToString(), Ejecutar(nodo.ChildNodes.ElementAt(3)));
                    return objeto3;
                case "VALOR":
                    String evaluar = nodo.ChildNodes[0].Term.Name;
                    switch (evaluar)
                    {
                        case "Cadena":
                            return nodo.ChildNodes[0].ToString().Replace(" (Cadena)", "");
                        case "Identificador":
                            return nodo.ChildNodes[0].ToString().Replace(" (Identificador)", "");
                        case "Numero":
                            return nodo.ChildNodes[0].ToString().Replace(" (Numero)", "");
                        case "Decimal":
                            return nodo.ChildNodes[0].ToString().Replace(" (Decimal)", "");
                        default:
                            return "";
                    }

                case "PERMISSIONS2":
                    if (nodo.ChildNodes.Count == 3)
                    {
                        List<Permiso> permisos = (List<Permiso>)Ejecutar(nodo.ChildNodes.ElementAt(0));
                        permisos.Add((Permiso)Ejecutar(nodo.ChildNodes.ElementAt(2)));
                        return permisos;
                    }
                    else if (nodo.ChildNodes.Count == 1)
                    {
                        List<Permiso> permisos = new List<Permiso>();
                        permisos.Add((Permiso)Ejecutar(nodo.ChildNodes.ElementAt(0)));
                        return permisos;
                    }
                    else return new List<Permiso>();

                case "PERMISSION":
                    Tipo_Objeto aux = (Tipo_Objeto)Ejecutar(nodo.ChildNodes.ElementAt(1));
                    Permiso permiso = new Permiso(aux.Valor.ToString());
                    return permiso;

            }
            return null;
        }


    }
}
