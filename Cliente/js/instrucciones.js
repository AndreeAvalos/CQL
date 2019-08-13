const TIPO_VALOR = {
    NUMERO:         'VAL_NUMERO',
	IDENTIFICADOR:  'VAL_IDENTIFICADOR',
	CADENA:         'VAL_CADENA',
}

const TIPO_INSTRUCCION = {
    LOGIN:      'INSTR_LOGIN',
    LOGOUT:     'INSTR_LOGOUT',
    DATA:       'INSTR_DATA',
    MESSAGE:    'INTR_MESSAGE',
    DATABASES:  'INSTR_DATABASES',
    ERROR:      'INSTR_ERROR',
}

const OBJETO_DATABASE = {
    DATABASE:  'OBJECT_DATABASE',
    TABLE:      'OBJECT_TABLE',
    TABLE2:     'OBJECT_TABLE2',
    TYPE:       'OBJECT_TYPE',
    TYPE2:      'OBJECT_TYPE2'

}

const OBJETO_INSTRUCCION = {
    nuevoValor: function(valor, tipo){
        return{
            tipo:tipo,
            valor:valor
        }
    },

    nuevoLogin: function(respuesta){
        return{
            tipo: TIPO_INSTRUCCION.LOGIN,
            valor:respuesta
        }
    },

    nuevoLogout: function(respuesta){
        return{
            tipo: TIPO_INSTRUCCION.LOGOUT,
            valor: respuesta
        }
    },

    nuevoData: function(respuesta){
        return{
            tipo: TIPO_INSTRUCCION.DATA,
            valor: respuesta
        }
    },

    nuevoError: function(linea, columna, type, descripcion){
        return{
            tipo: TIPO_INSTRUCCION.ERROR,
            line: linea,
            column: columna,
            type: type,
            desc: descripcion
        }
    },

    nuevoMessage: function(mensaje){
        return{
            tipo: TIPO_INSTRUCCION.MESSAGE,
            valor: mensaje
        }
    },
    nuevoDatabases: function(database){
        return{
            tipo: TIPO_INSTRUCCION.DATABASES,
            valor: database
        }

    },

    nuevoDatabase: function(nombre,tablas,tipos,procedimientos){
        return{
            tipo: OBJETO_DATABASE.DATABASE,
            name: nombre,
            tables: tablas,
            types: tipos,
            procedures: procedimientos
        }
    },

    nuevoTable: function(name, columns){
        return{
            tipo: OBJETO_DATABASE.TABLE,
            name: name,
            columns: columns
        }
    },

    nuevoTable2: function(names){
        return{
            tipo: OBJETO_DATABASE.TABLE2,
            name: names
        }
    },

    nuevoType: function(name, atributos){
        return{
            tipo: OBJETO_DATABASE.TYPE,
            name: name,
            attributes: atributos
        }
    },

    nuevoType2: function(names){
        return{
            tipo:OBJETO_DATABASE.TYPE2,
            name: names
        }
    }
}

module.exports.TIPO_INSTRUCCION = TIPO_INSTRUCCION;
module.exports.TIPO_VALOR = TIPO_VALOR;
module.exports.OBJETO_INSTRUCCION = OBJETO_INSTRUCCION;
module.exports.OBJETO_DATABASE = OBJETO_DATABASE