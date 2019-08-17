using Irony.Parsing;

namespace Servidor.Analizador.CHISON
{
    class Gramatica_CHISON : Grammar
    {
        public Gramatica_CHISON() : base(caseSensitive: false)
        {
            #region Expresiones_Regulares
            StringLiteral CADENA = new StringLiteral("Cadena", "\"");
            NumberLiteral NUMERO = new NumberLiteral("Numero");
            var DECIMAL = new RegexBasedTerminal("Decimal", "[0-9]+'.'[0-9]+");
            IdentifierTerminal IDENTIFICADOR = new IdentifierTerminal("Identificador");

            CommentTerminal comentarioLinea = new CommentTerminal("comentarioLinea", "//", "\n", "\r\n"); //si viene una nueva linea se termina de reconocer el comentario.
            CommentTerminal comentarioBloque = new CommentTerminal("comentarioBloque", "/*", "*/");

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
                RDATABASES = ToTerm("\"DATABASES\""),
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
                RUSERS = ToTerm("\"USERS\""),
                RPASSWORD = ToTerm("\"PASSWORD\""),
                RPERMISSIONS = ToTerm("\"PERMISSIONS\""),
                RFALSE = ToTerm("FALSE"),
                RTRUE = ToTerm("TRUE"),
                RIN = ToTerm("IN"),
                ROUT = ToTerm("OUT");
            #endregion

            #region No_Terminales
            NonTerminal
                S = new NonTerminal("S"),
                Instrucciones = new NonTerminal("Instrucciones"),
                Instrucciones2 = new NonTerminal("Instrucciones2"),
                Instruccion = new NonTerminal("Instruccion"),
                DATABASES = new NonTerminal("DATABASES"),
                DATABASES2 = new NonTerminal("DATABASES2"),
                DATABASE = new NonTerminal("DATABASE"),
                DATABASE2 = new NonTerminal("DATABASE2"),
                DATABASE3 = new NonTerminal("DATABASE3"),
                NAME = new NonTerminal("NAME"),
                DATA = new NonTerminal("DATA"),
                VALOR = new NonTerminal("VALOR"),
                DATA2 = new NonTerminal("DATA2"),
                DATA3 = new NonTerminal("DATA3"),
                DATA4 = new NonTerminal("DATA4"),
                DATA5 = new NonTerminal("DATA5"),
                TABLAS = new NonTerminal("TABLAS"),
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
                IN_OUT = new NonTerminal("IN_OUT"),
                USER = new NonTerminal("USER"),
                USERS = new NonTerminal("USERS"),
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
            S.Rule = Instrucciones;

            Instrucciones.Rule = DOLLAR + MENQUE + Instrucciones2 + MAYQUE + DOLLAR;

            Instrucciones2.Rule = Instrucciones2 + COMA + Instruccion
                | Instruccion;

            Instruccion.Rule = DATABASES
                | USERS;

            DATABASES.Rule = RDATABASES + IGUAL + CORIZQ + DATABASES2 + CORDER;

            DATABASES2.Rule = DATABASES2 + COMA + DATABASE
                | DATABASE
                | Empty;

            DATABASE.Rule = MENQUE + DATABASE2 + MAYQUE;

            DATABASE2.Rule = DATABASE2 + COMA + DATABASE3
                | DATABASE3;

            DATABASE3.Rule = NAME
                | DATA;

            NAME.Rule = RNAME + IGUAL + VALOR;

            DATA.Rule = RDATA + IGUAL + CORIZQ + DATA5 + CORDER;


            DATA5.Rule = DATA5 + COMA + DATA2
                | DATA2;

            DATA2.Rule = MENQUE + DATA3 + MAYQUE
                | DOLLAR + LLAVIZQ + VALOR + PUNTO + VALOR + LLAVDER + DOLLAR
                | Empty;

            DATA3.Rule = DATA3 + COMA + DATA4
                | DATA4 ;

            DATA4.Rule = TABLA
                | OBJETO
                | PROCEDURE;

            TABLA.Rule = CQL_TYPE
                | NAME
                | COLUMNS
                | DATA_DATA;


            CQL_TYPE.Rule = RCQL_TYPE + IGUAL + VALOR;

            COLUMNS.Rule = RCOLUMNS + IGUAL + CORIZQ + COLUMNS4 + CORDER;

            COLUMNS4.Rule = COLUMNS4 + COMA + COLUMNS2
                | COLUMNS2;

            COLUMNS2.Rule = MENQUE + COLUMNS3 + MAYQUE
                | Empty;
         

            COLUMNS3.Rule = COLUMNS3 + COMA + COLUMN
                | COLUMN;
         

            COLUMN.Rule = NAME
                | TYPE
                | PK;

            TYPE.Rule = RTYPE + IGUAL + VALOR;

            PK.Rule = RPK + IGUAL + BOOL;

            DATA_DATA.Rule = RDATA + IGUAL + CORIZQ + DATA_DATA2 + CORDER;

            DATA_DATA2.Rule = DOLLAR + LLAVIZQ + VALOR + PUNTO + VALOR + LLAVDER + DOLLAR
                | DATA_DATA3
                | Empty;

            DATA_DATA3.Rule = DATA_DATA3 + COMA + DATA_DATA4
                | DATA_DATA4;

            DATA_DATA4.Rule = MENQUE+ DATA_DATA5 + MAYQUE;

            DATA_DATA5.Rule = DATA_DATA5 + COMA + DATA_DATA6
                | DATA_DATA6;

            DATA_DATA6.Rule = VALOR + IGUAL + VALOR;

            OBJETO.Rule = CQL_TYPE
                | NAME
                | ATTRIBUTES;
            ATTRIBUTES.Rule = RATTRS + IGUAL + CORIZQ + ATTRIBUTES2 + CORDER;

            ATTRIBUTES2.Rule = ATTRIBUTES2 + COMA + ATTRIBUTES3
                | ATTRIBUTES3
                | Empty;

            ATTRIBUTES3.Rule = MENQUE + ATTRIBUTES4 + MAYQUE;

            ATTRIBUTES4.Rule = ATTRIBUTES4 + COMA + ATTRIBUTE
                | ATTRIBUTE;

            ATTRIBUTE.Rule = NAME
                | TYPE;

            PROCEDURE.Rule = CQL_TYPE
                | NAME
                | PARAMETERS
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
                | NUMERO;

            USERS.Rule = RUSERS + IGUAL + CORIZQ + USERS2 + CORDER;

            USERS2.Rule = USERS2 + COMA + USERS3
                | USERS3
                | Empty;

            USERS3.Rule = MENQUE + USERS4 + MAYQUE;

            USERS4.Rule = USERS4 + COMA + USER
                | USER;

            USER.Rule = NAME
                | PASSWORD
                | PERMISSIONS;

            PASSWORD.Rule = RPASSWORD + IGUAL + VALOR;

            PERMISSIONS.Rule = RPERMISSIONS + IGUAL + CORIZQ + PERMISSIONS2 + CORDER;

            PERMISSIONS2.Rule = PERMISSIONS2 + COMA + PERMISSION
                | PERMISSION
                | Empty;

            PERMISSION.Rule = MENQUE + NAME + MAYQUE;

            #endregion


            #region Preferencias
            this.Root = S;
            #endregion

        }
    }

}
