/*
*Universidad de San Carlos de Guatemala
*Falcultad de Ingenieria
*Compiladores 2
*Gramatica de interprete de paquetes LUP (Lenguaje unificado de paquetes)
*Hecho por Carlos Andree Avalos Soto 
*Registro estudiantil 201408580
*/


/* Definicion de Analisis Lexico */
%lex

%options case-insensitive

%%

\s+											// se ignoran espacios en blanco
"//".*										// comentario simple línea
[/][*][^*]*[*]+([^/*][^*]*[*]+)*[/]			// comentario multiple líneas

"LOGIN"         return 'RLOGIN';
"SUCCESS"       return 'RSUCCESS';
"FAIL"          return 'RFAIL';
"LOGOUT"        return 'RLOGOUT';
"USER"          return 'RUSER'
"PASS"          return 'RPASS';
"QUERY"         return 'RQUERY';
"DATA"          return 'RDATA';
"MESSAGE"       return 'RMESSAGE';
"ERROR"         return 'RERROR';
"LINE"          return 'RLINE';
"COLUMN"        return 'RCOLUMN';
"TYPE"          return 'RTYPE';
"DESC"          return 'RDESC';
"STRUC"         return 'RSTRUC';
"DATABASES"     return 'RDATABASES';
"DATABASE"      return 'RDATABASE';
"NAME"          return 'RNAME';
"TABLES"        return 'RTABLES';
"TABLE"         return 'RTABLE';
"COLUMNS"       return 'RCOLUMNS';
"TYPES"         return 'RTYPES';
"TYPE"          return 'RTYPE';
"ATTRIBUTES"    return 'RATTRIBUTES';
"PROCEDURES"    return 'RPROCEDURES';


"["                 return 'CORIZQ';
"]"                 return 'CORDER';
"+"                 return 'MAS';
"-"                 return 'MENOS';
\"[^\"]*\"				{ yytext = yytext.substr(1,yyleng-2); return 'CADENA'; }
[0-9]+("."[0-9]+)?\b  	return 'DECIMAL';
[0-9]+\b				return 'ENTERO';
([a-zA-Z])[a-zA-Z0-9_]*	return 'IDENTIFICADOR';



<<EOF>>				return 'EOF';
.					{ console.error('Este es un error léxico: ' + yytext + ', en la linea: ' + yylloc.first_line + ', en la columna: ' + yylloc.first_column); }


/lex

%start S

%% /* Definicion de analisis sintactico */

S
    : Instrucciones EOF
;

Instrucciones
    : Instrucciones Instruccion
    | Instruccion
    | error 
;

Instruccion
    : LOGIN
    | LOGOUT
    | QUERY
    | DATA
    | MESSAGE
    | ERROR
    | STRUC
    | DATABASES
;

LOGIN
    : CORIZQ MAS RLOGIN CORDER USER PASSWORD CORIZQ MENOS RLOGIN CORDER
    | CORIZQ MAS RLOGIN CORDER RESPUESTA CORIZQ MENOS RLOGIN CORDER
;

USER
    : CORIZQ MAS RUSER CORDER VALOR CORIZQ MENOS RUSER CORDER 
;

PASSWORD
    : CORIZQ MAS RPASS CORDER VALOR CORIZQ MENOS RPASS CORDER 
;

VALOR
    : CADENA
    | IDENTIFICADOR
    | ENTERO
    | DECIMAL
;

RESPUESTA
    : CORIZQ RSUCCESS CORDER
    |  CORIZQ RFAIL CORDER
;

LOGOUT
    : CORIZQ MAS RLOGOUT CORDER RESPUESTA CORIZQ MENOS RLOGOUT CORDER
    | CORIZQ MAS RLOGOUT CORDER USER CORIZQ MENOS RLOGOUT CORDER
;

QUERY
    : CORIZQ MAS RQUERY CORDER USER DATA CORIZQ MENOS RQUERY CORDER
;

DATA
    : CORIZQ MAS RDATA CORDER VALOR CORIZQ MENOS RDATA CORDER
;

MESSAGE
    : CORIZQ MAS RMESSAGE CORDER VALOR CORIZQ MENOS RMESSAGE CORDER
;

ERROR
    : CORIZQ MAS RERROR CORDER LINEA COLUMNA TIPO DESCRIPCION CORIZQ MENOS RERROR CORDER
; 

LINEA
    : CORIZQ MAS RLINE CORDER VALOR CORIZQ MENOS RLINE CORDER
;
COLUMNA
    : CORIZQ MAS RCOLUMN CORDER VALOR CORIZQ MENOS RCOLUMN CORDER 
;
TIPO
    : CORIZQ MAS RTYPE CORDER VALOR CORIZQ MENOS RTYPE CORDER 
;
DESCRIPCION
    : CORIZQ MAS RDESC CORDER VALOR CORIZQ MENOS RDESC CORDER 
;

STRUC
    : CORIZQ MAS RSTRUC CORDER USER CORIZQ MENOS RSTRUC CORDER
;

DATABASES
    : CORIZQ MAS RDATABASES CORDER DATABASES2 CORIZQ MENOS RDATABASES CORDER
;

DATABASES2
    : DATABASES2 DATABASE
    | DATABASE
;

DATABASE
    : CORIZQ MAS RDATABASE CORDER NAME TABLES TYPES PROCEDURES CORIZQ MENOS RDATABASE CORDER
;

NAME
    : CORIZQ MAS RNAME CORDER VALOR CORIZQ MENOS RNAME CORDER 
;

TABLES
    : CORIZQ MAS RTABLES CORDER TABLES2 CORIZQ MENOS RTABLES CORDER
;

TABLES2
    : TABLES2 TABLE
    | TABLE
;

TABLE
    : CORIZQ MAS RTABLE CORDER NAME COLUMNS CORIZQ MENOS RTABLE CORDER
    | CORIZQ MAS RTABLE CORDER VALOR CORIZQ MENOS RTABLE CORDER
;

COLUMNS
    : COLUMNS COLUMN
    | COLUMN
;

COLUMN
    : CORIZQ MAS RCOLUMNS CORDER VALOR CORIZQ MENOS RCOLUMNS CORDER
;

TYPES
    : CORIZQ MAS RTYPES CORDER TYPES2 CORIZQ MENOS RTYPES CORDER
;

TYPES2
    : TYPES2 TYPE
    | TYPE
;

TYPE
    : CORIZQ MAS RTYPE CORDER NAME ATTRIBUTES CORIZQ MENOS RTYPE CORDER
    | CORIZQ MAS RTYPE CORDER VALOR CORIZQ MENOS RTYPE CORDER
;

ATTRIBUTES
    : ATTRIBUTES ATTRIBUTE
    | ATTRIBUTE
;

ATTRIBUTE
    : CORIZQ MAS RATTRIBUTES CORDER VALOR CORIZQ MENOS RATTRIBUTES CORDER
;

PROCEDURES
    : CORIZQ MAS RPROCEDURES CORDER VALOR CORIZQ MENOS RPROCEDURES CORDER
;