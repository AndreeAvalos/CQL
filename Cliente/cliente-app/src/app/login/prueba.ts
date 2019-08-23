import { Injectable } from '@angular/core';
import { parser } from 'src/assets/analizador/gramatica';

@Injectable()
export class PruebaEntrada{

    ast:any;
    constructor(){
        
    }
    imprimirAlgo(text:string): any{
        //console.log(text);

            this.ast = parser.parse(text.toString());
            return this.procesarBloque(this.ast);

    }

    procesarBloque(instrucciones:any): any{
        var actual;
         instrucciones.forEach(instruccion => {
            if(instruccion.tipo === 'INSTR_LOGIN'){
                actual =  this.procesarLogin(instruccion);
                return;
            }
        });
        return actual;
    }

    procesarLogin(expresion:any): any{
        
        if(expresion.valor === "true")
        {
            console.log(expresion);
            return true;
        }
        else return false;
    }
}