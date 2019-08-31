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
                RARROBA = ToTerm("@"),
                RMAS = ToTerm("+"),
                RPOTENCIA = ToTerm("**"),
                RMENOS = ToTerm("-"),
                RINCREMENTO = ToTerm("++"),
                RDECREMENTO = ToTerm("--"),
                RMUL = ToTerm("*"),
                RDIV = ToTerm("/"),
                RIF = ToTerm("IF"),
                RELSE = ToTerm("ELSE"),
                RMODULAR = ToTerm("%"),
                MENQUE = ToTerm("<"),
                MAYQUE = ToTerm(">"),
                MAYIGUAL = ToTerm(">="),
                MENIGUAL = ToTerm("<="),
                IGUALIGUAL = ToTerm("=="),
                RDIFERENTE = ToTerm("!="),
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
                RCOMMIT = ToTerm("COMMIT"),
                RTYPE = ToTerm("TYPE"),
                RROLLBACK = ToTerm("ROLLBACK"),
                TSTRING = ToTerm("string"),
                TINT = ToTerm("int"),
                TDOUBLE = ToTerm("double"),
                TDATE = ToTerm("date"),
                TTIME = ToTerm("time"),
                TBOOLEAN = ToTerm("boolean"),
                TCOUNTER = ToTerm("counter"),
                TMAP = ToTerm("Map"),
                TSET = ToTerm("Set"),
                TLIST = ToTerm("List"),
                RUSER = ToTerm("USER"),
                RDELETE = ToTerm("DELETE"),
                RGRANT = ToTerm("GRANT"),
                RREVOKE = ToTerm("REVOKE"),
                RON = ToTerm("ON"),
                RWITH = ToTerm("WITH"),
                RPASSWORD = ToTerm("PASSWORD"),
                RINSERT = ToTerm("INSERT INTO"),
                RVALUES = ToTerm("VALUES"),
                RNEW = ToTerm("NEW"),
                RAS = ToTerm("AS"),
                RLOG = ToTerm("LOG")
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
                TRUNCATE_TABLE = new NonTerminal("TRUNCATE_TABLE"),
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
                IFC_PK = new NonTerminal("IFC_PK"),
                TCL = new NonTerminal("TCL"),
                DCL = new NonTerminal("DCL"),
                FCL = new NonTerminal("FCL"),
                USER_TYPE = new NonTerminal("USER_TYPE"),
                CREATE_TYPE = new NonTerminal("CREATE_TYPE"),
                USER_CONTENT = new NonTerminal("USER_CONTENT"),
                USER_CONTENT2 = new NonTerminal("USER_CONTENT2"),
                ALTER_TYPE = new NonTerminal("ALTER_TYPE"),
                DELETE_TYPE = new NonTerminal("DELETE_TYPE"),
                CREATE_USER = new NonTerminal("CREATE_USER"),
                GRANT = new NonTerminal("GRANT"),
                REVOKE = new NonTerminal("REVOKE"),
                SET = new NonTerminal("SET"),
                MAP = new NonTerminal("MAP"),
                LISTA = new NonTerminal("LISTA"),
                ASIGNACION = new NonTerminal("ASIGNACION"),
                VARIABLES = new NonTerminal("VARIABLES"),
                VARIABLE = new NonTerminal("VARIABLE"),
                INICIALIZACION = new NonTerminal("INICIALIZACION"),
                LOG = new NonTerminal("LOG"),
                CADENAS = new NonTerminal("CADENAS"),
                PCADENA = new NonTerminal("PCADENA"),
                OPERACION_NUMERICA = new NonTerminal("OPERACION_NUMERICA"),
                SENTENCIA_IF = new NonTerminal("SENTENCIA_IF"),
                SENTENCIA_ELSE = new NonTerminal("SENTENCIA_ELSE"),
                SENTENCIA_ELSE_IF = new NonTerminal("SENTENCIA_ELSE_IF"),
                SENTENCIA_ELSE_IF2 = new NonTerminal("SENTENCIA_ELSE_IF2"),
                EXPRESION_LOGICA = new NonTerminal("EXPRESION_LOGICA"),
                VALORES_LOGICOS = new NonTerminal("VALORES_LOGICOS")
                ;


            #endregion

            #region Gramatica
            S.Rule = Instrucciones;
            S.ErrorRule = SyntaxError + CORIZQ;

            Instrucciones.Rule = Instrucciones + Instruccion
                | Instruccion
                | Empty;

            Instruccion.Rule = USER_TYPE
                | DDL
                | TCL
                | DCL
                | FCL;
            Instruccion.ErrorRule = SyntaxError + PTCOMA;

            USER_TYPE.Rule = CREATE_TYPE
                | ALTER_TYPE
                | DELETE_TYPE;

            DDL.Rule = CREATE_DB
                | PUSE
                | DROP_DB
                | CREATE_TABLE
                | ALTER_TABLE
                | DROP_TABLE
                | TRUNCATE_TABLE;

            DCL.Rule = CREATE_USER
                | GRANT
                | REVOKE;

            FCL.Rule = ASIGNACION
                | VARIABLE + PTCOMA
                | LOG
                | SENTENCIA_IF;

            #region USER TYPES
            //CREATE TYPE IF NOT EXISTS PRUEBA (-);
            CREATE_TYPE.Rule = RCREATE + RTYPE + IFNE + IDENTIFICADOR + PARIZQ + USER_CONTENT + PARDER + PTCOMA;
            ///RECURSIVIDAD
            USER_CONTENT.Rule = USER_CONTENT + COMA + USER_CONTENT2
                | USER_CONTENT2;
            //CUI INT
            USER_CONTENT2.Rule = IDENTIFICADOR + TIPO_DATO;

            //ALTER TYPE ESTUDIANTE DELETE/ADD(-);
            ALTER_TYPE.Rule = RALTER + RTYPE + IDENTIFICADOR + RADD + PARIZQ + USER_CONTENT + PARDER + PTCOMA
                | RALTER + RTYPE + IDENTIFICADOR + RDELETE + PARIZQ + VALORES + PARDER + PTCOMA;


            DELETE_TYPE.Rule = RDELETE + RTYPE + IDENTIFICADOR + PTCOMA;
            #endregion

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

            #region TCL
            TCL.Rule = RCOMMIT + PTCOMA
                | RROLLBACK + PTCOMA;
            #endregion

            #region DCL
            CREATE_USER.Rule = RCREATE + RUSER + IDENTIFICADOR + RWITH + RPASSWORD + CADENA + PTCOMA;

            GRANT.Rule = RGRANT + IDENTIFICADOR + RON + IDENTIFICADOR + PTCOMA;

            REVOKE.Rule = RREVOKE + IDENTIFICADOR + RON + IDENTIFICADOR + PTCOMA;

            #endregion

            #region FCL

            ASIGNACION.Rule = TIPO_DATO + VARIABLES + PTCOMA;

            VARIABLES.Rule = VARIABLES + COMA + VARIABLE
                | VARIABLE;

            VARIABLE.Rule = RARROBA + IDENTIFICADOR + INICIALIZACION
                | RARROBA + IDENTIFICADOR + RINCREMENTO
                | RARROBA + IDENTIFICADOR + RDECREMENTO
                | RARROBA + IDENTIFICADOR + RMAS + IGUAL + NUMERO//@id+=10
                | RARROBA + IDENTIFICADOR + RMENOS + IGUAL + NUMERO//@id-=10
                | RARROBA + IDENTIFICADOR + RMUL + IGUAL + NUMERO//@id*=10
                | RARROBA + IDENTIFICADOR + RDIV + IGUAL + NUMERO;//@id/=10

            INICIALIZACION.Rule = IGUAL + OPERACION_NUMERICA
                | IGUAL + RNEW + IDENTIFICADOR
                | IGUAL + RNEW + MAP
                | IGUAL + RNEW + SET
                | IGUAL + RNEW + LISTA
                | IGUAL + CORIZQ + VALORES + CORDER + RAS + IDENTIFICADOR
                | IGUAL + CORIZQ + VALORES + CORDER
                | Empty;

            LOG.Rule = RLOG + PARIZQ + CADENAS + PARDER + PTCOMA;

            CADENAS.Rule = CADENAS + RMAS + PCADENA
                | PCADENA;

            PCADENA.Rule = CADENA
                | RARROBA + IDENTIFICADOR
                | RDATE
                | RTIME
                | NUMERO;

            //IF( CONDiCION ) { INSTRUCCIONES }
            SENTENCIA_IF.Rule = RIF + PARIZQ + EXPRESION_LOGICA + PARDER + LLAVIZQ + Instrucciones + LLAVDER + SENTENCIA_ELSE_IF2 + SENTENCIA_ELSE;
            
            // SENTENCIA ELSE IF O ELSE 
            SENTENCIA_ELSE_IF2.Rule = SENTENCIA_ELSE_IF2 + SENTENCIA_ELSE_IF
                | SENTENCIA_ELSE_IF
                | Empty;

            //ELSE IF( CONDICION ){ INSTRUCCIONES }
            SENTENCIA_ELSE_IF.Rule = RELSE + RIF + PARIZQ + EXPRESION_LOGICA + PARDER + LLAVIZQ + Instrucciones + LLAVDER;

            //ELSE { INSTRUCCIONES }
            SENTENCIA_ELSE.Rule = RELSE + LLAVIZQ + Instrucciones + LLAVDER
                | Empty;

            // >,<, <=, >=, ==, !=
            EXPRESION_LOGICA.Rule = OPERACION_NUMERICA + MAYQUE + OPERACION_NUMERICA
                | OPERACION_NUMERICA + MENQUE + OPERACION_NUMERICA
                | OPERACION_NUMERICA + MAYIGUAL + OPERACION_NUMERICA
                | OPERACION_NUMERICA + MENIGUAL + OPERACION_NUMERICA
                | OPERACION_NUMERICA + IGUALIGUAL + OPERACION_NUMERICA
                | OPERACION_NUMERICA + RDIFERENTE + OPERACION_NUMERICA;

            #endregion

            //TIPOS DE OPERACIONES NUMERICAS
            OPERACION_NUMERICA.Rule = RMENOS + OPERACION_NUMERICA//-numero
                | OPERACION_NUMERICA + RMAS + OPERACION_NUMERICA// OPERACION + OPERACION
                | OPERACION_NUMERICA + RMENOS + OPERACION_NUMERICA
                | OPERACION_NUMERICA + RMUL + OPERACION_NUMERICA
                | OPERACION_NUMERICA + RDIV + OPERACION_NUMERICA
                | OPERACION_NUMERICA + RMODULAR + OPERACION_NUMERICA
                | OPERACION_NUMERICA + RPOTENCIA + OPERACION_NUMERICA
                | PARIZQ + OPERACION_NUMERICA + PARDER
                | VALOR//NUMERO
                | RARROBA + IDENTIFICADOR//VARIABLE
                | RARROBA + IDENTIFICADOR + RINCREMENTO//VAR++
                | RARROBA + IDENTIFICADOR + RDECREMENTO;//VAR--

            //TIPOS DE DATOS POR EJEMPLO INT, DOUBLE, STRING, BOOLEAN, ETC.
            TIPO_DATO.Rule = TSTRING
                | SET
                | TTIME
                | TDATE
                | TCOUNTER
                | TDOUBLE
                | TBOOLEAN
                | TINT
                | MAP
                | LISTA
                | IDENTIFICADOR
                | RNULL
                | TMAP
                | TSET
                | TLIST
                ;

            SET.Rule = TSET + MENQUE + TIPO_DATO + MAYQUE;
            MAP.Rule = TMAP + MENQUE + VALOR + COMA + VALOR + MAYQUE;
            LISTA.Rule = TLIST + MENQUE + VALOR + MAYQUE;
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
            //RESERVAR PALABRAS
            string[] palabras = {
                RPRIMARY_KEY.Text,
                RDROP.Text, RADD.Text,
                RIF_NOT_EXISTS.Text,
                RINSERT.Text,
                RCOMMIT.Text,
                RREVOKE.Text,
                RGRANT.Text,
                RCREATE.Text,
                RDROP.Text,
                RWITH.Text,
                RFALSE.Text,
                RTRUE.Text,
                RALTER.Text,
                RUSE.Text,
                RDATABASE.Text,
                RTYPE.Text,
                RDELETE.Text
            };
            MarkReservedWords(palabras);
            //COMENTARIOS
            NonGrammarTerminals.Add(comentarioBloque);
            NonGrammarTerminals.Add(comentarioLinea);
            //PRESEDENCIA

            RegisterOperators(4, Associativity.Left, RMAS, RMENOS);
            RegisterOperators(5, Associativity.Left, RMUL, RDIV, RMODULAR);
            RegisterOperators(6, Associativity.Left, RPOTENCIA);

            //RETORNAR RAIZ
            this.Root = S;

            #endregion


            #endregion

        }
    }
}