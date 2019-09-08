using Servidor.NOSQL.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Servidor.Models.FCL
{
    public class Sentencia_For : Instruccion
    {
        Variable variable;
        Operacion expresion;
        Operacion actualizacion;
        LinkedList<Instruccion> instruccions;
        int line, column;
        List<string> salida = new List<string>();

        public Sentencia_For(Variable variable, Operacion expresion, Operacion actualizacion, LinkedList<Instruccion> instruccions, int line, int column)
        {
            this.variable = variable;
            this.expresion = expresion;
            this.actualizacion = actualizacion;
            this.instruccions = instruccions;
            this.line = line;
            this.column = column;
        }
        public void clearSalida()
        {
            this.salida.Clear();
        }

        public object Ejecutar(TablaDeSimbolos ts)
        {

            TablaDeSimbolos tabla_local = new TablaDeSimbolos();
            tabla_local.addPadre(ts);
            if (variable.Asignacion)
            {
                Tipo type = ts.getType(variable.Id);
                Operacion val = (Operacion)variable.Valor;
                object valor = val.Ejecutar(ts);
                salida.AddRange(val.getSalida());
                if (valor != null)
                {
                    if (Program.casteos.comprobarCasteo(type, valor))
                    {
                        ts.setValor(variable.Id, valor);
                    }
                    else salida.Add(Program.buildError(getLine(), getColumn(), "Semantico", "No se puede convertir a" + ts.tipoAsignado(variable.Id)));
                }
            }
            else
            {
                Simbolo new_simbol = new Simbolo(Program.getTipo(variable.Tipo.ToLower()), variable.Id);
                new_simbol.Sub_tipo = Tipo.VARIABLE;
                Operacion valor = (Operacion)variable.Valor;
                try
                {
                    new_simbol.Valor = valor.Ejecutar(ts);
                    if (new_simbol.Valor == null) new_simbol.Valor = variable.Valor;
                }
                catch (Exception)
                {
                    new_simbol.Valor = valor;
                }
                tabla_local.AddLast(new_simbol);
            }

            while ((bool)expresion.Ejecutar(tabla_local))
            {

                foreach (Instruccion item in instruccions)
                {
                    
                    if (item.getType() == Tipo.BREAK) return null;
                    if (item.getType() == Tipo.USER_TYPES || item.getType() == Tipo.FUNCION || item.getType() == Tipo.METODO || item.getType() == Tipo.PROCEDURE)
                    {
                        salida.Add(Program.buildError(item.getLine(), item.getColumn(), "Semantico", "No puede venir instruccion del tipo: " + item.getType() + " en un ambito local."));
                    }
                    else
                    {
                        item.Ejecutar(tabla_local);

                        salida.AddRange(item.getSalida());
                        item.clearSalida();
                    }
                }
                actualizacion.Ejecutar(tabla_local);
            }
            return null;
        }

        public int getColumn()
        {
            return this.column;
        }

        public int getLine()
        {
            return this.line;
        }

        public List<string> getSalida()
        {
            return this.salida;
        }

        public Tipo getType()
        {
            return Tipo.FOR;
        }

        public object Recolectar(TablaDeSimbolos ts)
        {
            return null;
        }
    }
}
