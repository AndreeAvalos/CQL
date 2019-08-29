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

            if (raiz != null && arbol.ParserMessages.Count == 0)
            {
                db_nosql = new Manejo();
                Instrucciones(raiz.ChildNodes.ElementAt(0).ChildNodes.ElementAt(2));
                return arbol.Root.ChildNodes.ElementAt(0);
            }
            else
            {

                Program.addError(arbol);
                Program.writeErrors();
                return null;
            }
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
                    break;
                case "DATABASES":
                    db_nosql.Databases = (List<Database>)Ejecutar(nodo.ChildNodes.ElementAt(0).ChildNodes.ElementAt(3));
                    break;
            }


        }

        private object Ejecutar(ParseTreeNode nodo)
        {

            string produccion = nodo.Term.Name.ToString();
            int tipo_real = 0;
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
                    List<Tipo_Objeto> lst_objetos = (List<Tipo_Objeto>)Ejecutar(nodo.ChildNodes.ElementAt(1));
                    Database new_db = new Database();

                    foreach (Tipo_Objeto item in lst_objetos)
                    {
                        if (item.Name.ToLower().Equals("\"name\"")) new_db.Name = item.Valor.ToString();
                        else if (item.Name.ToLower().Equals("\"data\""))
                        {
                            if (item.Valor != null)
                            {
                                try
                                {
                                    foreach (Tipo_Objeto item2 in (List<Tipo_Objeto>)item.Valor)
                                    {
                                        if (item2.Name.Equals("table")) new_db.Tablas.Add((Tabla)item2.Valor);
                                        else if (item2.Name.Equals("object")) new_db.Objetos.Add((Objeto)item2.Valor);
                                        else if (item2.Name.Equals("procedure")) new_db.Procedures.Add((Procedure)item2.Valor);
                                    }
                                }
                                catch (Exception)
                                {
                                    Tipo_Objeto val_aux = (Tipo_Objeto)item.Valor;
                                    new_db.Link = val_aux.Link;
                                    new_db.Exportada = val_aux.Export;

                                    //aqui exportamos los archivos de bases de datos

                                    String text = System.IO.File.ReadAllText("./NOSQL/Generados/" + new_db.Link);

                                    LanguageData lenguaje = new LanguageData(new Gramatica_Import_DATABASE());
                                    Parser parser = new Parser(lenguaje);
                                    ParseTree arbol = parser.Parse(text);
                                    ParseTreeNode raiz = arbol.Root;

                                    if (raiz != null && arbol.ParserMessages.Count == 0)
                                    {
                                        foreach (Tipo_Objeto item2 in (List<Tipo_Objeto>)Ejecutar(raiz.ChildNodes.ElementAt(0)))
                                        {
                                            if (item2.Name.Equals("table")) new_db.Tablas.Add((Tabla)item2.Valor);
                                            else if (item2.Name.Equals("object")) new_db.Objetos.Add((Objeto)item2.Valor);
                                            else if (item2.Name.Equals("procedure")) new_db.Procedures.Add((Procedure)item2.Valor);
                                        }
                                    }
                                    else
                                    {
                                        Program.addError(arbol);

                                    }
                                }
                            }
                        }
                    }
                    return new_db;

                case "DATABASE2":
                    if (nodo.ChildNodes.Count == 3)
                    {
                        List<Tipo_Objeto> tipo_Objetos = (List<Tipo_Objeto>)Ejecutar(nodo.ChildNodes.ElementAt(0));
                        tipo_Objetos.Add((Tipo_Objeto)Ejecutar(nodo.ChildNodes.ElementAt(2)));
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
                    return (Tipo_Objeto)Ejecutar(nodo.ChildNodes.ElementAt(0));

                case "DATA":
                    Tipo_Objeto objeto1 = new Tipo_Objeto(nodo.ChildNodes[0].Term.Name.ToString(), Ejecutar(nodo.ChildNodes.ElementAt(3)));
                    return objeto1;

                case "DATA5":
                    if (nodo.ChildNodes.Count == 3)
                    {
                        List<Tipo_Objeto> lst_general = (List<Tipo_Objeto>)Ejecutar(nodo.ChildNodes.ElementAt(0));
                        lst_general.Add((Tipo_Objeto)Ejecutar(nodo.ChildNodes.ElementAt(2)));
                        return lst_general;
                    }
                    else if (nodo.ChildNodes.Count == 1)
                    {
                        if (nodo.ChildNodes.ElementAt(0).Term.Name.Equals("ruta_import"))
                        {

                            Tipo_Objeto aux__ = new Tipo_Objeto("null", "null");
                            aux__.Export = true;
                            string link = nodo.ChildNodes.ElementAt(0).Token.Text.Replace("${ ", "");
                            link = link.Replace(" }$", "");
                            aux__.Link = link;
                            return aux__;
                        }

                        List<Tipo_Objeto> lst_general = new List<Tipo_Objeto>();
                        lst_general.Add((Tipo_Objeto)Ejecutar(nodo.ChildNodes.ElementAt(0)));
                        return lst_general;
                    }
                    else return null;

                case "DATA2":
                    Tabla tabla_aux = new Tabla();
                    Objeto objeto_aux = new Objeto();
                    Procedure procedure_aux = new Procedure();
                    string name = "", cql_type = "";
                    object columna = null, data = null, attrs = null, parametros = null, instr = null;
                    List<Tipo_Objeto> lst = (List<Tipo_Objeto>)Ejecutar(nodo.ChildNodes.ElementAt(1));
                    foreach (Tipo_Objeto item in lst)
                    {
                        if (item.Name.ToLower().Equals("\"name\"")) name = item.Valor.ToString();
                        else if (item.Name.ToLower().Equals("\"cql-type\"")) cql_type = item.Valor.ToString();
                        else if (item.Name.ToLower().Equals("\"columns\"")) columna = item.Valor;
                        else if (item.Name.ToLower().Equals("\"data\"")) data = item.Valor;
                        else if (item.Name.ToLower().Equals("\"attrs\"")) attrs = item.Valor;
                        else if (item.Name.ToLower().Equals("\"parameters\"")) parametros = item.Valor;
                        else if (item.Name.ToLower().Equals("\"instr\"")) instr = item.Valor;
                    }

                    if (cql_type.ToLower().Equals("table"))
                    {
                        tabla_aux = new Tabla();
                        tabla_aux.Name = name;
                        if (columna != null) tabla_aux.Columnas = (List<Columna>)columna;
                        else tabla_aux.Columnas = new List<Columna>();
                        if (data != null)
                        {
                            try
                            {
                                List<Fila> filas = (List<Fila>)data;
                                tabla_aux.Filas = filas;
                            }
                            catch (Exception)
                            {
                                Tipo_Objeto aux2 = (Tipo_Objeto)data;
                                tabla_aux.Link = aux2.Link;
                                tabla_aux.Exportada = aux2.Export;

                                String text = System.IO.File.ReadAllText("./NOSQL/Generados/" + tabla_aux.Link);

                                LanguageData lenguaje = new LanguageData(new Gramatica_Import_DATA());
                                Parser parser = new Parser(lenguaje);
                                ParseTree arbol = parser.Parse(text);
                                ParseTreeNode raiz = arbol.Root;

                                if (raiz != null && arbol.ParserMessages.Count == 0)
                                {

                                    tabla_aux.Filas = (List<Fila>)Ejecutar(raiz.ChildNodes.ElementAt(0));

                                }
                                else
                                {
                                    Program.addError(arbol);

                                }

                                //instanciamos un objeto para la base de datos no-sql



                            }
                        }
                        else tabla_aux.Filas = new List<Fila>();

                        return new Tipo_Objeto("table", tabla_aux);
                    }
                    else if (cql_type.ToLower().Equals("object"))
                    {
                        objeto_aux.Name = name;
                        if (attrs != null) objeto_aux.Atributos = (List<Atributo>)attrs;
                        else objeto_aux.Atributos = new List<Atributo>();
                        return new Tipo_Objeto("object", objeto_aux);
                    }
                    else if (cql_type.ToLower().Equals("procedure"))
                    {
                        procedure_aux.Name = name;
                        if (instr != null) procedure_aux.Instr = instr.ToString();
                        else procedure_aux.Instr = "";
                        if (parametros != null) procedure_aux.Parametros = (List<Parametro>)parametros;
                        else procedure_aux.Parametros = new List<Parametro>();

                        return new Tipo_Objeto("procedure", procedure_aux);

                    }
                    else return new Tipo_Objeto("table", tabla_aux);

                case "DATA3":
                    if (nodo.ChildNodes.Count == 3)
                    {
                        List<Tipo_Objeto> lst_general = (List<Tipo_Objeto>)Ejecutar(nodo.ChildNodes.ElementAt(0));
                        lst_general.Add((Tipo_Objeto)Ejecutar(nodo.ChildNodes.ElementAt(2)));
                        return lst_general;
                    }
                    else
                    {
                        List<Tipo_Objeto> lst_general = new List<Tipo_Objeto>();
                        lst_general.Add((Tipo_Objeto)Ejecutar(nodo.ChildNodes.ElementAt(0)));
                        return lst_general;
                    }

                case "DATA4":
                    return (Tipo_Objeto)Ejecutar(nodo.ChildNodes.ElementAt(0));

                case "TABLA":
                    return (Tipo_Objeto)Ejecutar(nodo.ChildNodes.ElementAt(0));
                case "OBJETO":
                    return (Tipo_Objeto)Ejecutar(nodo.ChildNodes.ElementAt(0));
                case "PROCEDURE":
                    return (Tipo_Objeto)Ejecutar(nodo.ChildNodes.ElementAt(0));

                case "COLUMNS":
                    if (Ejecutar(nodo.ChildNodes.ElementAt(3)) != null) return new Tipo_Objeto(nodo.ChildNodes[0].Term.Name.ToString(), Ejecutar(nodo.ChildNodes.ElementAt(3)));
                    else return new Tipo_Objeto(nodo.ChildNodes[0].Term.Name.ToString(), null);

                case "COLUMNS4":

                    if (nodo.ChildNodes.Count == 3)
                    {
                        List<Columna> columnas = (List<Columna>)Ejecutar(nodo.ChildNodes.ElementAt(0));
                        columnas.Add((Columna)Ejecutar(nodo.ChildNodes.ElementAt(2)));
                        return columnas;
                    }
                    else if (nodo.ChildNodes.Count == 1)
                    {
                        List<Columna> columnas = new List<Columna>();
                        columnas.Add((Columna)Ejecutar(nodo.ChildNodes.ElementAt(0)));
                        return columnas;
                    }
                    else return null;

                case "COLUMNS2":

                    Columna new_column = new Columna();
                    foreach (Tipo_Objeto item in (List<Tipo_Objeto>)Ejecutar(nodo.ChildNodes.ElementAt(1)))
                    {
                        if (item.Name.ToLower().Equals("\"name\"")) new_column.Name = item.Valor.ToString();
                        else if (item.Name.ToLower().Equals("\"type\""))
                        {
                            if (item.Valor.ToString().ToLower().Contains("set"))
                            {
                                new_column.Type = "SET";
                                new_column.Attr1 = item.Valor.ToString().ToLower().Replace("set<", "").Replace(">", "");

                            }
                            else if (item.Valor.ToString().ToLower().Contains("map"))
                            {
                                new_column.Type = "MAP";
                                string[] attrsa = item.Valor.ToString().ToLower().Replace("map<", "").Replace(">", "").Split(",");

                                new_column.Attr1 = attrsa.ElementAt(0);
                                new_column.Attr2 = attrsa.ElementAt(1);

                            }
                            else if (item.Valor.ToString().ToLower().Contains("list"))
                            {
                                new_column.Type = "LIST";
                                new_column.Attr1 = item.Valor.ToString().ToLower().Replace("list<", "").Replace(">", "");

                            }
                            else new_column.Type = item.Valor.ToString();
                        }
                        else if (item.Name.ToLower().Equals("\"pk\""))
                        {
                            if (item.Valor.ToString().ToLower().Equals("false")) new_column.Pk = false;
                            else if (item.Valor.ToString().ToLower().Equals("true")) new_column.Pk = true;
                            else
                            {
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
                case "AS":
                    return new Tipo_Objeto(nodo.ChildNodes[0].Term.Name.ToString(), Ejecutar(nodo.ChildNodes.ElementAt(2)).ToString());
                case "IN_OUT":
                    return nodo.ChildNodes[0].Token.Text;

                case "DATA_DATA":
                    //Aqui armamos las filas;
                    if (Ejecutar(nodo.ChildNodes.ElementAt(3)) != null) return new Tipo_Objeto(nodo.ChildNodes[0].Term.Name.ToString(), Ejecutar(nodo.ChildNodes.ElementAt(3)));
                    else return new Tipo_Objeto(nodo.ChildNodes[0].Term.Name.ToString(), null);

                case "DATA_DATA2":

                    if (nodo.ChildNodes.Count == 1)
                    {
                        if (nodo.ChildNodes.ElementAt(0).Term.Name.Equals("ruta_import"))
                        {
                            Tipo_Objeto aux__ = new Tipo_Objeto("null", "null");
                            string link = nodo.ChildNodes.ElementAt(0).Token.Text.Replace("${ ", "");
                            link = link.Replace(" }$", "");
                            aux__.Export = true;
                            aux__.Link = link;

                            return aux__;
                        }

                        return Ejecutar(nodo.ChildNodes.ElementAt(0));
                    }
                    else return null;


                case "DATA_DATA3":
                    if (nodo.ChildNodes.Count == 3)
                    {
                        List<Fila> filas = (List<Fila>)Ejecutar(nodo.ChildNodes.ElementAt(0));
                        filas.Add((Fila)Ejecutar(nodo.ChildNodes.ElementAt(2)));
                        return filas;
                    }
                    else
                    {
                        List<Fila> filas = new List<Fila>();
                        filas.Add((Fila)Ejecutar(nodo.ChildNodes.ElementAt(0)));
                        return filas;
                    }
                case "DATA_DATA4":
                    //aqui armamos la fila
                    Fila fila = new Fila();
                    fila.Valores = (List<Valor>)Ejecutar(nodo.ChildNodes.ElementAt(1));
                    return fila;

                case "DATA_DATA5":
                    if (nodo.ChildNodes.Count == 3)
                    {
                        List<Valor> valors = (List<Valor>)Ejecutar(nodo.ChildNodes.ElementAt(0));
                        valors.Add((Valor)Ejecutar(nodo.ChildNodes.ElementAt(2)));
                        return valors;
                    }
                    else
                    {
                        List<Valor> valors = new List<Valor>();
                        valors.Add((Valor)Ejecutar(nodo.ChildNodes.ElementAt(0)));
                        return valors;
                    }

                case "DATA_DATA6":
                    tipo_real = getType(nodo.ChildNodes[2].ChildNodes.ElementAt(0));
                    Valor val = new Valor(Ejecutar(nodo.ChildNodes.ElementAt(0)).ToString(), Ejecutar(nodo.ChildNodes.ElementAt(2)), tipo_real);
                    return val;

                case "ATTRIBUTES":
                    return new Tipo_Objeto(nodo.ChildNodes[0].Term.Name.ToString(), Ejecutar(nodo.ChildNodes.ElementAt(3)));
                case "ATTRIBUTES2":
                    if (nodo.ChildNodes.Count == 3)
                    {
                        List<Atributo> atributos = (List<Atributo>)Ejecutar(nodo.ChildNodes.ElementAt(0));
                        atributos.Add((Atributo)Ejecutar(nodo.ChildNodes.ElementAt(2)));
                        return atributos;
                    }
                    else if (nodo.ChildNodes.Count == 1)
                    {
                        List<Atributo> atributos = new List<Atributo>();
                        atributos.Add((Atributo)Ejecutar(nodo.ChildNodes.ElementAt(0)));
                        return atributos;
                    }
                    else return new List<Atributo>();

                case "ATTRIBUTES3":
                    Atributo atributo_new = new Atributo();

                    foreach (Tipo_Objeto item in (List<Tipo_Objeto>)Ejecutar(nodo.ChildNodes.ElementAt(1)))
                    {
                        if (item.Name.ToLower().Equals("\"name\"")) atributo_new.Name = item.Valor.ToString();
                        else if (item.Name.ToLower().Equals("\"type\""))
                        {

                            if (item.Valor.ToString().ToLower().Contains("set"))
                            {
                                atributo_new.Type = "SET";
                                atributo_new.Attr1 = item.Valor.ToString().ToLower().Replace("set<", "").Replace(">", "");

                            }
                            else if (item.Valor.ToString().ToLower().Contains("map"))
                            {
                                atributo_new.Type = "MAP";
                                string[] attrsa = item.Valor.ToString().ToLower().Replace("map<", "").Replace(">", "").Split(",");

                                atributo_new.Attr1 = attrsa.ElementAt(0);
                                atributo_new.Attr2 = attrsa.ElementAt(1);

                            }
                            else if (item.Valor.ToString().ToLower().Contains("list"))
                            {
                                atributo_new.Type = "LIST";
                                atributo_new.Attr1 = item.Valor.ToString().ToLower().Replace("list<", "").Replace(">", "");

                            }
                            else atributo_new.Type = item.Valor.ToString();
                        }
                        else
                        {
                            //algun error
                        }
                    }
                    return atributo_new;

                case "ATTRIBUTES4":
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

                case "ATTRIBUTE":
                    return Ejecutar(nodo.ChildNodes.ElementAt(0));
                case "INSTR":
                    return new Tipo_Objeto(nodo.ChildNodes[0].Term.Name.ToString(), nodo.ChildNodes.ElementAt(2).Token.Text);
                case "PARAMETERS":
                    return new Tipo_Objeto(nodo.ChildNodes[0].Term.Name.ToString(), Ejecutar(nodo.ChildNodes.ElementAt(3)));
                case "PARAMETERS2":
                    if (nodo.ChildNodes.Count == 3)
                    {
                        List<Parametro> param = (List<Parametro>)Ejecutar(nodo.ChildNodes.ElementAt(0));
                        param.Add((Parametro)Ejecutar(nodo.ChildNodes.ElementAt(2)));
                        return param;
                    }
                    else if (nodo.ChildNodes.Count == 1)
                    {
                        List<Parametro> param = new List<Parametro>();
                        param.Add((Parametro)Ejecutar(nodo.ChildNodes.ElementAt(0)));
                        return param;
                    }
                    else return new List<Parametro>();
                case "PARAMETERS3":
                    Parametro new_param = new Parametro();

                    foreach (Tipo_Objeto item in (List<Tipo_Objeto>)Ejecutar(nodo.ChildNodes.ElementAt(1)))
                    {
                        if (item.Name.ToLower().Equals("\"name\"")) new_param.Name = item.Valor.ToString();
                        else if (item.Name.ToLower().Equals("\"type\""))
                        {

                            if (item.Valor.ToString().ToLower().Contains("set"))
                            {
                                new_param.Type = "SET";
                                new_param.Attr1 = item.Valor.ToString().ToLower().Replace("set<", "").Replace(">", "");

                            }
                            else if (item.Valor.ToString().ToLower().Contains("map"))
                            {
                                new_param.Type = "MAP";
                                string[] attrsa = item.Valor.ToString().ToLower().Replace("map<", "").Replace(">", "").Split(",");

                                new_param.Attr1 = attrsa.ElementAt(0);
                                new_param.Attr2 = attrsa.ElementAt(1);

                            }
                            else if (item.Valor.ToString().ToLower().Contains("list"))
                            {
                                new_param.Type = "LIST";
                                new_param.Attr1 = item.Valor.ToString().ToLower().Replace("list<", "").Replace(">", "");

                            }
                            else new_param.Type = item.Valor.ToString();
                        }
                        else if (item.Name.ToLower().Equals("\"as\""))
                        {
                            if (item.Valor.ToString().ToLower().Equals("in"))
                            {
                                new_param.Out_ = false; new_param.Out__ = item.Valor.ToString();
                            }
                            else
                            {
                                new_param.Out_ = true; new_param.Out__ = item.Valor.ToString();
                            }
                        }
                        else
                        {
                            //algun error
                        }
                    }
                    return new_param;

                case "PARAMETERS4":
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

                case "PARAMETER":
                    return Ejecutar(nodo.ChildNodes.ElementAt(0));
                case "LISTAS":
                    return Ejecutar(nodo.ChildNodes.ElementAt(1));
                case "MAPA":
                    return Ejecutar(nodo.ChildNodes.ElementAt(1));
                case "MAPA2":
                    if (nodo.ChildNodes.Count == 3)
                    {
                        List<Tipo_Objeto> valores = (List<Tipo_Objeto>)Ejecutar(nodo.ChildNodes.ElementAt(0));
                        valores.Add((Tipo_Objeto)Ejecutar(nodo.ChildNodes.ElementAt(2)));
                        return valores;
                    }
                    else if (nodo.ChildNodes.Count == 1)
                    {
                        List<Tipo_Objeto> valores = new List<Tipo_Objeto>();
                        valores.Add((Tipo_Objeto)Ejecutar(nodo.ChildNodes.ElementAt(0)));
                        return valores;
                    }
                    else
                    {
                        return new List<Tipo_Objeto>();
                    }
                case "MAPA3":
                    tipo_real = getType(nodo.ChildNodes[2].ChildNodes.ElementAt(0));
                    Tipo_Objeto ret = new Tipo_Objeto(Ejecutar(nodo.ChildNodes.ElementAt(0)).ToString(), Ejecutar(nodo.ChildNodes.ElementAt(2)));
                    ret.Type = tipo_real;
                    return ret;

                case "LISTAS2":
                    if (nodo.ChildNodes.Count == 3)
                    {
                        List<Item_List> valores = (List<Item_List>)Ejecutar(nodo.ChildNodes.ElementAt(0));
                        valores.Add((Item_List)Ejecutar(nodo.ChildNodes.ElementAt(2)));
                        return valores;
                    }
                    else if (nodo.ChildNodes.Count == 1)
                    {
                        List<Item_List> valores = new List<Item_List>();
                        valores.Add((Item_List)Ejecutar(nodo.ChildNodes.ElementAt(0)));
                        return valores;
                    }
                    else
                    {
                        return new List<Item_List>();
                    }
                case "LISTAS3":
                    tipo_real = getType(nodo.ChildNodes[0].ChildNodes.ElementAt(0));

                    return new Item_List(tipo_real, Ejecutar(nodo.ChildNodes.ElementAt(0)));


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
                    return new Tipo_Objeto(nodo.ChildNodes[0].Term.Name.ToString(), Ejecutar(nodo.ChildNodes.ElementAt(2)).ToString());

                case "PASSWORD":
                    return new Tipo_Objeto(nodo.ChildNodes[0].Term.Name.ToString(), Ejecutar(nodo.ChildNodes.ElementAt(2)).ToString());

                case "PERMISSIONS":
                    return new Tipo_Objeto(nodo.ChildNodes[0].Term.Name.ToString(), Ejecutar(nodo.ChildNodes.ElementAt(3)));

                case "VALOR":
                    String evaluar = nodo.ChildNodes[0].Term.Name;
                    switch (evaluar)
                    {
                        case "Cadena":
                            return nodo.ChildNodes[0].ToString().Replace(" (Cadena)", "");
                        case "Identificador":
                            return nodo.ChildNodes[0].ToString().Replace(" (Identificador)", "");
                        case "Numero":
                            return nodo.ChildNodes[0].Token.Text;
                        case "NULL":
                            return null;
                        case "Time":
                            return nodo.ChildNodes[0].Token.Text;
                        case "FALSE":
                            return false;
                        case "TRUE":
                            return true;
                        case "Date":
                            return nodo.ChildNodes[0].Token.Text;
                        case "LISTAS":
                            return Ejecutar(nodo.ChildNodes.ElementAt(0));
                        case "MAPA":
                            return Ejecutar(nodo.ChildNodes.ElementAt(0));
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
        private int getType(ParseTreeNode nodo)
        {
            string opcion = nodo.Term.Name;
            switch (opcion)
            {
                case "Cadena":
                    return 7;
                case "Numero":
                    try
                    {
                        Convert.ToInt32(nodo.Token.Text);
                        return 3;

                    }
                    catch (Exception)
                    {

                        Convert.ToDouble(nodo.Token.Text);
                        return 4;
                    }

                case "LISTAS":
                    return 2;
                case "MAPA":
                    return 1;

                case "TRUE":
                    return 5;

                case "FALSE":
                    return 5;

                case "Date":
                    return 6;
                case "Time":
                    return 8;
                case "NULL":
                    return 9;
                default:
                    return -1;
            }
        }

    }
}
