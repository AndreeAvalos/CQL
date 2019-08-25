using Irony.Parsing;
using Servidor.Models;
using Servidor.NOSQL.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Servidor.Analizador.CQL
{
    public class Sintactico_CQL
    {
        string user;
        public List<string> salida = new List<string>();
        public bool Validar(String entrada, Grammar gramatica)
        {
            LanguageData lenguaje = new LanguageData(gramatica);
            Parser parser = new Parser(lenguaje);
            ParseTree arbol = parser.Parse(entrada);
            if (arbol.Root != null) return true;
            else return false;
        }
        public ParseTreeNode Analizar(String entrada, Grammar gramatica, string user)
        {
            this.user = user;
            LanguageData lenguaje = new LanguageData(gramatica);
            Parser parser = new Parser(lenguaje);
            ParseTree arbol = parser.Parse(entrada);
            ParseTreeNode raiz = arbol.Root;

            LinkedList<Instruccion> AST = Instrucciones(raiz.ChildNodes.ElementAt(0));
            TablaDeSimbolos global = new TablaDeSimbolos();

            foreach (Instruccion ins in AST)
            {
                ins.Ejecutar(global);
                foreach (string item in ins.getSalida())
                {
                    salida.Add(item);
                }
            }
            return arbol.Root.ChildNodes.ElementAt(0);
        }

        private LinkedList<Instruccion> Instrucciones(ParseTreeNode nodo)
        {
            if (nodo.ChildNodes.Count == 2)
            {
                LinkedList<Instruccion> lista = Instrucciones(nodo.ChildNodes.ElementAt(0));
                lista.AddLast(Instruccion(nodo.ChildNodes.ElementAt(1)));
                return lista;

            }
            else
            {
                LinkedList<Instruccion> lista = new LinkedList<Instruccion>();
                lista.AddLast(Instruccion(nodo.ChildNodes.ElementAt(0)));
                return lista;
            }

        }

        private Instruccion Instruccion(ParseTreeNode nodo)
        {
            string produccion = nodo.ChildNodes.ElementAt(0).Term.Name;

            switch (produccion)
            {
                case "DDL":
                    return DDL(nodo.ChildNodes.ElementAt(0));
            }
            return null;
        }

        private Instruccion DDL(ParseTreeNode nodo)
        {
            string produccion = nodo.ChildNodes.ElementAt(0).Term.Name;
            int linea = 0, columna = 0;
            switch (produccion)
            {
                case "CREATE_DB":
                    bool existe = false;
                    if (nodo.ChildNodes.ElementAt(0).ChildNodes.ElementAt(2).ChildNodes.Count != 0) existe = true;
                    string name = VALOR(nodo.ChildNodes.ElementAt(0).ChildNodes.ElementAt(3)).ToString();
                    linea = nodo.ChildNodes.ElementAt(0).ChildNodes.ElementAt(0).Token.Location.Line;
                    columna = nodo.ChildNodes.ElementAt(0).ChildNodes.ElementAt(0).Token.Location.Column;
                    return new Create_DataBase(existe, name, this.user, linea, columna);
                case "PUSE":
                    name = VALOR(nodo.ChildNodes.ElementAt(0).ChildNodes.ElementAt(1)).ToString();
                    linea = nodo.ChildNodes.ElementAt(0).ChildNodes.ElementAt(0).Token.Location.Line;
                    columna = nodo.ChildNodes.ElementAt(0).ChildNodes.ElementAt(0).Token.Location.Column;
                    return new DDL_USE(name, linea, columna);
                case "DROP_DB":
                    name = VALOR(nodo.ChildNodes.ElementAt(0).ChildNodes.ElementAt(2)).ToString();
                    linea = nodo.ChildNodes.ElementAt(0).ChildNodes.ElementAt(0).Token.Location.Line;
                    columna = nodo.ChildNodes.ElementAt(0).ChildNodes.ElementAt(0).Token.Location.Column;
                    return new Drop_DataBase(name, linea, columna);
                case "CREATE_TABLE":
                    existe = false; bool pk_c = false;
                    if (nodo.ChildNodes.ElementAt(0).ChildNodes.ElementAt(2).ChildNodes.Count != 0) existe = true;
                    name = VALOR(nodo.ChildNodes.ElementAt(0).ChildNodes.ElementAt(3)).ToString();
                    List<Columna> columnas = COLUMNS(nodo.ChildNodes.ElementAt(0).ChildNodes.ElementAt(5));
                    List<object> llaves_compuestas = new List<object>();
                    linea = nodo.ChildNodes.ElementAt(0).ChildNodes.ElementAt(0).Token.Location.Line;
                    columna = nodo.ChildNodes.ElementAt(0).ChildNodes.ElementAt(0).Token.Location.Column;
                    if (nodo.ChildNodes.ElementAt(0).ChildNodes.ElementAt(6).ChildNodes.Count != 0)
                    {
                        llaves_compuestas = PK_C(nodo.ChildNodes.ElementAt(0).ChildNodes.ElementAt(6));
                        pk_c = true;
                    }
                    return new Create_Table(name, columnas, existe, pk_c, llaves_compuestas, linea, columna);
                case "ALTER_TABLE":
                    string opcion = nodo.ChildNodes.ElementAt(0).ChildNodes.ElementAt(3).Term.ToString();
                    name = nodo.ChildNodes.ElementAt(0).ChildNodes.ElementAt(2).ToString().Replace(" (Identificador)", "");
                    linea = nodo.ChildNodes.ElementAt(0).ChildNodes.ElementAt(0).Token.Location.Line;
                    columna = nodo.ChildNodes.ElementAt(0).ChildNodes.ElementAt(0).Token.Location.Column;
                    switch (opcion)
                    {

                        case "DROP":
                            List<object> columnas_eliminar = DROP_COLUMNS(nodo.ChildNodes.ElementAt(0).ChildNodes.ElementAt(4));
                            return new Alter_Table(name, false, columnas_eliminar, linea, columna);
                        case "ADD":
                            List<Columna> columnas_agregar = ADD_COLUMNS(nodo.ChildNodes.ElementAt(0).ChildNodes.ElementAt(4));
                            return new Alter_Table(name, true, columnas_agregar, linea, columna);

                    }
                    return null;
                case "DROP_TABLE":
                    bool ife = false;
                    if (nodo.ChildNodes.ElementAt(0).ChildNodes.ElementAt(2).ChildNodes.Count != 0) ife = true;
                    linea = nodo.ChildNodes.ElementAt(0).ChildNodes.ElementAt(0).Token.Location.Line;
                    columna = nodo.ChildNodes.ElementAt(0).ChildNodes.ElementAt(0).Token.Location.Column;
                    name = nodo.ChildNodes.ElementAt(0).ChildNodes.ElementAt(3).ToString().Replace(" (Identificador)", "");
                    return new Drop_Table(name, ife, linea, columna);
                case "TRUNCATE_TABLE":
                    name = nodo.ChildNodes.ElementAt(0).ChildNodes.ElementAt(2).ToString().Replace(" (Identificador)", "");
                    linea = nodo.ChildNodes.ElementAt(0).ChildNodes.ElementAt(0).Token.Location.Line;
                    columna = nodo.ChildNodes.ElementAt(0).ChildNodes.ElementAt(0).Token.Location.Column;
                    return new Truncate_Table(name, linea, columna);


            }
            return null;
        }

        private List<Columna> ADD_COLUMNS(ParseTreeNode nodo)
        {
            if (nodo.ChildNodes.Count == 3)
            {
                List<Columna> columnas_agregar = ADD_COLUMNS(nodo.ChildNodes.ElementAt(0));
                columnas_agregar.Add(ADD_COLUMN(nodo.ChildNodes.ElementAt(2)));
                return columnas_agregar;
            }
            else
            {
                List<Columna> columnas_agregar = new List<Columna>
                {
                    ADD_COLUMN(nodo.ChildNodes.ElementAt(0))
                };
                return columnas_agregar;
            }
        }

        private Columna ADD_COLUMN(ParseTreeNode nodo)
        {
            Columna columna = new Columna
            {
                Name = nodo.ChildNodes.ElementAt(0).ToString().Replace(" (Identificador)", ""),
                Type = TIPO_DATO(nodo.ChildNodes.ElementAt(1)),
                Pk = false
            };

            return columna;
        }

        private List<object> DROP_COLUMNS(ParseTreeNode nodo)
        {
            if (nodo.ChildNodes.Count == 3)
            {
                List<object> columnas_eliminar = DROP_COLUMNS(nodo.ChildNodes.ElementAt(0));
                columnas_eliminar.Add(DROP_COLUMN(nodo.ChildNodes.ElementAt(2)));
                return columnas_eliminar;
            }
            else
            {
                List<Object> columnas_eliminar = new List<object>
                {
                    DROP_COLUMN(nodo.ChildNodes.ElementAt(0))
                };
                return columnas_eliminar;
            }

        }

        private object DROP_COLUMN(ParseTreeNode nodo)
        {
            return nodo.ChildNodes.ElementAt(0).ToString().Replace(" (Identificador)", ""); ;
        }

        private List<Columna> COLUMNS(ParseTreeNode nodo)
        {
            if (nodo.ChildNodes.Count == 3)
            {
                List<Columna> columnas = COLUMNS(nodo.ChildNodes.ElementAt(0));
                columnas.Add(COLUMN(nodo.ChildNodes.ElementAt(2)));
                return columnas;
            }
            else
            {
                List<Columna> columnas = new List<Columna>
                {
                    COLUMN(nodo.ChildNodes.ElementAt(0))
                };
                return columnas;
            }
        }
        private Columna COLUMN(ParseTreeNode nodo)
        {
            Columna columna = new Columna
            {
                Name = nodo.ChildNodes.ElementAt(0).ToString().Replace(" (Identificador)", "")
            };
            ;
            columna.Type = TIPO_DATO(nodo.ChildNodes.ElementAt(1));
            if (nodo.ChildNodes.ElementAt(2).ChildNodes.Count != 0) columna.Pk = true;
            return columna;
        }

        private List<object> PK_C(ParseTreeNode nodo)
        {

            List<object> nombres = VALORES(nodo.ChildNodes.ElementAt(3));
            return nombres;

        }

        private string TIPO_DATO(ParseTreeNode nodo)
        {

            if (nodo.ChildNodes.ElementAt(0).Term.Name.Equals("Identificador")) return nodo.ChildNodes[0].ToString().Replace(" (Identificador)", "").ToString();
            else return nodo.ChildNodes.ElementAt(0).Term.Name.ToString();
        }

        private List<Object> VALORES(ParseTreeNode nodo)
        {
            if (nodo.ChildNodes.Count == 3)
            {
                List<Object> valores = VALORES(nodo.ChildNodes.ElementAt(0));
                valores.Add(VALOR(nodo.ChildNodes.ElementAt(2)));
                return valores;
            }
            else
            {
                List<Object> valores = new List<Object>
                {
                    VALOR(nodo.ChildNodes.ElementAt(0))
                };
                return valores;
            }
        }

        private object VALOR(ParseTreeNode node)
        {
            string evaluar = node.ChildNodes[0].Term.Name;

            switch (evaluar)
            {
                //agregar decimal, date, time, etc
                case "Cadena":
                    return node.ChildNodes[0].ToString().Replace(" (Cadena)", "");
                case "Identificador":
                    return node.ChildNodes[0].ToString().Replace(" (Identificador)", "");
                case "Numero":
                    return Convert.ToInt32(node.ChildNodes[0].ToString().Replace(" (Numero)", ""));
                default:
                    return "";
            }
        }
    }
}
