using Servidor.NOSQL.Modelos;
using System;
using System.Collections.Generic;

namespace Servidor.Models.FCL
{
    public class Operacion : Instruccion
    {
        int line, column;
        List<string> salida = new List<string>();
        private Operacion operadorDer;
        private Operacion operadorIzq;
        public Object valor;

        public Operacion(object valor, Tipo tipo, int line, int column)
        {
            this.line = line;
            this.column = column;
            this.valor = valor;
            Tipo = tipo;
        }

        public Tipo Tipo { get; set; }

        public Operacion(Operacion operadorIzq, Operacion operadorDer, Tipo tipo, int line, int column)
        {
            this.Tipo = tipo;
            this.line = line;
            this.column = column;
            this.operadorDer = operadorDer;
            this.operadorIzq = operadorIzq;
        }
        public void clearSalida()
        {
            this.salida.Clear();
        }

        public Operacion(Operacion operadorIzq, Tipo tipo, int line, int column)
        {
            this.Tipo = tipo;
            this.line = line;
            this.column = column;
            this.operadorIzq = operadorIzq;
        }

        public Operacion(object valor, Operacion operadorIzq, Tipo tipo, int line, int column)
        {
            this.Tipo = tipo;
            this.line = line;
            this.column = column;
            this.operadorIzq = operadorIzq;
            this.valor = valor;
        }

        public Operacion(string valor, Tipo tipo, int line, int column)
        {
            this.Tipo = tipo;
            this.line = line;
            this.column = column;
            this.valor = valor;
        }

        public Operacion(object valor, int line, int column)
        {
            this.line = line;
            this.column = column;
            this.valor = valor;
            this.Tipo = Tipo.NUMERO;
        }

        public object Ejecutar(TablaDeSimbolos ts)
        {
            try {
                if (Tipo == Tipo.DIVISION)
                {
                    return (Double)operadorIzq.Ejecutar(ts) / (Double)operadorDer.Ejecutar(ts);
                }
                else if (Tipo == Tipo.MODULAR)
                {
                    return (Double)operadorIzq.Ejecutar(ts) % (Double)operadorDer.Ejecutar(ts);
                }
                else if (Tipo == Tipo.POTENCIA)
                {
                    return (Double)Math.Pow((Double)operadorIzq.Ejecutar(ts), (Double)operadorDer.Ejecutar(ts));
                }
                else if (Tipo == Tipo.MULTIPLICACION)
                {
                    return (Double)operadorIzq.Ejecutar(ts) * (Double)operadorDer.Ejecutar(ts);
                }
                else if (Tipo == Tipo.RESTA)
                {
                    return (Double)operadorIzq.Ejecutar(ts) - (Double)operadorDer.Ejecutar(ts);
                }
                else if (Tipo == Tipo.SUMA)
                {
                    return (Double)operadorIzq.Ejecutar(ts) + (Double)operadorDer.Ejecutar(ts);
                }
                else if (Tipo == Tipo.NEGATIVO)
                {
                    return (Double)operadorIzq.Ejecutar(ts) * -1;
                }
                else if (Tipo == Tipo.NUMERO)
                {
                    return Double.Parse(valor.ToString());
                }
                else if (Tipo == Tipo.VARIABLE)
                {
                    return ts.getValor(valor.ToString());
                }
                else if (Tipo == Tipo.CADENA)
                {
                    return valor.ToString();
                }
                else if (Tipo == Tipo.INCREMENTO)
                {
                    Double aux;
                    if (ts.existID(valor.ToString()))
                    {
                        if (ts.getType(valor.ToString()) == Tipo.ENTERO || ts.getType(valor.ToString()) == Tipo.DECIMAL)
                        {
                            aux = (Double)ts.getValor(valor.ToString());
                            ts.setValor(valor.ToString(), aux + 1);
                            return aux;
                        }
                        else salida.Add(Program.buildError(getLine(), getColumn(), "Semantico", "No es un numero."));
                    }
                    else
                    {
                        salida.Add(Program.buildError(getLine(), getColumn(), "Semantico", valor.ToString() + " No existe en el ambito actual"));
                    }
                    return null;
                }
                else if (Tipo == Tipo.DECREMENTO)
                {
                    Double aux;
                    if (ts.existID(valor.ToString()))
                    {
                        if (ts.getType(valor.ToString()) == Tipo.ENTERO || ts.getType(valor.ToString()) == Tipo.DECIMAL)
                        {
                            aux = (Double)ts.getValor(valor.ToString());
                            ts.setValor(valor.ToString(), aux - 1);
                            return aux;
                        }
                        else salida.Add(Program.buildError(getLine(), getColumn(), "Semantico", "No es un numero."));
                    }
                    else
                    {
                        salida.Add(Program.buildError(getLine(), getColumn(), "Semantico", valor.ToString() + " No existe en el ambito actual"));
                    }
                    return null;
                }
                else if (Tipo == Tipo.MASIGUAL)
                {

                    //(Double)operadorIzq.Ejecutar(ts);
                    if (ts.existID(valor.ToString()))
                    {
                        if (ts.getType(valor.ToString()) == Tipo.ENTERO || ts.getType(valor.ToString()) == Tipo.DECIMAL)
                        {
                            Double aux = (Double)ts.getValor(valor.ToString()) + (Double)operadorIzq.Ejecutar(ts);
                            ts.setValor(valor.ToString(), aux);
                        }
                        else salida.Add(Program.buildError(getLine(), getColumn(), "Semantico", "No es un numero."));
                    }
                    else
                    {
                        salida.Add(Program.buildError(getLine(), getColumn(), "Semantico", valor.ToString() + " No existe en el ambito actual"));
                    }
                    return null;
                }
                else if (Tipo == Tipo.MENOSIGUAL)
                {

                    //(Double)operadorIzq.Ejecutar(ts);
                    if (ts.existID(valor.ToString()))
                    {
                        if (ts.getType(valor.ToString()) == Tipo.ENTERO || ts.getType(valor.ToString()) == Tipo.DECIMAL)
                        {
                            Double aux = (Double)ts.getValor(valor.ToString()) - (Double)operadorIzq.Ejecutar(ts);
                            ts.setValor(valor.ToString(), aux);
                        }
                        else salida.Add(Program.buildError(getLine(), getColumn(), "Semantico", "No es un numero."));
                    }
                    else
                    {
                        salida.Add(Program.buildError(getLine(), getColumn(), "Semantico", valor.ToString() + " No existe en el ambito actual"));
                    }
                    return null;
                }
                else if (Tipo == Tipo.MULIGUAL)
                {

                    //(Double)operadorIzq.Ejecutar(ts);
                    if (ts.existID(valor.ToString()))
                    {
                        if (ts.getType(valor.ToString()) == Tipo.ENTERO || ts.getType(valor.ToString()) == Tipo.DECIMAL)
                        {
                            Double aux = (Double)ts.getValor(valor.ToString()) * (Double)operadorIzq.Ejecutar(ts);
                            ts.setValor(valor.ToString(), aux);
                        }
                        else salida.Add(Program.buildError(getLine(), getColumn(), "Semantico", "No es un numero."));
                    }
                    else
                    {
                        salida.Add(Program.buildError(getLine(), getColumn(), "Semantico", valor.ToString() + " No existe en el ambito actual"));
                    }
                    return null;
                }
                else if (Tipo == Tipo.DIVIGUAL)
                {

                    //(Double)operadorIzq.Ejecutar(ts);
                    if (ts.existID(valor.ToString()))
                    {
                        if (ts.getType(valor.ToString()) == Tipo.ENTERO || ts.getType(valor.ToString()) == Tipo.DECIMAL)
                        {
                            Double aux = (Double)ts.getValor(valor.ToString()) / (Double)operadorIzq.Ejecutar(ts);
                            ts.setValor(valor.ToString(), aux);
                        }
                        else salida.Add(Program.buildError(getLine(), getColumn(), "Semantico", "No es un numero."));
                    }
                    else
                    {
                        salida.Add(Program.buildError(getLine(), getColumn(), "Semantico", valor.ToString() + " No existe en el ambito actual"));
                    }
                    return null;
                }
                else if (Tipo == Tipo.MAYOR_QUE)
                {
                    return ((Double)operadorIzq.Ejecutar(ts)) > ((Double)operadorDer.Ejecutar(ts));
                }
                else if (Tipo == Tipo.MAYOR_IGUAL)
                {
                    return ((Double)operadorIzq.Ejecutar(ts)) >= ((Double)operadorDer.Ejecutar(ts));
                }
                else if (Tipo == Tipo.MENOR_QUE)
                {
                    return ((Double)operadorIzq.Ejecutar(ts)) < ((Double)operadorDer.Ejecutar(ts));
                }
                else if (Tipo == Tipo.MENOR_IGUAL)
                {
                    return ((Double)operadorIzq.Ejecutar(ts)) <= ((Double)operadorDer.Ejecutar(ts));
                }
                else if (Tipo == Tipo.IGUAL)
                {
                    return ((Double)operadorIzq.Ejecutar(ts)) == ((Double)operadorDer.Ejecutar(ts));
                }
                else if (Tipo == Tipo.DIFERENTE)
                {
                    return ((Double)operadorIzq.Ejecutar(ts)) != ((Double)operadorDer.Ejecutar(ts));
                }
                else if (Tipo == Tipo.AND)
                {
                    return ((bool)operadorIzq.Ejecutar(ts)) && ((bool)operadorDer.Ejecutar(ts));
                }
                else if (Tipo == Tipo.OR)
                {
                    return ((bool)operadorIzq.Ejecutar(ts)) || ((bool)operadorDer.Ejecutar(ts));
                }
                else if (Tipo == Tipo.XOR)
                {
                    return ((bool)operadorIzq.Ejecutar(ts)) ^ ((bool)operadorDer.Ejecutar(ts));
                }
                else if (Tipo == Tipo.NOT)
                {
                    return !(bool)operadorIzq.Ejecutar(ts);
                }
                else if (Tipo == Tipo.CONCATENACION)
                {
                    return operadorIzq.Ejecutar(ts).ToString() + operadorDer.Ejecutar(ts).ToString();
                }
                else if (Tipo == Tipo.BOOLEANO)
                {
                    return Convert.ToBoolean(valor);
                }
                else if (Tipo == Tipo.VARIABLE_METODOS)
                {
                    Variable var = (Variable)valor;
                    Tipo tipo = ts.getType(var.Id);
                    object aux = Program.getValor(tipo, var.Id, var.Valor, ts);
                    if (aux != null) return aux;
                    else
                    {
                        salida.Add(Program.buildError(getLine(), getColumn(), "Semantico", " IndexOutException."));
                        return null;
                    }
                }
                else
                {
                    return null;
                }
            } catch {
                salida.Add(Program.buildError(getLine(), getColumn(), "Semantico", " No se puede operar el tipo."));
                return null;
            }
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
            return salida;
        }

        public object Recolectar(TablaDeSimbolos ts)
        {
            return null;
        }

        public Tipo getType()
        {
            return Tipo.OPERACION;
        }
    }
}
