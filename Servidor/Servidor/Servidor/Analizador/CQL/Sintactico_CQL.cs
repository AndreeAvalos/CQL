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

            switch (produccion)
            {
                case "CREATE_DB":
                    bool existe = false;
                    if (nodo.ChildNodes.ElementAt(0).ChildNodes.ElementAt(2).ChildNodes.Count != 0) existe = true;
                    string name = VALOR(nodo.ChildNodes.ElementAt(0).ChildNodes.ElementAt(3)).ToString();
                    return new Create_DataBase(existe, name, this.user);
                case "PUSE":
                    name = VALOR(nodo.ChildNodes.ElementAt(0).ChildNodes.ElementAt(1)).ToString();
                    return new DDL_USE(name);
                case "DROP_DB":
                    name = VALOR(nodo.ChildNodes.ElementAt(0).ChildNodes.ElementAt(2)).ToString();
                    return new Drop_DataBase(name);
                case "CREATE_TABLE":
                    existe = false; bool pk_c = false;
                    if (nodo.ChildNodes.ElementAt(0).ChildNodes.ElementAt(2).ChildNodes.Count != 0) existe = true;
                    name = VALOR(nodo.ChildNodes.ElementAt(0).ChildNodes.ElementAt(3)).ToString();
                    List<Columna> columnas = COLUMNS(nodo.ChildNodes.ElementAt(0).ChildNodes.ElementAt(5));
                    List<object> llaves_compuestas = new List<object>();
                    if (nodo.ChildNodes.ElementAt(0).ChildNodes.ElementAt(6).ChildNodes.Count != 0)
                    {
                        llaves_compuestas = PK_C(nodo.ChildNodes.ElementAt(0).ChildNodes.ElementAt(6));
                        pk_c = true;
                    }
                    return new Create_Table(name, columnas, existe, pk_c, llaves_compuestas);
                case "ALTER_TABLE":
                    string opcion = nodo.ChildNodes.ElementAt(0).ChildNodes.ElementAt(3).Term.ToString();
                    switch (opcion)
                    {
                        case "DROP":
                            List<object> columnas_eliminar = DROP_COLUMNS(nodo.ChildNodes.ElementAt(0).ChildNodes.ElementAt(4));
                            name = nodo.ChildNodes.ElementAt(0).ChildNodes.ElementAt(2).ToString().Replace(" (Identificador)", "");
                            return new ALTER_TABLE(name, false, columnas_eliminar);
                        case "ADD":
                            break;

                    }

                    return null;



            }
            return null;
        }

        private List<object> DROP_COLUMNS(ParseTreeNode nodo)
        {
            if (nodo.ChildNodes.Count == 3)
            {
                List<object> columnas_eliminar = DROP_COLUMNS(nodo.ChildNodes.ElementAt(0));
                columnas_eliminar.Add(DROP_COLUMN(nodo.ChildNodes.ElementAt(2)));
                return columnas_eliminar;
            }
            else {
                List<Object> columnas_eliminar = new List<object>();
                columnas_eliminar.Add(DROP_COLUMN(nodo.ChildNodes.ElementAt(0)));
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
                List<Columna> columnas = new List<Columna>();
                columnas.Add(COLUMN(nodo.ChildNodes.ElementAt(0)));
                return columnas;
            }
        }
        private Columna COLUMN(ParseTreeNode nodo)
        {
            Columna columna = new Columna();
            columna.Name = nodo.ChildNodes.ElementAt(0).ToString().Replace(" (Identificador)", ""); ;
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
                List<Object> valores = new List<Object>();
                valores.Add(VALOR(nodo.ChildNodes.ElementAt(0)));
                return valores;
            }
        }

        private object VALOR(ParseTreeNode node)
        {
            String evaluar = node.ChildNodes[0].Term.Name;

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
