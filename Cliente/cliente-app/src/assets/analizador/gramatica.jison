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
"ATTRIBUTE"    return 'RATTRIBUTE';
"PROCEDURES"    return 'RPROCEDURES';
"PROCEDURE"    return 'RPROCEDURE';


"["                 return 'CORIZQ';
"]"                 return 'CORDER';
"+"                 return 'MAS';
"-"                 return 'MENOS';
\"(\\.|[^\\"])*\"				{ yytext = yytext.substr(1,yyleng-2); return 'CADENA'; }
[0-9]+("."[0-9]+)?\b  	return 'DECIMAL';
[0-9]+\b				return 'ENTERO';
([a-zA-Z])[a-zA-Z0-9_]*	return 'IDENTIFICADOR';



<<EOF>>				return 'EOF';
.					{ console.error('Este es un error léxico: ' + yytext + ', en la linea: ' + yylloc.first_line + ', en la columna: ' + yylloc.first_column); }


/lex


%{
	const TIPO_INSTRUCCION	    = require('./instrucciones').TIPO_INSTRUCCION;
	const TIPO_VALOR 		    = require('./instrucciones').TIPO_VALOR;
	const OBJETO_INSTRUCCION    = require('./instrucciones').OBJETO_INSTRUCCION;
    const OBJETO_DATABASE    = require('./instrucciones').OBJETO_DATABASE;
%}

%start S

%% /* Definicion de analisis sintactico */

S
    : Instrucciones EOF {return $1}
;

Instrucciones
    : Instrucciones Instruccion { $1.push($2); $$ = $1;}
    | Instruccion {$$ = [$1];} 
;

Instruccion
    : LOGIN     {$$ = $1;}
    | LOGOUT    {$$ = $1;}
    | QUERY
    | DATA      {$$ = $1;}
    | MESSAGE   {$$ = $1;}
    | ERROR     {$$ = $1;}
    | STRUC
    | DATABASES {$$ = $1;}
    | error { console.error('Este es un error sintáctico: ' + yytext + ', en la linea: ' + this._$.first_line + ', en la columna: ' + this._$.first_column); }
;

LOGIN
    : CORIZQ MAS RLOGIN CORDER USER PASSWORD CORIZQ MENOS RLOGIN CORDER
    | CORIZQ MAS RLOGIN CORDER RESPUESTA CORIZQ MENOS RLOGIN CORDER                 {$$ = OBJETO_INSTRUCCION.nuevoLogin($5);}
;

USER
    : CORIZQ MAS RUSER CORDER VALOR CORIZQ MENOS RUSER CORDER 
;

PASSWORD
    : CORIZQ MAS RPASS CORDER VALOR CORIZQ MENOS RPASS CORDER 
;

VALOR
    : CADENA                                                                        {$$ = OBJETO_INSTRUCCION.nuevoValor($1,TIPO_VALOR.CADENA);}
    | IDENTIFICADOR                                                                 {$$ = OBJETO_INSTRUCCION.nuevoValor($1,TIPO_VALOR.IDENTIFICADOR);}
    | ENTERO                                                                        {$$ = OBJETO_INSTRUCCION.nuevoValor(Number($1),TIPO_VALOR.NUMERO);}
    | DECIMAL                                                                       {$$ = OBJETO_INSTRUCCION.nuevoValor(Number($1),TIPO_VALOR.NUMERO);}
;

RESPUESTA
    : CORIZQ RSUCCESS CORDER                                                        {$$ = "true"}
    |  CORIZQ RFAIL CORDER                                                          {$$ = "false"}
;

LOGOUT
    : CORIZQ MAS RLOGOUT CORDER RESPUESTA CORIZQ MENOS RLOGOUT CORDER               {$$ = OBJETO_INSTRUCCION.nuevoLogout($5);}
    | CORIZQ MAS RLOGOUT CORDER USER CORIZQ MENOS RLOGOUT CORDER {$$ = null} 
;

QUERY
    : CORIZQ MAS RQUERY CORDER USER DATA CORIZQ MENOS RQUERY CORDER
;

DATA
    : CORIZQ MAS RDATA CORDER VALOR CORIZQ MENOS RDATA CORDER                       {$$ = OBJETO_INSTRUCCION.nuevoData($5);}
    | CORIZQ MAS RDATA CORDER CORIZQ MENOS RDATA CORDER  {$$ = null}
;

MESSAGE
    : CORIZQ MAS RMESSAGE CORDER VALORES CORIZQ MENOS RMESSAGE CORDER                 {$$ = OBJETO_INSTRUCCION.nuevoMessage($5);}
    | CORIZQ MAS RMESSAGE CORDER CORIZQ MENOS RMESSAGE CORDER {$$ = null}
;

ERROR
    : CORIZQ MAS RERROR CORDER LINEA COLUMNA TIPO DESCRIPCION CORIZQ MENOS RERROR CORDER    {$$ = OBJETO_INSTRUCCION.nuevoError($5,$6,$7,$8);}
; 

LINEA
    : CORIZQ MAS RLINE CORDER VALOR CORIZQ MENOS RLINE CORDER                   {$$ = $5;}
;
COLUMNA
    : CORIZQ MAS RCOLUMN CORDER VALOR CORIZQ MENOS RCOLUMN CORDER               {$$ = $5;}
;
TIPO
    : CORIZQ MAS RTYPE CORDER VALOR CORIZQ MENOS RTYPE CORDER                   {$$ = $5;}
;
DESCRIPCION
    : CORIZQ MAS RDESC CORDER VALOR CORIZQ MENOS RDESC CORDER                   {$$ = $5;}
;

STRUC
    : CORIZQ MAS RSTRUC CORDER USER CORIZQ MENOS RSTRUC CORDER                 
;

DATABASES
    : CORIZQ MAS RDATABASES CORDER DATABASES2 CORIZQ MENOS RDATABASES CORDER    {$$ = OBJETO_INSTRUCCION.nuevoDatabases($5);}
    | CORIZQ MAS RDATABASES CORDER CORIZQ MENOS RDATABASES CORDER {$$ = null}
;

DATABASES2
    : DATABASES2 DATABASE                                                       {$1.push($2); $$ = $1;}
    | DATABASE                                                                  { $$ = [$1];}
;

DATABASE
    : CORIZQ MAS RDATABASE CORDER NAME TABLES TYPES PROCEDURES CORIZQ MENOS RDATABASE CORDER  {$$ = OBJETO_INSTRUCCION.nuevoDatabase($5,$6,$7,$8);}
;

NAME
    : CORIZQ MAS RNAME CORDER VALOR CORIZQ MENOS RNAME CORDER                   {$$ = $5;}
;

TABLES
    : CORIZQ MAS RTABLES CORDER TABLES2 CORIZQ MENOS RTABLES CORDER             {$$ = $5;}
    | CORIZQ MAS RTABLES CORDER CORIZQ MENOS RTABLES CORDER {$$ = null}
;

TABLES2
    : TABLES2 TABLE                                                             {$1.push($2); $$ = $1;}
    | TABLE                                                                     { $$ = [$1];}
;

TABLE
    : CORIZQ MAS RTABLE CORDER NAME COLUMNS CORIZQ MENOS RTABLE CORDER          {$$ = OBJETO_INSTRUCCION.nuevoTable($5,$6);}
    | CORIZQ MAS RTABLE CORDER VALORES CORIZQ MENOS RTABLE CORDER               {$$ = OBJETO_INSTRUCCION.nuevoTable2($5);}
;


COLUMNS
	: CORIZQ MAS RCOLUMNS CORDER COLUMNS2 CORIZQ MENOS RCOLUMNS CORDER			{ $$ = $5;}
    | CORIZQ MAS RCOLUMNS CORDER CORIZQ MENOS RCOLUMNS CORDER {$$ = null}
;	

COLUMNS2
    : COLUMNS2 COLUMN                                                            {$1.push($2); $$ = $1;}
    | COLUMN                                                                    { $$ = [$1];}
;

COLUMN
    : CORIZQ MAS RCOLUMN CORDER NAME CORIZQ MENOS RCOLUMN CORDER          { $$ = $5;}
;


TYPES
    : CORIZQ MAS RTYPES CORDER TYPES2 CORIZQ MENOS RTYPES CORDER               { $$ = $5;}
    | CORIZQ MAS RTYPES CORDER CORIZQ MENOS RTYPES CORDER {$$ = null}
;

TYPES2
    : TYPES2 TYPE {$1.push($2); $$ = $1;}
    | TYPE  { $$ = [$1];}
;

TYPE
    : CORIZQ MAS RTYPE CORDER NAME ATTRIBUTES CORIZQ MENOS RTYPE CORDER         {$$ = OBJETO_INSTRUCCION.nuevoType($5,$6);}
    | CORIZQ MAS RTYPE CORDER VALORES CORIZQ MENOS RTYPE CORDER                 {$$ = OBJETO_INSTRUCCION.nuevoType2($5);}
;

ATTRIBUTES
    : CORIZQ MAS RATTRIBUTES CORDER ATTRIBUTES2 CORIZQ MENOS RATTRIBUTES CORDER { $$ = $5;}
    | CORIZQ MAS RATTRIBUTES CORDER CORIZQ MENOS RATTRIBUTES CORDER {$$ = null}
;

ATTRIBUTES2
    : ATTRIBUTES2 ATTRIBUTE  {$1.push($2); $$ = $1;}
    | ATTRIBUTE { $$ = [$1];}
;

ATTRIBUTE
    : CORIZQ MAS RATTRIBUTE CORDER NAME CORIZQ MENOS RATTRIBUTE CORDER     { $$ = $5;}
;

PROCEDURES
    : CORIZQ MAS RPROCEDURES CORDER PROCEDURES2 CORIZQ MENOS RPROCEDURES CORDER     { $$ = $5;}
    | CORIZQ MAS RPROCEDURES CORDER CORIZQ MENOS RPROCEDURES CORDER {$$ = null}
;

PROCEDURES2
	: PROCEDURES2 PROCEDURE {$1.push($2); $$ = $1;}
	| PROCEDURE { $$ = [$1];}
;

PROCEDURE
	: CORIZQ MAS RPROCEDURE CORDER NAME CORIZQ MENOS RPROCEDURE CORDER { $$ = $5;}
;

VALORES
    : VALORES VALOR {$1.push($2); $$ = $1;}
    |VALOR   { $$ = [$1];}
;