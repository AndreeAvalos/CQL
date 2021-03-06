﻿using Irony.Parsing;
using Servidor.Analizador.CHISON;
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
        string clave = "", valor = "";
        public List<string> salida = new List<string>();
        List<string> lst_ids = new List<string>();
        bool concatenacion = false;
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

                    foreach (Instruccion ins in AST)
                    {
                        if (ins.getType() == Tipo.BREAK) salida.Add(Program.buildError(ins.getLine(), ins.getColumn(), "Semantico", "Break no puede existir en el ambito global"));
                        ins.Ejecutar(ts_global);
                        salida.AddRange(ins.getSalida());
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
            return new LinkedList<Instruccion>();

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
            int column = nodo.ChildNodes.ElementAt(0).Span.Location.Column;
            aux_global = global;
            Tipo real_type = Tipo.OBJETO;

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
                    if (nodo.ChildNodes.ElementAt(0).ChildNodes.Count == 5)
                    {
                        string opcion = nodo.ChildNodes.ElementAt(0).ChildNodes.ElementAt(2).Term.Name.ToString();
                        name = nodo.ChildNodes.ElementAt(0).ChildNodes.ElementAt(1).Token.Value.ToString();
                        switch (opcion)
                        {
                            case "+":
                                return new Operacion("@" + name, new Operacion(nodo.ChildNodes.ElementAt(0).ChildNodes.ElementAt(4).Token.Value.ToString(), line, column), Tipo.MASIGUAL, line, column);
                            case "-":
                                return new Operacion("@" + name, new Operacion(nodo.ChildNodes.ElementAt(0).ChildNodes.ElementAt(4).Token.Value.ToString(), line, column), Tipo.MENOSIGUAL, line, column);
                            case "*":
                                return new Operacion("@" + name, new Operacion(nodo.ChildNodes.ElementAt(0).ChildNodes.ElementAt(4).Token.Value.ToString(), line, column), Tipo.MULIGUAL, line, column);
                            case "/":
                                return new Operacion("@" + name, new Operacion(nodo.ChildNodes.ElementAt(0).ChildNodes.ElementAt(4).Token.Value.ToString(), line, column), Tipo.DIVIGUAL, line, column);
                            default:
                                return null;
                        }
                    }
                    else
                    {
                        if (nodo.ChildNodes.ElementAt(0).ChildNodes.ElementAt(2).Term.Name.Equals("INICIALIZACION"))
                        {
                            Variable new_var = VARIABLE(nodo.ChildNodes.ElementAt(0));
                            new_var.Real_type = Tipo.VARIABLE;
                            return new Instancia_Variable(line, column, new_var);
                        }
                        else if (nodo.ChildNodes.ElementAt(0).ChildNodes.ElementAt(2).Term.Name.Equals("VAR_ATTRS"))
                        {
                            Variable new_var = VARIABLE(nodo.ChildNodes.ElementAt(0));
                            new_var.Real_type = Tipo.VARIABLE_ATRIBUTOS;
                            return new Instancia_Variable(line, column, new_var);
                        }
                        else if (nodo.ChildNodes.ElementAt(0).ChildNodes.ElementAt(3).Term.Name.Equals("METODOS_MAP"))
                        {
                            Variable new_var = new Variable();
                            new_var.Valor = METODOS(nodo.ChildNodes.ElementAt(0).ChildNodes.ElementAt(3));
                            new_var.Id = "@" + nodo.ChildNodes.ElementAt(0).ChildNodes.ElementAt(1).Token.Value.ToString();
                            return new Operacion(new_var, Tipo.VARIABLE_METODOS, line, column);
                        }
                        else if (nodo.ChildNodes.ElementAt(0).ChildNodes.ElementAt(2).Term.Name.Equals("++"))
                        {
                            name = nodo.ChildNodes.ElementAt(0).ChildNodes.ElementAt(1).Token.Value.ToString();
                            return new Operacion("@" + name, Tipo.INCREMENTO, line, column);
                        }
                        else
                        {
                            name = nodo.ChildNodes.ElementAt(0).ChildNodes.ElementAt(1).Token.Value.ToString();
                            return new Operacion("@" + name, Tipo.DECREMENTO, line, column);
                        }
                    }
                case "SENTENCIA_IF":

                    global = false;
                    Tipo_IF sentencia_if = new Tipo_IF
                    {
                        Condicion = VALORES_LOGICOS(nodo.ChildNodes.ElementAt(0).ChildNodes.ElementAt(2)),
                        If_instrucciones = Instrucciones(nodo.ChildNodes.ElementAt(0).ChildNodes.ElementAt(5)),
                        //obtnemos todas las sentencias else if
                        Else_if = SENTENCIA_ELSE_IF2(nodo.ChildNodes.ElementAt(0).ChildNodes.ElementAt(7)),
                        Else_instrucciones = SENTENCIA_ELSE(nodo.ChildNodes.ElementAt(0).ChildNodes.ElementAt(8))
                    };
                    global = true;
                    return new Sentencia_If(sentencia_if, line, column);
                case "BREAK":
                    return new Sentencia_Break(line, column);
                case "SENTENCIA_SWITCH":
                    Tipo_Switch tipo_Switch = TIPO_SWITCH(nodo.ChildNodes.ElementAt(0));
                    return new Sentencia_Switch(tipo_Switch, line, column);
                case "SENTENCIA_WHILE":
                    return SENTENCIA_WHILE(nodo.ChildNodes.ElementAt(0));
                case "SENTENCIA_DO_WHILE":
                    return SENTENCIA_DO_WHILE(nodo.ChildNodes.ElementAt(0));
                case "SENTENCIA_FOR":
                    return SENTENCIA_FOR(nodo.ChildNodes.ElementAt(0));
            }
            return null;
        }


        private Instruccion SENTENCIA_FOR(ParseTreeNode nodo)
        {
            global = true;
            int line = nodo.ChildNodes.ElementAt(0).Span.Location.Line;
            int column = nodo.ChildNodes.ElementAt(0).Span.Location.Column;
            Variable var = INICIALIZAR(nodo.ChildNodes.ElementAt(2));
            Operacion expresion = OPERACION_LOGICA(nodo.ChildNodes.ElementAt(4));
            Operacion actualizacion = ACTUALIZACION(nodo.ChildNodes.ElementAt(6));
            LinkedList<Instruccion> instruccions = Instrucciones(nodo.ChildNodes.ElementAt(9));
            global = false;
            return new Sentencia_For(var, expresion, actualizacion, instruccions, line, column);

        }

        private Operacion ACTUALIZACION(ParseTreeNode nodo)
        {
            int line = nodo.ChildNodes.ElementAt(0).Span.Location.Line;
            int column = nodo.ChildNodes.ElementAt(0).Span.Location.Column;
            string operador = nodo.ChildNodes.ElementAt(2).Term.Name.ToString();
            if (operador.Equals("++")) return new Operacion("@" + nodo.ChildNodes.ElementAt(1).Token.Value, Tipo.INCREMENTO, line, column);
            else return new Operacion("@" + nodo.ChildNodes.ElementAt(1).Token.Value, Tipo.DECREMENTO, line, column);
        }

        private Variable INICIALIZAR(ParseTreeNode nodo)
        {
            Variable new_var = new Variable();
            if (nodo.ChildNodes.Count == 5)
            {
                new_var.Tipo = TIPO_DATO(nodo.ChildNodes.ElementAt(0));
                new_var.Real_type = Tipo.VARIABLE;
                new_var.Asignacion = false;
                new_var.Id = "@" + nodo.ChildNodes.ElementAt(2).Token.Value.ToString();
                new_var.Valor = OPERACION_NUMERICA(nodo.ChildNodes.ElementAt(4));
            }
            else
            {
                new_var.Real_type = Tipo.VARIABLE;
                new_var.Asignacion = true;
                new_var.Id = "@" + nodo.ChildNodes.ElementAt(1).Token.Value.ToString();
                new_var.Valor = OPERACION_NUMERICA(nodo.ChildNodes.ElementAt(3));
            }
            return new_var;
        }

        private Instruccion SENTENCIA_DO_WHILE(ParseTreeNode nodo)
        {
            global = false;
            int line = nodo.ChildNodes.ElementAt(0).Span.Location.Line;
            int column = nodo.ChildNodes.ElementAt(0).Span.Location.Column;
            Operacion expresion = VALORES_LOGICOS(nodo.ChildNodes.ElementAt(6));
            LinkedList<Instruccion> instruccions = Instrucciones(nodo.ChildNodes.ElementAt(2));
            global = true;
            return new Sentencia_Do_While(expresion, instruccions, line, column);
        }

        private Instruccion SENTENCIA_WHILE(ParseTreeNode nodo)
        {
            global = false;
            int line = nodo.ChildNodes.ElementAt(0).Span.Location.Line;
            int column = nodo.ChildNodes.ElementAt(0).Span.Location.Column;
            Operacion expresion = VALORES_LOGICOS(nodo.ChildNodes.ElementAt(2));
            LinkedList<Instruccion> instruccions = Instrucciones(nodo.ChildNodes.ElementAt(5));
            global = true;
            return new Sentencia_While(expresion, instruccions, line, column);
        }

        private Tipo_Switch TIPO_SWITCH(ParseTreeNode nodo)
        {
            global = false;
            Operacion expresion = VALORES_LOGICOS(nodo.ChildNodes.ElementAt(2));
            List<Tipo_Case> casos = CASOS(nodo.ChildNodes.ElementAt(5));
            LinkedList<Instruccion> instrucions_default;
            if (nodo.ChildNodes.ElementAt(6).ChildNodes.Count != 0) instrucions_default = SWITCH_DEFAULT(nodo.ChildNodes.ElementAt(6));
            else instrucions_default = new LinkedList<Instruccion>();
            global = true;
            return new Tipo_Switch(expresion, casos, instrucions_default);
        }

        private LinkedList<Instruccion> SWITCH_DEFAULT(ParseTreeNode nodo)
        {
            return Instrucciones(nodo.ChildNodes.ElementAt(2));
        }

        private List<Tipo_Case> CASOS(ParseTreeNode nodo)
        {
            if (nodo.ChildNodes.Count == 2)
            {
                List<Tipo_Case> casos = CASOS(nodo.ChildNodes.ElementAt(0));
                casos.Add(CASO(nodo.ChildNodes.ElementAt(1)));
                return casos;
            }
            else
            {
                List<Tipo_Case> casos = new List<Tipo_Case>
                {
                    CASO(nodo.ChildNodes.ElementAt(0))
                };
                return casos;
            }
        }

        private Tipo_Case CASO(ParseTreeNode nodo)
        {
            int line = nodo.ChildNodes.ElementAt(0).Span.Location.Line;
            int column = nodo.ChildNodes.ElementAt(0).Span.Location.Column;
            string expresion = VALOR(nodo.ChildNodes.ElementAt(1)).ToString();
            LinkedList<Instruccion> lst = Instrucciones(nodo.ChildNodes.ElementAt(3));

            return new Tipo_Case(expresion, lst, line, column);
        }

        private LinkedList<Instruccion> SENTENCIA_ELSE(ParseTreeNode nodo)
        {
            if (nodo.ChildNodes.Count != 0)
            {
                return Instrucciones(nodo.ChildNodes.ElementAt(2));
            }
            else return new LinkedList<Instruccion>();
        }

        private List<Tipo_IF> SENTENCIA_ELSE_IF2(ParseTreeNode nodo)
        {
            if (nodo.ChildNodes.Count == 3)
            {
                List<Tipo_IF> else_if = SENTENCIA_ELSE_IF2(nodo.ChildNodes.ElementAt(0));
                else_if.Add(SENTENCIA_ELSE_IF(nodo.ChildNodes.ElementAt(1)));
                return else_if;
            }
            else if (nodo.ChildNodes.Count == 1)
            {
                List<Tipo_IF> else_if = new List<Tipo_IF>
                {
                    SENTENCIA_ELSE_IF(nodo.ChildNodes.ElementAt(0))
                };
                return else_if;
            }
            else return new List<Tipo_IF>();
        }

        private Tipo_IF SENTENCIA_ELSE_IF(ParseTreeNode nodo)
        {
            Tipo_IF sentencia_if = new Tipo_IF
            {
                Condicion = VALORES_LOGICOS(nodo.ChildNodes.ElementAt(2)),
                If_instrucciones = Instrucciones(nodo.ChildNodes.ElementAt(5))
            };

            return sentencia_if;

        }

        private Operacion VALORES_LOGICOS(ParseTreeNode nodo)
        {
            int line = nodo.ChildNodes.ElementAt(0).Span.Location.Line;
            int column = nodo.ChildNodes.ElementAt(0).Span.Location.Column;
            if (nodo.ChildNodes.Count == 3)
            {
                string operador = nodo.ChildNodes.ElementAt(1).Term.Name.ToString();
                switch (operador)
                {
                    case "||":
                        return new Operacion(VALORES_LOGICOS(nodo.ChildNodes.ElementAt(0)), VALORES_LOGICOS(nodo.ChildNodes.ElementAt(2)), Tipo.OR, line, column);
                    case "&&":
                        return new Operacion(VALORES_LOGICOS(nodo.ChildNodes.ElementAt(0)), VALORES_LOGICOS(nodo.ChildNodes.ElementAt(2)), Tipo.AND, line, column);
                    case "^":
                        return new Operacion(VALORES_LOGICOS(nodo.ChildNodes.ElementAt(0)), VALORES_LOGICOS(nodo.ChildNodes.ElementAt(2)), Tipo.XOR, line, column);
                    default:
                        return VALORES_LOGICOS(nodo.ChildNodes.ElementAt(1));
                }
            }
            else if (nodo.ChildNodes.Count == 2)
            {
                return new Operacion(VALORES_LOGICOS(nodo.ChildNodes.ElementAt(1)), Tipo.NOT, line, column);
            }
            else
            {
                return OPERACION_LOGICA(nodo.ChildNodes.ElementAt(0));
            }

        }

        private Operacion OPERACION_LOGICA(ParseTreeNode nodo)
        {
            int line = nodo.ChildNodes.ElementAt(0).Span.Location.Line;
            int column = nodo.ChildNodes.ElementAt(0).Span.Location.Column;
            if (nodo.ChildNodes.Count == 3)
            {
                string operador = nodo.ChildNodes.ElementAt(1).Term.Name.ToString();
                switch (operador)
                {
                    case ">":
                        return new Operacion(OPERACION_LOGICA(nodo.ChildNodes.ElementAt(0)), OPERACION_LOGICA(nodo.ChildNodes.ElementAt(2)), Tipo.MAYOR_QUE, line, column);
                    case "<":
                        return new Operacion(OPERACION_LOGICA(nodo.ChildNodes.ElementAt(0)), OPERACION_LOGICA(nodo.ChildNodes.ElementAt(2)), Tipo.MENOR_QUE, line, column);
                    case ">=":
                        return new Operacion(OPERACION_LOGICA(nodo.ChildNodes.ElementAt(0)), OPERACION_LOGICA(nodo.ChildNodes.ElementAt(2)), Tipo.MAYOR_IGUAL, line, column);
                    case "<=":
                        return new Operacion(OPERACION_LOGICA(nodo.ChildNodes.ElementAt(0)), OPERACION_LOGICA(nodo.ChildNodes.ElementAt(2)), Tipo.MENOR_IGUAL, line, column);
                    case "==":
                        return new Operacion(OPERACION_LOGICA(nodo.ChildNodes.ElementAt(0)), OPERACION_LOGICA(nodo.ChildNodes.ElementAt(2)), Tipo.IGUAL, line, column);
                    case "!=":
                        return new Operacion(OPERACION_LOGICA(nodo.ChildNodes.ElementAt(0)), OPERACION_LOGICA(nodo.ChildNodes.ElementAt(2)), Tipo.DIFERENTE, line, column);
                    default:
                        return OPERACION_LOGICA(nodo.ChildNodes.ElementAt(1));
                }
            }
            else
            {
                return OPERACION_NUMERICA(nodo.ChildNodes.ElementAt(0));
            }
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
            else if (nodo.ChildNodes.Count == 3)
            {

                string name = nodo.ChildNodes.ElementAt(1).Token.Text;
                Variable variable = new Variable();
                variable.Atributos = VAR_ATTRS(nodo.ChildNodes.ElementAt(2));
                variable.Id = "@" + nodo.ChildNodes.ElementAt(1).Token.Value.ToString();
                Valor new_valor = new Valor(variable, Tipo.VARIABLE_ATRIBUTOS);
                return new_valor;

            }
            else if (nodo.ChildNodes.Count == 4)
            {
                Variable new_var = new Variable();
                new_var.Valor = METODOS(nodo.ChildNodes.ElementAt(3));
                new_var.Id = "@" + nodo.ChildNodes.ElementAt(1).Token.Value.ToString();
                return new Valor(new_var, Tipo.VARIABLE_METODOS);
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
        string as_type = "";
        private Variable VARIABLE(ParseTreeNode nodo)
        {
            Variable new_variable = new Variable();
            new_variable.Id = "@" + nodo.ChildNodes.ElementAt(1).Token.Text;
            if (nodo.ChildNodes.ElementAt(2).Term.Name.Equals("VAR_ATTRS") && nodo.ChildNodes.Count == 4)
            {

                if (nodo.ChildNodes.ElementAt(3).ChildNodes.Count == 0)
                {
                    new_variable.Instanciada = false;
                    new_variable.Valor = null;
                    new_variable.Atributos = VAR_ATTRS(nodo.ChildNodes.ElementAt(2));
                }
                else
                {
                    new_variable.Instanciada = true;
                    lst_ids = new List<string>();
                    as_type = "";
                    new_variable.Atributos = VAR_ATTRS(nodo.ChildNodes.ElementAt(2));
                    new_variable.Valor = INICIALIZACION(nodo.ChildNodes.ElementAt(3));
                    new_variable.Lst_variables = lst_ids;
                    lst_ids = new List<string>();
                    new_variable.As_type = as_type;
                    new_variable.Is_var = is_var;

                }
            }
            /*else if (nodo.ChildNodes.Count == 4) {
                Variable_Metodo new_var = METODOS(nodo.ChildNodes.ElementAt(3));
                new_var.Id = "@"+ nodo.ChildNodes.ElementAt(1);
                return new_var;

            }*/
            else
            {
                if (nodo.ChildNodes.ElementAt(2).ChildNodes.Count == 0)
                {
                    new_variable.Instanciada = false;
                    new_variable.Valor = null;
                }
                else
                {
                    new_variable.Instanciada = true;
                    lst_ids = new List<string>();
                    as_type = "";
                    new_variable.Valor = INICIALIZACION(nodo.ChildNodes.ElementAt(2));
                    new_variable.Lst_variables = lst_ids;
                    lst_ids = new List<string>();
                    new_variable.As_type = as_type;
                    new_variable.Is_var = is_var;

                }
            }

            is_var = false;
            return new_variable;
        }

        private Variable_Metodo METODOS(ParseTreeNode nodo)
        {
            string op = nodo.ChildNodes.ElementAt(0).Term.Name.ToString();
            Variable_Metodo new_metodo = new Variable_Metodo(op);
            switch (op)
            {
                case "INSERT":
                    if (nodo.ChildNodes.Count == 6)
                    {
                        new_metodo.Clave = OPERACION_NUMERICA(nodo.ChildNodes.ElementAt(2));
                        new_metodo.Valor = VUT(nodo.ChildNodes.ElementAt(4));
                    }
                    else
                    {
                        new_metodo.Valor = VUT(nodo.ChildNodes.ElementAt(2));
                    }
                    break;
                case "SET":
                    new_metodo.Clave = OPERACION_NUMERICA(nodo.ChildNodes.ElementAt(2));
                    new_metodo.Valor = VUT(nodo.ChildNodes.ElementAt(4));
                    break;
                case "REMOVE":
                    new_metodo.Clave = OPERACION_NUMERICA(nodo.ChildNodes.ElementAt(2));
                    break;
                case "CLEAR":
                    break;
                case "GET":
                    new_metodo.Clave = OPERACION_NUMERICA(nodo.ChildNodes.ElementAt(2));
                    break;
                case "SIZE":
                    break;
                case "CONTAINS":
                    new_metodo.Clave = OPERACION_NUMERICA(nodo.ChildNodes.ElementAt(2));
                    break;
            }

            return new_metodo;
        }

        private Stack<string> VAR_ATTRS(ParseTreeNode nodo)
        {
            if (nodo.ChildNodes.Count == 3)
            {
                Stack<string> val = VAR_ATTRS(nodo.ChildNodes.ElementAt(0));
                val.Push(VAR_ATTR(nodo.ChildNodes.ElementAt(1)));
                return val;
            }
            else
            {
                Stack<string> val = new Stack<string>();
                val.Push(VAR_ATTR(nodo.ChildNodes.ElementAt(0)));
                return val;
            }
        }

        private string VAR_ATTR(ParseTreeNode nodo)
        {
            return nodo.ChildNodes.ElementAt(1).Token.Value.ToString();
        }

        private object INICIALIZACION(ParseTreeNode nodo)
        {
            if (nodo.ChildNodes.Count == 2)
            {
                return OPERACION_NUMERICA(nodo.ChildNodes.ElementAt(1));
            }
            else if (nodo.ChildNodes.Count == 3)
            {
                if (nodo.ChildNodes.ElementAt(2).Term.Name.Equals("Identificador"))
                {
                    return new Tipo_Objeto(nodo.ChildNodes.ElementAt(2).Token.Value.ToString(), "");
                }
                else if (nodo.ChildNodes.ElementAt(2).Term.Name.Equals("MAP"))
                {
                    return MAP(nodo.ChildNodes.ElementAt(2));
                }
                else if (nodo.ChildNodes.ElementAt(2).Term.Name.Equals("LISTA"))
                {
                    return LISTA(nodo.ChildNodes.ElementAt(2));
                }
            }
            else if (nodo.ChildNodes.Count == 4 && nodo.ChildNodes.ElementAt(1).Token.Value.Equals("@"))
            {

                Variable variable = new Variable();
                variable.Atributos = VAR_ATTRS(nodo.ChildNodes.ElementAt(3));
                variable.Id = "@" + nodo.ChildNodes.ElementAt(2).Token.Value.ToString();

                return variable;
            }
            else if (nodo.ChildNodes.Count == 4)
            {
                if (nodo.ChildNodes.ElementAt(2).Term.Name.Equals("MAP_VALS"))
                {
                    Map new_map = new Map("", "");
                    new_map.Mapita = MAP_VALS(nodo.ChildNodes.ElementAt(2));
                    new_map.Clave = clave;
                    new_map.Valor = valor;
                    clave = valor = "";
                    return new_map;
                }
                else
                {
                    Lista new_lista = new Lista("");
                    new_lista.Lista_valores = (List<Tipo_Collection>)VALORES2(nodo.ChildNodes.ElementAt(2));
                    new_lista.Tipo = valor;
                    clave = valor = "";
                    return new_lista;

                }
            }
            else if (nodo.ChildNodes.Count == 6)
            {
                as_type = nodo.ChildNodes.ElementAt(5).Token.Value.ToString();
                return VALORES2(nodo.ChildNodes.ElementAt(2));
            }

            //si es igual a 4 entonces nos ponemos hacer incremento 
            return null;
        }

        private object LISTA(ParseTreeNode nodo)
        {
            return new Lista(VALOR(nodo.ChildNodes.ElementAt(2)).ToString());
        }

        private List<Item_Map> MAP_VALS(ParseTreeNode nodo)
        {
            if (nodo.ChildNodes.Count == 3)
            {
                List<Item_Map> lst = MAP_VALS(nodo.ChildNodes.ElementAt(0));
                lst.Add(MAP_VAL(nodo.ChildNodes.ElementAt(2)));
                return lst;
            }
            else
            {
                List<Item_Map> lst = new List<Item_Map>();
                lst.Add(MAP_VAL(nodo.ChildNodes.ElementAt(0)));
                return lst;

            }
        }

        private Item_Map MAP_VAL(ParseTreeNode nodo)
        {
            return new Item_Map(VALOR(nodo.ChildNodes.ElementAt(0)).ToString(), VUT(nodo.ChildNodes.ElementAt(2)));
        }

        private object MAP(ParseTreeNode nodo)
        {
            string attr1 = VALOR(nodo.ChildNodes.ElementAt(2)).ToString();
            string attr2 = VALOR(nodo.ChildNodes.ElementAt(4)).ToString();

            return new Map(attr1, attr2);
        }

        private List<Tipo_Collection> VALORES2(ParseTreeNode nodo)
        {
            if (nodo.ChildNodes.Count == 3)
            {
                List<Tipo_Collection> lst_valores = VALORES2(nodo.ChildNodes.ElementAt(0));
                lst_valores.Add(VUT(nodo.ChildNodes.ElementAt(2)));
                return lst_valores;
            }
            else
            {
                List<Tipo_Collection> lst_valores = new List<Tipo_Collection>
                {
                    VUT(nodo.ChildNodes.ElementAt(0))
                };
                return lst_valores;
            }
        }

        private Tipo_Collection VUT(ParseTreeNode nodo)
        {
            if (nodo.ChildNodes.Count == 1)
            {
                valor = "double";
                return new Tipo_Collection(Tipo.OPERACION, VALORES_LOGICOS(nodo.ChildNodes.ElementAt(0)), "");
            }
            else if (nodo.ChildNodes.Count == 5)
            {
                valor = nodo.ChildNodes.ElementAt(4).Token.Value.ToString();
                return new Tipo_Collection(Tipo.USER_TYPES, VALORES2(nodo.ChildNodes.ElementAt(1)), nodo.ChildNodes.ElementAt(4).Token.Value.ToString());
            }
            else return null;
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
                        Operacion op1, op2;
                        concatenacion = false;
                        op1 = OPERACION_NUMERICA(nodo.ChildNodes.ElementAt(0));
                        op2 = OPERACION_NUMERICA(nodo.ChildNodes.ElementAt(2));
                        if (!concatenacion) return new Operacion(op1, op2, Tipo.SUMA, line, colum);
                        else return new Operacion(op1, op2, Tipo.CONCATENACION, line, colum);
                    case "-":
                        return new Operacion(OPERACION_NUMERICA(nodo.ChildNodes.ElementAt(0)), OPERACION_NUMERICA(nodo.ChildNodes.ElementAt(2)), Tipo.RESTA, line, colum);
                    case "*":
                        return new Operacion(OPERACION_NUMERICA(nodo.ChildNodes.ElementAt(0)), OPERACION_NUMERICA(nodo.ChildNodes.ElementAt(2)), Tipo.MULTIPLICACION, line, colum);
                    case "/":
                        return new Operacion(OPERACION_NUMERICA(nodo.ChildNodes.ElementAt(0)), OPERACION_NUMERICA(nodo.ChildNodes.ElementAt(2)), Tipo.DIVISION, line, colum);
                    case "**":
                        return new Operacion(OPERACION_NUMERICA(nodo.ChildNodes.ElementAt(0)), OPERACION_NUMERICA(nodo.ChildNodes.ElementAt(2)), Tipo.POTENCIA, line, colum);
                    case "%":
                        return new Operacion(OPERACION_NUMERICA(nodo.ChildNodes.ElementAt(0)), OPERACION_NUMERICA(nodo.ChildNodes.ElementAt(2)), Tipo.MODULAR, line, colum);
                    case "Identificador":
                        if (nodo.ChildNodes.ElementAt(2).Term.Name.Equals("++"))
                            return new Operacion("@" + nodo.ChildNodes.ElementAt(1).Token.Value.ToString(), Tipo.INCREMENTO, line, colum);
                        else return new Operacion("@" + nodo.ChildNodes.ElementAt(1).Token.Value.ToString(), Tipo.DECREMENTO, line, colum);

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
            else if (nodo.ChildNodes.Count == 4)
            {
                Variable new_var = new Variable();
                new_var.Valor = METODOS(nodo.ChildNodes.ElementAt(3));
                new_var.Id = "@" + nodo.ChildNodes.ElementAt(1).Token.Value.ToString();
                return new Operacion(new_var, Tipo.VARIABLE_METODOS, line, colum);
            }
            else
            {
                string valor = nodo.ChildNodes.ElementAt(0).ChildNodes.ElementAt(0).Term.Name;
                if (valor.ToLower().Equals("numero"))
                    return new Operacion(VALOR(nodo.ChildNodes.ElementAt(0)), line, colum);
                else if (valor.ToLower().Equals("cadena"))
                {
                    valor = "string";
                    concatenacion = true;
                    return new Operacion(VALOR(nodo.ChildNodes.ElementAt(0)).ToString(), Tipo.CADENA, line, colum);
                }
                else return new Operacion(nodo.ChildNodes.ElementAt(0).ChildNodes.ElementAt(0).Token.Value.ToString(), Tipo.BOOLEANO, line, colum);
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
                    clave = "String";
                    return node.ChildNodes[0].ToString().Replace(" (Cadena)", "");
                case "Identificador":
                    return node.ChildNodes[0].ToString().Replace(" (Identificador)", "");
                case "Numero":
                    clave = "double";
                    return node.ChildNodes[0].ToString().Replace(" (Numero)", "");
                default:
                    return evaluar;
            }
        }
    }
}
