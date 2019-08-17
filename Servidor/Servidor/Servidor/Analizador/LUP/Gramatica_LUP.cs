using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Irony.Ast;
using Irony.Parsing;


namespace Servidor.Analizador.CQL
{
    class Gramatica_LUP : Grammar
    {
        public Gramatica_LUP() : base(caseSensitive: false)
        {
            #region Expresiones_Regulares
            StringLiteral CADENA = new StringLiteral("Cadena", "\"");
            NumberLiteral NUMERO = new NumberLiteral("Numero");
            IdentifierTerminal IDENTIFICADOR = new IdentifierTerminal("Identificador");
            #endregion

            #region Terminales
            var RLOGIN = ToTerm("LOGIN");
            var RSUCCESS = ToTerm("SUCCESS");
            var RFAIL = ToTerm("FAIL");
            var RLOGOUT = ToTerm("LOGOUT");
            var RUSER = ToTerm("USER");
            var RPASS = ToTerm("PASS");
            var RQUERY = ToTerm("QUERY");
            var RDATA = ToTerm("DATA");
            var RMESSAGE = ToTerm("MESSAGE");
            var RERROR = ToTerm("ERROR");
            var RLINE = ToTerm("LINE");
            var RCOLUMN = ToTerm("COLUMN");
            var RTYPE = ToTerm("TYPE");
            var RDESC = ToTerm("DESC");
            var RSTRUC = ToTerm("STRUC");
            var RDATABASES = ToTerm("DATABASES");
            var RDATABASE = ToTerm("DATABASE");
            var RNAME = ToTerm("NAME");
            var RTABLES = ToTerm("TABLES");
            var RTABLE = ToTerm("TABLE");
            var RCOLUMNS = ToTerm("COLUMNS");
            var RTYPES = ToTerm("TYPES");
            var RATTRIBUTES = ToTerm("ATTRIBUTES");
            var RPROCEDURES = ToTerm("PROCEDURES");

            var CORIZQ = ToTerm("[");
            var CORDER = ToTerm("]");
            var MAS = ToTerm("+");
            var MENOS = ToTerm("-");
            #endregion

            #region No_Terminales
            NonTerminal
                S = new NonTerminal("S"),
                Instrucciones = new NonTerminal("Instrucciones"),
                Instruccion = new NonTerminal("Instruccion"),
                LOGIN = new NonTerminal("LOGIN"),
                LOGOUT = new NonTerminal("LOGOUT"),
                QUERY = new NonTerminal("QUERY"),
                DATA = new NonTerminal("DATA"),
                MESSAGE = new NonTerminal("MESSAGE"),
                ERROR = new NonTerminal("ERROR"),
                STRUC = new NonTerminal("STRUC"),
                DATABASES = new NonTerminal("DATABASES"),
                USER = new NonTerminal("USER"),
                PASSWORD = new NonTerminal("PASSWORD"),
                VALOR = new NonTerminal("VALOR"),
                RESPUESTA = new NonTerminal("RESPUESTA"),
                LINEA = new NonTerminal("LINEA"),
                COLUMNA = new NonTerminal("COLUMNA"),
                TIPO = new NonTerminal("TIPO"),
                DESCRIPCION = new NonTerminal("DESCRIPCION"),
                DATABASES2 = new NonTerminal("DATABASES2"),
                DATABASE = new NonTerminal("DATABASE"),
                NAME = new NonTerminal("NAME"),
                TABLES = new NonTerminal("TABLES"),
                TABLES2 = new NonTerminal("TABLES2"),
                TABLE = new NonTerminal("TABLE"),
                COLUMNS = new NonTerminal("COLUMNS"),
                COLUMN = new NonTerminal("COLUMN"),
                TYPES = new NonTerminal("TYPES"),
                TYPES2 = new NonTerminal("TYPES2"),
                TYPE = new NonTerminal("TYPE"),
                ATTRIBUTES = new NonTerminal("ATTRIBUTES"),
                ATTRIBUTE = new NonTerminal("ATTRIBUTE"),
                PROCEDURES = new NonTerminal("PROCEDURES"),
                VALORES = new NonTerminal("VALORES");
            #endregion

            #region GRAMATICA

            S.Rule = Instrucciones;

            Instrucciones.Rule = Instrucciones + Instruccion
                | Instruccion;

            Instruccion.Rule = LOGIN
                | LOGOUT
                | QUERY
                | DATA
                | MESSAGE
                | ERROR
                | STRUC
                | DATABASES;

            LOGIN.Rule = CORIZQ + MAS + RLOGIN + CORDER + USER + PASSWORD + CORIZQ + MENOS + RLOGIN + CORDER
                | CORIZQ + MAS + RLOGIN + CORDER + RESPUESTA + CORIZQ + MENOS + RLOGIN + CORDER;

            USER.Rule = CORIZQ + MAS + RUSER + CORDER + VALOR + CORIZQ + MENOS + RUSER + CORDER;

            PASSWORD.Rule = CORIZQ + MAS + RPASS + CORDER + VALOR + CORIZQ + MENOS + RPASS + CORDER;

            VALOR.Rule = CADENA
                | IDENTIFICADOR
                | NUMERO;

            RESPUESTA.Rule = CORIZQ + RSUCCESS + CORDER
                | CORIZQ + RFAIL + CORDER;

            LOGOUT.Rule = CORIZQ + MAS + RLOGOUT + CORDER + RESPUESTA + CORIZQ + MENOS + RLOGOUT + CORDER
                | CORIZQ + MAS + RLOGOUT + CORDER + USER + CORIZQ + MENOS + RLOGOUT + CORDER;

            QUERY.Rule = CORIZQ + MAS + RQUERY + CORDER + USER + DATA + CORIZQ + MENOS + RQUERY + CORDER;

            DATA.Rule = CORIZQ + MAS + RDATA + CORDER + VALOR + CORIZQ + MENOS + RDATA + CORDER;

            MESSAGE.Rule = CORIZQ + MAS + RMESSAGE + CORDER + VALORES + CORIZQ + MENOS + RMESSAGE + CORDER;

            ERROR.Rule = CORIZQ + MAS + RERROR + CORDER + LINEA + COLUMNA + TIPO + DESCRIPCION + CORIZQ + MENOS + RERROR + CORDER;

            LINEA.Rule = CORIZQ + MAS + RLINE + CORDER + VALOR + CORIZQ + MENOS + RLINE + CORDER;

            COLUMNA.Rule = CORIZQ + MAS + RCOLUMN + CORDER + VALOR + CORIZQ + MENOS + RCOLUMN + CORDER;

            TIPO.Rule = CORIZQ + MAS + RTYPE + CORDER + VALOR + CORIZQ + MENOS + RTYPE + CORDER;

            DESCRIPCION.Rule = CORIZQ + MAS + RDESC + CORDER + VALOR + CORIZQ + MENOS + RDESC + CORDER;

            STRUC.Rule = CORIZQ + MAS + RSTRUC + CORDER + USER + CORIZQ + MENOS + RSTRUC + CORDER;

            DATABASES.Rule = CORIZQ + MAS + RDATABASES + CORDER + DATABASES2 + CORIZQ + MENOS + RDATABASES + CORDER;

            DATABASES2.Rule = DATABASES2 + DATABASE
                | DATABASE;

            DATABASE.Rule = CORIZQ + MAS + RDATABASE + CORDER + NAME + TABLES + TYPES + PROCEDURES + CORIZQ + MENOS + RDATABASE + CORDER;

            NAME.Rule = CORIZQ + MAS + RNAME + CORDER + VALOR + CORIZQ + MENOS + RNAME + CORDER;

            TABLES.Rule = CORIZQ + MAS + RTABLES + CORDER + TABLES2 + CORIZQ + MENOS + RTABLES + CORDER;

            TABLES2.Rule = TABLES2 + TABLE
                | TABLE;

            TABLE.Rule = CORIZQ + MAS + RTABLE + CORDER + NAME + COLUMNS + CORIZQ + MENOS + RTABLE + CORDER
                | CORIZQ + MAS + RTABLE + CORDER + VALORES + CORIZQ + MENOS + RTABLE + CORDER;

            COLUMNS.Rule = COLUMNS + COLUMN
                | COLUMN;

            COLUMN.Rule = CORIZQ + MAS + RCOLUMNS + CORDER + VALORES + CORIZQ + MENOS + RCOLUMNS + CORDER;

            TYPES.Rule = CORIZQ + MAS + RTYPES + CORDER + TYPES2 + CORIZQ + MENOS + RTYPES + CORDER;

            TYPES2.Rule = TYPES2 + TYPE
                | TYPE;

            TYPE.Rule = CORIZQ + MAS + RTYPE + CORDER + NAME + ATTRIBUTES + CORIZQ + MENOS + RTYPE + CORDER
                | CORIZQ + MAS + RTYPE + CORDER + VALORES + CORIZQ + MENOS + RTYPE + CORDER;

            ATTRIBUTES.Rule = ATTRIBUTES + ATTRIBUTE
                | ATTRIBUTE;

            ATTRIBUTE.Rule = CORIZQ + MAS + RATTRIBUTES + CORDER + VALORES + CORIZQ + MENOS + RATTRIBUTES + CORDER;

            PROCEDURES.Rule = CORIZQ + MAS + RPROCEDURES + CORDER + VALORES + CORIZQ + MENOS + RPROCEDURES + CORDER;

            VALORES.Rule = VALORES + VALOR
                | VALOR;

            #endregion

            #region Preferencias
            this.Root = S;
            #endregion


        }
    }
}