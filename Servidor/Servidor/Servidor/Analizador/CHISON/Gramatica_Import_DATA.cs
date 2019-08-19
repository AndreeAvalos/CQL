using Irony.Parsing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Servidor.Analizador.CHISON
{
    class Gramatica_Import_DATA : Grammar
    {
        public Gramatica_Import_DATA() : base(caseSensitive: false)
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

            CommentTerminal CONT_DATA_IMPORT = new CommentTerminal("CONT_DATA_IMPORT", "$", "$");
            #endregion

            #region Terminales
            KeyTerm
                MENQUE = ToTerm("<"),
                MAYQUE = ToTerm(">"),
                IGUAL = ToTerm("="),
                CORIZQ = ToTerm("["),
                CORDER = ToTerm("]"),
                COMA = ToTerm(","),
                RFALSE = ToTerm("FALSE"),
                RTRUE = ToTerm("TRUE");

            #endregion

            #region No_Terminales
            NonTerminal
                S = new NonTerminal("S"),

                VALOR = new NonTerminal("VALOR"),

                DATA_DATA3 = new NonTerminal("DATA_DATA3"),
                DATA_DATA4 = new NonTerminal("DATA_DATA4"),
                DATA_DATA5 = new NonTerminal("DATA_DATA5"),
                DATA_DATA6 = new NonTerminal("DATA_DATA6"),

                MAPA = new NonTerminal("MAPA"),
                MAPA2 = new NonTerminal("MAPA2"),
                MAPA3 = new NonTerminal("MAPA3");

                ;
            #endregion

            #region GRAMATICA
            S.Rule = DATA_DATA3;

            DATA_DATA3.Rule = DATA_DATA3 + COMA + DATA_DATA4
                | DATA_DATA4;

            DATA_DATA4.Rule = MENQUE + DATA_DATA5 + MAYQUE;

            DATA_DATA5.Rule = DATA_DATA5 + COMA + DATA_DATA6
                |   DATA_DATA6;

            DATA_DATA6.Rule = VALOR + IGUAL + VALOR;

            VALOR.Rule = CADENA
                | DECIMAL
                | IDENTIFICADOR
                | MAPA
                | NUMERO
                | RTRUE
                | RFALSE
                | RTIME
                | RDATE
                | Empty;

            MAPA.Rule = CORIZQ + MAPA2 + CORDER;

            MAPA2.Rule = MAPA2 + COMA + MAPA3
                | MAPA3
                | Empty;

            MAPA3.Rule = VALOR;

            #endregion


            #region Preferencias
            this.Root = S;
            #endregion
        }
    }
}
