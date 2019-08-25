using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Irony.Ast;
using Irony.Parsing;

namespace Servidor.Analizador.CQL
{
    class Gramatica_CQL : Grammar
    {
        public Gramatica_CQL() : base(caseSensitive: false)
        {

            #region Expresiones Regulares
            StringLiteral CADENA = new StringLiteral("Cadena", "\"");
            NumberLiteral NUMERO = new NumberLiteral("Numero");
            var DECIMAL = new RegexBasedTerminal("Decimal", "[0-9]+'.'[0-9]+");
            var RDATE = new RegexBasedTerminal("Date", "\'[0-9][0-9][0-9][0-9]-[0-9][0-9]-[0-9][0-9]\'");
            var RTIME = new RegexBasedTerminal("Time", "\'[0-9][0-9]:[0-9][0-9]:[0-9][0-9]\'");
            IdentifierTerminal IDENTIFICADOR = new IdentifierTerminal("Identificador");

            CommentTerminal comentarioLinea = new CommentTerminal("comentarioLinea", "//", "\n", "\r\n");//Comentario de una linea
            CommentTerminal comentarioBloque = new CommentTerminal("comentarioBloque", "/*", "*/");//Comentario multilinea
            #endregion

            #region Terminales
            KeyTerm
                MENQUE = ToTerm("<"),
                MAYQUE = ToTerm(">"),
                IGUAL = ToTerm("="),
                CORIZQ = ToTerm("["),
                CORDER = ToTerm("]"),
                COMA = ToTerm(","),
                PARIZQ = ToTerm("("),
                PARDER = ToTerm(")"),
                PUNTO = ToTerm("."),
                LLAVIZQ = ToTerm("{"),
                LLAVDER = ToTerm("}"),
                PTCOMA = ToTerm(";"),
                RNULL = ToTerm("null"),
                RFALSE = ToTerm("false"),
                RTRUE = ToTerm("true"),
                RCREATE = ToTerm("CREATE"),
                RDATABASE = ToTerm("DATABASE"),
                RUSE = ToTerm("USE"),
                RDROP = ToTerm("DROP"),
                RALTER = ToTerm("ALTER"),
                RTRUNCATE = ToTerm("TRUNCATE"),
                RADD = ToTerm("ADD"),
                RIF_NOT_EXISTS = ToTerm("IF NOT EXISTS"),
                RIF_EXISTS = ToTerm("IF EXISTS"),
                RTABLE = ToTerm("TABLE"),
                RPRIMARY_KEY = ToTerm("PRIMARY KEY"),
                TSTRING = ToTerm("string"),
                TINT = ToTerm("int"),
                TDOUBLE = ToTerm("double"),
                TDATE = ToTerm("date"),
                TTIME = ToTerm("time"),
                TBOOLEAN = ToTerm("boolean"),
                TCOUNTER = ToTerm("counter"),
                TMAP = ToTerm("Map"),
                TSET = ToTerm("Set"),
                TLIST = ToTerm("List")
                ;
            #endregion

            #region No_Terminales
            NonTerminal
                S = new NonTerminal("S"),
                Instrucciones = new NonTerminal("Instrucciones"),
                Instruccion = new NonTerminal("Instruccion"),
                DDL = new NonTerminal("DDL"),
                CREATE_DB = new NonTerminal("CREATE_DB"),
                VALOR = new NonTerminal("VALOR"),
                VALORES = new NonTerminal("VALORES"),
                IFNE = new NonTerminal("IFNE"),
                PUSE = new NonTerminal("PUSE"),
                TRUNCATE_TABLE =new NonTerminal("TRUNCATE_TABLE"),
                DROP_DB = new NonTerminal("DROP_DB"),
                CREATE_TABLE = new NonTerminal("CREATE_TABLE"),
                ALTER_TABLE = new NonTerminal("ALTER_TABLE"),
                ADD_COLUMNS = new NonTerminal("ADD_COLUMNS"),
                ADD_COLUMN = new NonTerminal("ADD_COLUMN"),
                DROP_TABLE = new NonTerminal("DROP_TABLE"),
                IFE = new NonTerminal("IFE"),
                DROP_COLUMNS = new NonTerminal("DROP_COLUMNS"),
                DROP_COLUMN = new NonTerminal("DROP_COLUMN"),
                COLUMNS = new NonTerminal("COLUMNS"),
                COLUMN = new NonTerminal("COLUMN"),
                TIPO_DATO = new NonTerminal("TIPO_DATO"),
                IFN_PK = new NonTerminal("IFN_PK"),
                IFC_PK = new NonTerminal("IFC_PK")

                ;


            #endregion

            #region Gramatica
            S.Rule = Instrucciones;

            Instrucciones.Rule = Instrucciones + Instruccion
                | Instruccion;

            Instruccion.Rule = DDL;

            DDL.Rule = CREATE_DB
                | PUSE
                | DROP_DB
                | CREATE_TABLE
                | ALTER_TABLE
                | DROP_TABLE
                | TRUNCATE_TABLE;

            #region DDL
            // CREATE DATABASE
            CREATE_DB.Rule = RCREATE + RDATABASE + IFNE + VALOR + PTCOMA;
            //IF NOT EXIST
            IFNE.Rule = RIF_NOT_EXISTS
                | Empty;
            //USE DATABASE
            PUSE.Rule = RUSE + VALOR + PTCOMA;
            //DROP DATABASE
            DROP_DB.Rule = RDROP + RDATABASE + VALOR + PTCOMA;
            //CREATE TABLE
            CREATE_TABLE.Rule = RCREATE + RTABLE + IFNE + VALOR + PARIZQ + COLUMNS + IFC_PK + PARDER + PTCOMA;
            //OBTENER COLUMNAS O COLUMNA
            COLUMNS.Rule = COLUMNS + COMA + COLUMN
                | COLUMN;
            //OBTENER DATO DE COLUMNA
            COLUMN.Rule = IDENTIFICADOR + TIPO_DATO + IFN_PK;
            // SI LA COLUMNA TIENE LA PALABRA PRIMARY KEY O VIENE VACIA
            IFN_PK.Rule = RPRIMARY_KEY
                | Empty;
            //SI LA LLAVE ES COMPUESTA
            IFC_PK.Rule = COMA + RPRIMARY_KEY + PARIZQ + VALORES + PARDER
                | Empty;
            //ALTER TABLE ADD O DROP
            ALTER_TABLE.Rule = RALTER + RTABLE + IDENTIFICADOR + RADD + ADD_COLUMNS + PTCOMA
                | RALTER + RTABLE + IDENTIFICADOR + RDROP + DROP_COLUMNS + PTCOMA;
            // COLUMNA, COLUMNA, COLUMNA
            ADD_COLUMNS.Rule = ADD_COLUMNS + COMA + ADD_COLUMN
                | ADD_COLUMN;
            //ID TIPO
            ADD_COLUMN.Rule = IDENTIFICADOR + TIPO_DATO;
            //COLUMNA, COLUMNA
            DROP_COLUMNS.Rule = DROP_COLUMNS + COMA + DROP_COLUMN
                | DROP_COLUMN;
            //ID
            DROP_COLUMN.Rule = IDENTIFICADOR;
            //DROP TABLE
            DROP_TABLE.Rule = RDROP + RTABLE + IFE + IDENTIFICADOR + PTCOMA;
            // IF EXISTS VIENE
            IFE.Rule = RIF_EXISTS
                | Empty;
            //TRUNCATE TABLE
            TRUNCATE_TABLE.Rule = RTRUNCATE + RTABLE + IDENTIFICADOR + PTCOMA;

            #endregion

            //TIPOS DE DATOS POR EJEMPLO INT, DOUBLE, STRING, BOOLEAN, ETC.
            TIPO_DATO.Rule = TSTRING
                | TSET
                | TTIME
                | TDATE
                | TCOUNTER
                | TDOUBLE
                | TBOOLEAN
                | TINT
                | TMAP
                | TLIST
                | IDENTIFICADOR
                ;

            // SI VIENE POR EJEMPLO 1,2,3,4 O PERSONA, PERRO, CARRO O TRUE, FALSE, TRUE
            VALORES.Rule = VALORES + COMA + VALOR
                | VALOR;
            //SOLO OBTIENE EL VALOR 
            VALOR.Rule = IDENTIFICADOR
                | NUMERO
                | DECIMAL
                | CADENA
                | RTRUE
                | RDATE
                | RTIME
                | RNULL
                | RFALSE;
            //agregar los tipos de datos, como collections, types.

            #region Preferencias
            this.Root = S;
            string[] palabras = { RPRIMARY_KEY.Text,RDROP.Text, RADD.Text, RIF_NOT_EXISTS.Text };
            MarkReservedWords(palabras);
            #endregion


            #endregion

        }
    }
}