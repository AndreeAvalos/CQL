var fs = require('fs');
var parser = require('./gramatica');

const TIPO_INSTRUCCION	    = require('./instrucciones').TIPO_INSTRUCCION;
const TIPO_VALOR 		    = require('./instrucciones').TIPO_VALOR;
const OBJETO_INSTRUCCION    = require('./instrucciones').OBJETO_INSTRUCCION;
const OBJETO_DATABASE    = require('./instrucciones').OBJETO_DATABASE;


let ast;

try{
    const entrada = fs.readFileSync('./entrada.txt');
    ast = parser.parse(entrada.toString());

}catch(e){
    console.error(e);
    return;
}

procesarBloque(ast);

function procesarBloque(instrucciones){
    instrucciones.forEach(instruccion => {
        if(instruccion.tipo === TIPO_INSTRUCCION.LOGIN) procesarLogin(instruccion);
        else if(instruccion.tipo === TIPO_INSTRUCCION.LOGOUT) procesarLogout(instruccion);
        else if(instruccion.tipo === TIPO_INSTRUCCION.DATA) procesarData(instruccion);
        else if(instruccion.tipo === TIPO_INSTRUCCION.MESSAGE) procesarMessage(instruccion);
        else if(instruccion.tipo === TIPO_INSTRUCCION.ERROR) procesarError(instruccion);
        else if(instruccion.tipo === TIPO_INSTRUCCION.DATABASES) procesarDatabases(instruccion);
        else throw 'ERROR: tipo de instrucción no válido: ' + instruccion;
    });
}

function procesarLogin(expresion){
    console.log(expresion)
    if(expresion.valor === "true") return true;
    else return false;
}

function procesarLogout(expresion){
    console.log(expresion)
    if(expresion.valor === "true") return true;
    else return false;
}

function procesarData(expresion){
    console.log(expresion);
}

function procesarMessage(expresion){
    console.log(expresion);
}
function procesarError(expresion){
    console.log(expresion);
}
function procesarDatabases(expresion){
    console.log(expresion);
}