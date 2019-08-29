using Irony.Parsing;
using Servidor.Models;
using Servidor.Models.DCL;
using Servidor.Models.FCL;
using Servidor.Models.TCL;
using Servidor.Models.USER_TYPES;
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
        bool global = true;
        bool aux_global = true;
        bool is_var = false;
        public List<string> salida = new List<string>();
        List<string> lst_ids = new List<string>();
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

            if (raiz != null && arbol.ParserMessages.Count == 0 && Program.sistema != null)
            {
                LinkedList<Instruccion> AST = Instrucciones(raiz.ChildNodes.ElementAt(0));
                TablaDeSimbolos ts_global = new TablaDeSimbolos();
                if (AST != null)
                {

                    foreach (Instruccion ins in AST)
                    {
                        ins.Recolectar(ts_global);
                    }
                    Console.WriteLine(ts_global);
                    foreach (Instruccion ins in AST)
                    {
                        ins.Ejecutar(ts_global);
                        foreach (string item in ins.getSalida())
                        {
                            salida.Add(item);
                        }
                    }
                }
            }
            else
            {
                salida = Program.lst_Errors(arbol);

            }
            return null;
        }

        private LinkedList<Instruccion> Instrucciones(ParseTreeNode nodo)
        {
            if (nodo.ChildNodes.Count == 2)
            {
                LinkedList<Instruccion> lista = Instrucciones(nodo.ChildNodes.ElementAt(0));
                lista.AddLast(Instruccion(nodo.ChildNodes.ElementAt(1)));
                return lista;

            }
            else if (nodo.ChildNodes.Count == 1)
            {
                LinkedList<Instruccion> lista = new LinkedList<Instruccion>();
                lista.AddLast(Instruccion(nodo.ChildNodes.ElementAt(0)));
                return lista;
            }
            return null;

        }

        private Instruccion Instruccion(ParseTreeNode nodo)
        {
            string produccion = nodo.ChildNodes.ElementAt(0).Term.Name;

            switch (produccion)
            {
                case "USER_TYPE":
                    return USERTYPES(nodo.ChildNodes.ElementAt(0));
                case "DDL":
                    return DDL(nodo.ChildNodes.ElementAt(0));
                case "TCL":
                    return TCL(nodo.ChildNodes.ElementAt(0));
                case "DCL":
                    return DCL(nodo.ChildNodes.ElementAt(0));
                case "FCL":
                    global = true;
                    return FCL(nodo.ChildNodes.ElementAt(0));
            }
            return null;
        }



        #region USER TYPES
        private Instruccion USERTYPES(ParseTreeNode nodo)
        {
            string produccion = nodo.ChildNodes.ElementAt(0).Term.Name;
            int linea;
            int columna;
            string name;
            switch (produccion)
            {
                case "CREATE_TYPE":
                    bool existe = false;
                    if (nodo.ChildNodes.ElementAt(0).ChildNodes.ElementAt(2).ChildNodes.Count != 0) existe = true;
                    name = nodo.ChildNodes.ElementAt(0).ChildNodes.ElementAt(3).Token.Text;
                    List<Atributo> atributos = ATRIBUTOS(nodo.ChildNodes.ElementAt(0).ChildNodes.ElementAt(5));
                    linea = nodo.ChildNodes.ElementAt(0).ChildNodes.ElementAt(0).Token.Location.Line;
                    columna = nodo.ChildNodes.ElementAt(0).ChildNodes.ElementAt(0).Token.Location.Column;
                    return new Create_Type(name, existe, atributos, linea, columna);
                case "ALTER_TYPE":
                    if (nodo.ChildNodes.ElementAt(0).ChildNodes.ElementAt(3).Token.Text.ToLower().Equals("add"))
                    {
                        name = nodo.ChildNodes.ElementAt(0).ChildNodes.ElementAt(2).Token.Text;
                        atributos = ATRIBUTOS(nodo.ChildNodes.ElementAt(0).ChildNodes.ElementAt(5));
                        linea = nodo.ChildNodes.ElementAt(0).ChildNodes.ElementAt(0).Token.Location.Line;
                        columna = nodo.ChildNodes.ElementAt(0).ChildNodes.ElementAt(0).Token.Location.Column;
                        return new Alter_Type(name, atributos, linea, columna, true);
                    }
                    else
                    {
                        name = nodo.ChildNodes.ElementAt(0).ChildNodes.ElementAt(2).Token.Text;
                        List<object> nombres = VALORES(nodo.ChildNodes.ElementAt(0).ChildNodes.ElementAt(5));
                        linea = nodo.ChildNodes.ElementAt(0).ChildNodes.ElementAt(0).Token.Location.Line;
                        columna = nodo.ChildNodes.ElementAt(0).ChildNodes.ElementAt(0).Token.Location.Column;
                        return new Alter_Type(name, nombres, linea, columna, false);
                    }
                case "DELETE_TYPE":
                    linea = nodo.ChildNodes.ElementAt(0).ChildNodes.ElementAt(0).Token.Location.Line;
                    columna = nodo.ChildNodes.ElementAt(0).ChildNodes.ElementAt(0).Token.Location.Column;
                    name = nodo.ChildNodes.ElementAt(0).ChildNodes.ElementAt(2).Token.Text;
                    return new Delete_Type(name, linea, columna);
            }
            return null;

        }

        private List<Atributo> ATRIBUTOS(ParseTreeNode nodo)
        {
            if (nodo.ChildNodes.Count == 3)
            {
                List<Atributo> atributos = ATRIBUTOS(nodo.ChildNodes.ElementAt(0));
                atributos.Add((Atributo)ATRIBUTO(nodo.ChildNodes.ElementAt(2)));
                return atributos;
            }
            else
            {
                List<Atributo> atributos = new List<Atributo>();
                atributos.Add((Atributo)ATRIBUTO(nodo.ChildNodes.ElementAt(0)));
                return atributos;
            }
        }

        private Atributo ATRIBUTO(ParseTreeNode nodo)
        {
            Atributo atributo = new Atributo
            {
                Name = nodo.ChildNodes.ElementAt(0).Token.Text,
            };


            if (nodo.ChildNodes.ElementAt(1).ChildNodes.ElementAt(0).Term.Name.Equals("SET"))
            {
                atributo.Type = "SET";
                atributo.Collection = true;
                atributo.Attr1 = TIPO_DATO(nodo.ChildNodes.ElementAt(1).ChildNodes.ElementAt(0).ChildNodes.ElementAt(2));
            }
            else if (nodo.ChildNodes.ElementAt(1).ChildNodes.ElementAt(0).Term.Name.Equals("LISTA"))
            {
                atributo.Type = "LIST";
                atributo.Collection = true;
                atributo.Attr1 = TIPO_DATO(nodo.ChildNodes.ElementAt(1).ChildNodes.ElementAt(0).ChildNodes.ElementAt(2));
            }
            else if (nodo.ChildNodes.ElementAt(1).ChildNodes.ElementAt(0).Term.Name.Equals("MAP"))
            {
                atributo.Type = "MAP";
                atributo.Collection = true;
                atributo.Attr1 = TIPO_DATO(nodo.ChildNodes.ElementAt(1).ChildNodes.ElementAt(0).ChildNodes.ElementAt(2));
                atributo.Attr2 = TIPO_DATO(nodo.ChildNodes.ElementAt(1).ChildNodes.ElementAt(0).ChildNodes.ElementAt(4));
            }
            else
            {
                atributo.Type = TIPO_DATO(nodo.ChildNodes.ElementAt(1));
            }

            return atributo;
        }

        #endregion

        #region DDL
        private Instruccion DDL(ParseTreeNode nodo)
        {
            string produccion = nodo.ChildNodes.ElementAt(0).Term.Name;
            int linea;
            int columna;
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
                    return new DDL_USE(name, linea, columna, user);
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
                Pk = false
            };
            if (nodo.ChildNodes.ElementAt(1).ChildNodes.ElementAt(0).Term.Name.Equals("SET"))
            {
                columna.Type = "SET";
                columna.Collection = true;
                columna.Attr1 = TIPO_DATO(nodo.ChildNodes.ElementAt(1).ChildNodes.ElementAt(0).ChildNodes.ElementAt(2));
            }
            else if (nodo.ChildNodes.ElementAt(1).ChildNodes.ElementAt(0).Term.Name.Equals("LISTA"))
            {
                columna.Type = "LIST";
                columna.Collection = true;
                columna.Attr1 = TIPO_DATO(nodo.ChildNodes.ElementAt(1).ChildNodes.ElementAt(0).ChildNodes.ElementAt(2));
            }
            else if (nodo.ChildNodes.ElementAt(1).ChildNodes.ElementAt(0).Term.Name.Equals("MAP"))
            {
                columna.Type = "MAP";
                columna.Collection = true;
                columna.Attr1 = TIPO_DATO(nodo.ChildNodes.ElementAt(1).ChildNodes.ElementAt(0).ChildNodes.ElementAt(2));
                columna.Attr2 = TIPO_DATO(nodo.ChildNodes.ElementAt(1).ChildNodes.ElementAt(0).ChildNodes.ElementAt(4));
            }
            else
            {
                columna.Type = TIPO_DATO(nodo.ChildNodes.ElementAt(1));
            }

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

            if (nodo.ChildNodes.ElementAt(1).ChildNodes.ElementAt(0).Term.Name.Equals("SET"))
            {
                columna.Type = "SET";
                columna.Collection = true;
                columna.Attr1 = TIPO_DATO(nodo.ChildNodes.ElementAt(1).ChildNodes.ElementAt(0).ChildNodes.ElementAt(2));
            }
            else if (nodo.ChildNodes.ElementAt(1).ChildNodes.ElementAt(0).Term.Name.Equals("LISTA"))
            {
                columna.Type = "LIST";
                columna.Collection = true;
                columna.Attr1 = TIPO_DATO(nodo.ChildNodes.ElementAt(1).ChildNodes.ElementAt(0).ChildNodes.ElementAt(2));
            }
            else if (nodo.ChildNodes.ElementAt(1).ChildNodes.ElementAt(0).Term.Name.Equals("MAP"))
            {
                columna.Type = "MAP";
                columna.Collection = true;
                columna.Attr1 = TIPO_DATO(nodo.ChildNodes.ElementAt(1).ChildNodes.ElementAt(0).ChildNodes.ElementAt(2));
                columna.Attr2 = TIPO_DATO(nodo.ChildNodes.ElementAt(1).ChildNodes.ElementAt(0).ChildNodes.ElementAt(4));
            }
            else
            {
                columna.Type = TIPO_DATO(nodo.ChildNodes.ElementAt(1));
            }

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
        #endregion

        #region TCL
        private Instruccion TCL(ParseTreeNode nodo)
        {
            if (nodo.ChildNodes.ElementAt(0).Term.Name.Equals("COMMIT"))
                return new Commit();
            else return new Rollback();
        }
        #endregion

        #region DCL

        private Instruccion DCL(ParseTreeNode nodo)
        {
            string name, password, db;
            string produccion = nodo.ChildNodes.ElementAt(0).Term.Name;
            int line;
            int column;
            switch (produccion)
            {
                case "CREATE_USER":
                    name = nodo.ChildNodes.ElementAt(0).ChildNodes.ElementAt(2).Token.Text;
                    password = nodo.ChildNodes.ElementAt(0).ChildNodes.ElementAt(5).Token.Value.ToString();
                    line = nodo.ChildNodes.ElementAt(0).Span.Location.Line;
                    column = nodo.ChildNodes.ElementAt(0).Span.Location.Column;
                    return new Create_User(line, column, name, password);
                case "GRANT":
                    name = nodo.ChildNodes.ElementAt(0).ChildNodes.ElementAt(1).Token.Text;
                    db = nodo.ChildNodes.ElementAt(0).ChildNodes.ElementAt(3).Token.Text;
                    line = nodo.ChildNodes.ElementAt(0).Span.Location.Line;
                    column = nodo.ChildNodes.ElementAt(0).Span.Location.Column;
                    return new Grant(line, column, name, db, user);
                case "REVOKE":
                    name = nodo.ChildNodes.ElementAt(0).ChildNodes.ElementAt(1).Token.Text;
                    db = nodo.ChildNodes.ElementAt(0).ChildNodes.ElementAt(3).Token.Text;
                    line = nodo.ChildNodes.ElementAt(0).Span.Location.Line;
                    column = nodo.ChildNodes.ElementAt(0).Span.Location.Column;
                    return new Revoke(line, column, name, db, user);
            }
            return null;
        }

        #endregion

        #region FCL
        private Instruccion FCL(ParseTreeNode nodo)
        {
            string name;
            string produccion = nodo.ChildNodes.ElementAt(0).Term.Name;
            int line = nodo.ChildNodes.ElementAt(0).Span.Location.Line;
            aux_global = global;
            Tipo real_type = Tipo.OBJETO;
            int column = nodo.ChildNodes.ElementAt(0).Span.Location.Column;
            switch (produccion)
            {
                case "ASIGNACION":
                    name = TIPO_DATO(nodo.ChildNodes.ElementAt(0).ChildNodes.ElementAt(0));
                    List<Variable> variables = VARIABLES(nodo.ChildNodes.ElementAt(0).ChildNodes.ElementAt(1));
                    real_type = Program.getTipo(name.ToLower());

                    return new Asignacion_Variable(real_type, name, aux_global, variables, line, column);
                case "LOG":
                    List<Valor> valores = CADENAS(nodo.ChildNodes.ElementAt(0).ChildNodes.ElementAt(2));
                    return new LOG(line, column, valores);
                case "VARIABLE":
                    Variable new_var = VARIABLE(nodo.ChildNodes.ElementAt(0));
                    return new Instancia_Variable(line, column, new_var);

            }
            return null;
        }

        private List<Valor> CADENAS(ParseTreeNode nodo)
        {
            if (nodo.ChildNodes.Count == 3)
            {
                List<Valor> valores = CADENAS(nodo.ChildNodes.ElementAt(0));
                valores.Add(CADENA(nodo.ChildNodes.ElementAt(2)));
                return valores;
            }
            else
            {
                List<Valor> valores = new List<Valor>
                {
                    CADENA(nodo.ChildNodes.ElementAt(0))
                };
                return valores;
            }
        }

        private Valor CADENA(ParseTreeNode nodo)
        {
            if (nodo.ChildNodes.Count == 2)
            {
                string name = nodo.ChildNodes.ElementAt(1).Token.Text;
                Valor new_valor = new Valor("@" + name, Tipo.VARIABLE);
                return new_valor;
            }
            else
            {
                Tipo real_type = Program.getTipo2(nodo.ChildNodes.ElementAt(0).Term.Name.ToLower());
                string val = nodo.ChildNodes.ElementAt(0).Token.Value.ToString();
                Valor new_valor = new Valor(val, real_type);
                return new_valor;
            }
        }

        private List<Variable> VARIABLES(ParseTreeNode nodo)
        {
            if (nodo.ChildNodes.Count == 3)
            {
                List<Variable> variables = VARIABLES(nodo.ChildNodes.ElementAt(0));
                variables.Add(VARIABLE(nodo.ChildNodes.ElementAt(2)));
                return variables;
            }
            else
            {
                List<Variable> variables = new List<Variable>();
                variables.Add(VARIABLE(nodo.ChildNodes.ElementAt(0)));
                return variables;
            }
        }

        private Variable VARIABLE(ParseTreeNode nodo)
        {
            Variable new_variable = new Variable();
            new_variable.Id = "@" + nodo.ChildNodes.ElementAt(1).Token.Text;
            if (nodo.ChildNodes.ElementAt(2).ChildNodes.Count == 0)
            {
                new_variable.Instanciada = false;
                new_variable.Valor = null;
            }
            else
            {
                new_variable.Instanciada = true;
                lst_ids = new List<string>();
                new_variable.Valor = INICIALIZACION(nodo.ChildNodes.ElementAt(2));
                new_variable.Lst_variables = lst_ids;
                new_variable.Is_var = is_var;
            }
            is_var = false;
            return new_variable;
        }
   


        private object INICIALIZACION(ParseTreeNode nodo)
        {
            if (nodo.ChildNodes.Count == 2)
            {
                return OPERACION_NUMERICA(nodo.ChildNodes.ElementAt(1));
            }
            return null;
        }

        private Operacion OPERACION_NUMERICA(ParseTreeNode nodo)
        {
            int line = nodo.ChildNodes.ElementAt(0).Span.Location.Line;
            int colum = nodo.ChildNodes.ElementAt(0).Span.Location.Column;
            if (nodo.ChildNodes.Count == 3)
            {
                string operador = nodo.ChildNodes.ElementAt(1).Term.Name.ToString();
                switch (operador)
                {
                    case "+":
                        return new Operacion(OPERACION_NUMERICA(nodo.ChildNodes.ElementAt(0)), OPERACION_NUMERICA(nodo.ChildNodes.ElementAt(2)), Tipo.SUMA, line, colum);
                    case "-":
                        return new Operacion(OPERACION_NUMERICA(nodo.ChildNodes.ElementAt(0)), OPERACION_NUMERICA(nodo.ChildNodes.ElementAt(2)), Tipo.RESTA, line, colum);
                    case "*":
                        return new Operacion(OPERACION_NUMERICA(nodo.ChildNodes.ElementAt(0)), OPERACION_NUMERICA(nodo.ChildNodes.ElementAt(2)), Tipo.MULTIPLICACION, line, colum);
                    case "/":
                        return new Operacion(OPERACION_NUMERICA(nodo.ChildNodes.ElementAt(0)), OPERACION_NUMERICA(nodo.ChildNodes.ElementAt(2)), Tipo.DIVISION, line, colum);
                    default:
                        return OPERACION_NUMERICA(nodo.ChildNodes.ElementAt(1));
                }
            }
            else if (nodo.ChildNodes.Count == 2)
            {
                if (nodo.ChildNodes.ElementAt(0).Token.Value.Equals("-"))
                    return new Operacion(OPERACION_NUMERICA(nodo.ChildNodes.ElementAt(1)), Tipo.NEGATIVO, line, colum);
                else
                {
                    is_var = true;
                    lst_ids.Add("@" + nodo.ChildNodes.ElementAt(1).Token.Value.ToString());
                    return new Operacion("@" + nodo.ChildNodes.ElementAt(1).Token.Value.ToString(), Tipo.VARIABLE, line, colum);
                }
            }
            else {

                return new Operacion(VALOR(nodo.ChildNodes.ElementAt(0)), line, colum);
            }
        }
        #endregion

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
