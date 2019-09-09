using Irony.Parsing;

namespace Servidor.Analizador.CHISON
{
    class Gramatica_Import_User : Grammar
    {
        public Gramatica_Import_User() : base(caseSensitive: false)
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

                MENQUE = ToTerm("<"),
                MAYQUE = ToTerm(">"),
                IGUAL = ToTerm("="),
                CORIZQ = ToTerm("["),
                CORDER = ToTerm("]"),
                COMA = ToTerm(","),
                RNAME = ToTerm("\"NAME\""),
                RUSERS = ToTerm("\"USERS\""),
                RPASSWORD = ToTerm("\"PASSWORD\""),
                RPERMISSIONS = ToTerm("\"PERMISSIONS\"");

            #endregion

            #region No_Terminales
            NonTerminal
                S = new NonTerminal("S"),
                NAME = new NonTerminal("NAME"),
                VALOR = new NonTerminal("VALOR"),
                USER = new NonTerminal("USER"),
                USERS2 = new NonTerminal("USERS2"),
                USERS3 = new NonTerminal("USERS3"),
                USERS4 = new NonTerminal("USERS4"),
                PASSWORD = new NonTerminal("PASSWORD"),
                PERMISSIONS = new NonTerminal("PERMISSIONS"),
                PERMISSIONS2 = new NonTerminal("PERMISSIONS2"),
                PERMISSION = new NonTerminal("PERMISSION")
                ;
            #endregion

            #region GRAMATICA
            S.Rule = USERS2;

            NAME.Rule = RNAME + IGUAL + VALOR;

            USERS2.Rule = USERS2 + COMA + USERS3
                | USERS3
                | Empty;
            USERS2.ErrorRule = SyntaxError + MENQUE;

            USERS3.Rule = MENQUE + USERS4 + MAYQUE;


            USERS4.Rule = USERS4 + COMA + USER
                | USER;

            USER.Rule = NAME
                | PASSWORD
                | PERMISSIONS;

            VALOR.Rule = CADENA
                | DECIMAL
                | IDENTIFICADOR
                | NUMERO
                | RTIME
                | RDATE;

            PASSWORD.Rule = RPASSWORD + IGUAL + VALOR;

            PERMISSIONS.Rule = RPERMISSIONS + IGUAL + CORIZQ + PERMISSIONS2 + CORDER;


            PERMISSIONS2.Rule = PERMISSIONS2 + COMA + PERMISSION
                | PERMISSION
                | Empty;

            PERMISSION.Rule = MENQUE + NAME + MAYQUE;
            #endregion

            #region PREFERENCIAS
            this.Root = S;

            #endregion

        }
    }
}
