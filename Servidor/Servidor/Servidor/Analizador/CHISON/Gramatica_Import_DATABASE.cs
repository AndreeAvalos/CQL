using Irony.Parsing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Servidor.Analizador.CHISON
{
    class Gramatica_Import_DATABASE : Grammar
    {
        public Gramatica_Import_DATABASE() : base(caseSensitive: false)
        {

            #region Expresiones_Regulares
            StringLiteral CADENA = new StringLiteral("Cadena", "\"");
            NumberLiteral NUMERO = new NumberLiteral("Numero");
            var DECIMAL = new RegexBasedTerminal("Decimal", "[0-9]+'.'[0-9]+");
            var RDATE = new RegexBasedTerminal("Date", "\'[0-9][0-9][0-9][0-9]-[0-9][0-9]-[0-9][0-9]\'");
            var RTIME = new RegexBasedTerminal("Time", "\'[0-9][0-9]:[0-9][0-9]:[0-9][0-9]\'");
            IdentifierTerminal IDENTIFICADOR = new IdentifierTerminal("Identificador");

            CommentTerminal comentarioLinea = new CommentTerminal("comentarioLinea", "//", "\n", "\r\n"); //si viene una nueva linea se termina de reconocer el comentario.
            CommentTerminal comentarioBloque = new CommentTerminal("comentarioBloque", "/*", "*/");

            CommentTerminal ruta_import = new CommentTerminal("ruta_import", "${", "}$");
            CommentTerminal CONT_DATA_IMPORT = new CommentTerminal("CONT_DATA_IMPORT", "$", "$");
            #endregion

            #region Terminales
            KeyTerm
                DOLLAR = ToTerm("$"),
                MENQUE = ToTerm("<"),
                MAYQUE = ToTerm(">"),
                IGUAL = ToTerm("="),
                CORIZQ = ToTerm("["),
                CORDER = ToTerm("]"),
                COMA = ToTerm(","),
                PUNTO = ToTerm("."),
                LLAVIZQ = ToTerm("{"),
                LLAVDER = ToTerm("}"),
                RNAME = ToTerm("\"NAME\""),
                RDATA = ToTerm("\"DATA\""),
                RCQL_TYPE = ToTerm("\"CQL-TYPE\""),
                RCOLUMNS = ToTerm("\"COLUMNS\""),
                RTYPE = ToTerm("\"TYPE\""),
                RPK = ToTerm("\"PK\""),
                RATTRS = ToTerm("\"ATTRS\""),
                RINSTR = ToTerm("\"INSTR\""),
                RPARAMETERS = ToTerm("\"PARAMETERS\""),
                RAS = ToTerm("\"AS\""),
                RFALSE = ToTerm("FALSE"),
                RTRUE = ToTerm("TRUE"),
                RIN = ToTerm("IN"),
                ROUT = ToTerm("OUT"),
                RNULL = ToTerm("NULL");
            #endregion

            #region No_Terminales
            NonTerminal
                S = new NonTerminal("S"),
                NAME = new NonTerminal("NAME"),
                VALOR = new NonTerminal("VALOR"),
                DATA2 = new NonTerminal("DATA2"),
                DATA3 = new NonTerminal("DATA3"),
                DATA4 = new NonTerminal("DATA4"),
                DATA5 = new NonTerminal("DATA5"),
                TABLA = new NonTerminal("TABLA"),
                OBJETO = new NonTerminal("OBJETO"),
                PROCEDURE = new NonTerminal("PROCEDURE"),
                CQL_TYPE = new NonTerminal("CQL_TYPE"),
                COLUMNS = new NonTerminal("COLUMNS"),
                DATA_DATA = new NonTerminal("DATA_DATA"),
                DATA_DATA2 = new NonTerminal("DATA_DATA2"),
                DATA_DATA3 = new NonTerminal("DATA_DATA3"),
                DATA_DATA4 = new NonTerminal("DATA_DATA4"),
                DATA_DATA5 = new NonTerminal("DATA_DATA5"),
                DATA_DATA6 = new NonTerminal("DATA_DATA6"),
                COLUMNS2 = new NonTerminal("COLUMNS2"),
                COLUMNS3 = new NonTerminal("COLUMNS3"),
                COLUMNS4 = new NonTerminal("COLUMNS4"),
                COLUMN = new NonTerminal("COLUMN"),
                TYPE = new NonTerminal("TYPE"),
                PK = new NonTerminal("PK"),
                MAPA = new NonTerminal("MAPA"),
                MAPA2 = new NonTerminal("MAPA2"),
                MAPA3 = new NonTerminal("MAPA3"),
                LISTAS = new NonTerminal("LISTAS"),
                LISTAS2 = new NonTerminal("LISTAS2"),
                LISTAS3 = new NonTerminal("LISTAS3"),
                ATTRIBUTES = new NonTerminal("ATTRIBUTES"),
                ATTRIBUTES2 = new NonTerminal("ATTRIBUTES2"),
                ATTRIBUTES3 = new NonTerminal("ATTRIBUTES3"),
                ATTRIBUTES4 = new NonTerminal("ATTRIBUTES4"),
                ATTRIBUTE = new NonTerminal("ATTRIBUTE"),
                PARAMETERS = new NonTerminal("PARAMETERS"),
                PARAMETERS2 = new NonTerminal("PARAMETERS2"),
                PARAMETERS3 = new NonTerminal("PARAMETERS3"),
                PARAMETERS4 = new NonTerminal("PARAMETERS4"),
                PARAMETER = new NonTerminal("PARAMETER"),
                INSTR = new NonTerminal("INSTR"),
                BOOL = new NonTerminal("BOOL"),
                AS = new NonTerminal("AS"),
                IN_OUT = new NonTerminal("IN_OUT");

            ;
            #endregion

            #region GRAMATICA
            S.Rule = DATA5;


            NAME.Rule = RNAME + IGUAL + VALOR;



            DATA5.Rule = DATA5 + COMA + DATA2
                | DATA2
                | ruta_import
                | Empty;

            DATA2.Rule = MENQUE + DATA3 + MAYQUE;


            DATA3.Rule = DATA3 + COMA + DATA4
                | DATA4;

            DATA4.Rule = CQL_TYPE
                | NAME
                | TABLA
                | OBJETO
                | PROCEDURE;

            TABLA.Rule = COLUMNS
                | DATA_DATA;


            CQL_TYPE.Rule = RCQL_TYPE + IGUAL + VALOR;

            COLUMNS.Rule = RCOLUMNS + IGUAL + CORIZQ + COLUMNS4 + CORDER;

            COLUMNS4.Rule = COLUMNS4 + COMA + COLUMNS2
                | COLUMNS2
                | Empty;

            COLUMNS2.Rule = MENQUE + COLUMNS3 + MAYQUE;



            COLUMNS3.Rule = COLUMNS3 + COMA + COLUMN
                | COLUMN;


            COLUMN.Rule = NAME
                | TYPE
                | PK;

            TYPE.Rule = RTYPE + IGUAL + VALOR;

            PK.Rule = RPK + IGUAL + BOOL;

            DATA_DATA.Rule = RDATA + IGUAL + CORIZQ + DATA_DATA2 + CORDER;

            DATA_DATA2.Rule = ruta_import
                | DATA_DATA3
                | Empty;

            DATA_DATA3.Rule = DATA_DATA3 + COMA + DATA_DATA4
                | DATA_DATA4;

            DATA_DATA4.Rule = MENQUE + DATA_DATA5 + MAYQUE;


            DATA_DATA5.Rule = DATA_DATA5 + COMA + DATA_DATA6
                | DATA_DATA6;

            DATA_DATA6.Rule = VALOR + IGUAL + VALOR;

            OBJETO.Rule = ATTRIBUTES;
            ATTRIBUTES.Rule = RATTRS + IGUAL + CORIZQ + ATTRIBUTES2 + CORDER;

            ATTRIBUTES2.Rule = ATTRIBUTES2 + COMA + ATTRIBUTES3
                | ATTRIBUTES3
                | Empty;

            ATTRIBUTES3.Rule = MENQUE + ATTRIBUTES4 + MAYQUE;

            ATTRIBUTES4.Rule = ATTRIBUTES4 + COMA + ATTRIBUTE
                | ATTRIBUTE;

            ATTRIBUTE.Rule = NAME
                | TYPE;

            PROCEDURE.Rule = PARAMETERS
                | INSTR;

            INSTR.Rule = RINSTR + IGUAL + CONT_DATA_IMPORT;

            PARAMETERS.Rule = RPARAMETERS + IGUAL + CORIZQ + PARAMETERS2 + CORDER;

            PARAMETERS2.Rule = PARAMETERS2 + COMA + PARAMETERS3
                | PARAMETERS3;

            PARAMETERS3.Rule = MENQUE + PARAMETERS4 + MAYQUE;

            PARAMETERS4.Rule = PARAMETERS4 + COMA + PARAMETER
                | PARAMETER;

            PARAMETER.Rule = NAME
                | TYPE
                | AS;

            AS.Rule = RAS + IGUAL + IN_OUT;

            IN_OUT.Rule = RIN
                | ROUT;

            BOOL.Rule = RTRUE
                 | RFALSE;

            VALOR.Rule = CADENA
                | DECIMAL
                | IDENTIFICADOR
                | MAPA
                | LISTAS
                | NUMERO
                | RTRUE
                | RFALSE
                | RTIME
                | RNULL
                | RDATE;

            #region  LISTAS y MAPAS
            LISTAS.Rule = CORIZQ + LISTAS2 + CORDER;

            LISTAS2.Rule = LISTAS2 + COMA + LISTAS3
                | LISTAS3
                | Empty;

            LISTAS3.Rule = VALOR;


            MAPA.Rule = MENQUE + MAPA2 + MAYQUE;

            MAPA2.Rule = MAPA2 + COMA + MAPA3
                | MAPA3
                | Empty;

            MAPA3.Rule = VALOR + IGUAL + VALOR;

            #endregion


            #region Preferencias
            this.Root = S;
            #endregion

            #endregion
        }

    }
}
