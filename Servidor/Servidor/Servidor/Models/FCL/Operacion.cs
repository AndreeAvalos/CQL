using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Servidor.Models.FCL
{
    public class Operacion : Instruccion
    {
        Tipo tipo;
        int line, column;
        List<string> salida = new List<string>();
        private Operacion operadorDer;
        private Operacion operadorIzq;
        private Object valor;

        public Operacion(Operacion operadorIzq, Operacion operadorDer, Tipo tipo, int line, int column)
        {
            this.tipo = tipo;
            this.line = line;
            this.column = column;
            this.operadorDer = operadorDer;
            this.operadorIzq = operadorIzq;
        }

        public Operacion(Operacion operadorIzq, Tipo tipo, int line, int column)
        {
            this.tipo = tipo;
            this.line = line;
            this.column = column;
            this.operadorIzq = operadorIzq;
        }

        public Operacion(object valor, Operacion operadorIzq, Tipo tipo, int line, int column)
        {
            this.tipo = tipo;
            this.line = line;
            this.column = column;
            this.operadorIzq = operadorIzq;
            this.valor = valor;
        }

        public Operacion(string valor, Tipo tipo, int line, int column)
        {
            this.tipo = tipo;
            this.line = line;
            this.column = column;
            this.valor = valor;
        }

        public Operacion(object valor, int line, int column)
        {
            this.line = line;
            this.column = column;
            this.valor = valor;
            this.tipo = Tipo.NUMERO;
        }

        public object Ejecutar(TablaDeSimbolos ts)
        {
            if (tipo == Tipo.DIVISION)
            {
                return (Double)operadorIzq.Ejecutar(ts) / (Double)operadorDer.Ejecutar(ts);
            }
            else if (tipo == Tipo.MODULAR)
            {
                return (Double)operadorIzq.Ejecutar(ts) % (Double)operadorDer.Ejecutar(ts);
            }
            else if (tipo == Tipo.POTENCIA)
            {
                return (Double)Math.Pow((Double)operadorIzq.Ejecutar(ts), (Double)operadorDer.Ejecutar(ts));
            }
            else if (tipo == Tipo.MULTIPLICACION)
            {
                return (Double)operadorIzq.Ejecutar(ts) * (Double)operadorDer.Ejecutar(ts);
            }
            else if (tipo == Tipo.RESTA)
            {
                return (Double)operadorIzq.Ejecutar(ts) - (Double)operadorDer.Ejecutar(ts);
            }
            else if (tipo == Tipo.SUMA)
            {
                return (Double)operadorIzq.Ejecutar(ts) + (Double)operadorDer.Ejecutar(ts);
            }
            else if (tipo == Tipo.NEGATIVO)
            {
                return (Double)operadorIzq.Ejecutar(ts) * -1;
            }
            else if (tipo == Tipo.NUMERO)
            {
                return Double.Parse(valor.ToString());
            }
            else if (tipo == Tipo.VARIABLE)
            {
                return ts.getValor(valor.ToString());
            }
            else if (tipo == Tipo.CADENA)
            {
                return valor.ToString();
            }
            else if (tipo == Tipo.INCREMENTO)
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
            else if (tipo == Tipo.DECREMENTO)
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
            else if (tipo == Tipo.MASIGUAL)
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
            else if (tipo == Tipo.MENOSIGUAL)
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
            else if (tipo == Tipo.MULIGUAL)
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
            else if (tipo == Tipo.DIVIGUAL)
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
            else if (tipo == Tipo.MAYOR_QUE)
            {
                return ((Double)operadorIzq.Ejecutar(ts)) > ((Double)operadorDer.Ejecutar(ts));
            }
            else if (tipo == Tipo.MAYOR_IGUAL)
            {
                return ((Double)operadorIzq.Ejecutar(ts)) >= ((Double)operadorDer.Ejecutar(ts));
            }
            else if (tipo == Tipo.MENOR_QUE)
            {
                return ((Double)operadorIzq.Ejecutar(ts)) < ((Double)operadorDer.Ejecutar(ts));
            }
            else if (tipo == Tipo.MENOR_IGUAL)
            {
                return ((Double)operadorIzq.Ejecutar(ts)) <= ((Double)operadorDer.Ejecutar(ts));
            }
            else if (tipo == Tipo.IGUAL)
            {
                return ((Double)operadorIzq.Ejecutar(ts)) == ((Double)operadorDer.Ejecutar(ts));
            }
            else if (tipo == Tipo.DIFERENTE)
            {
                return ((Double)operadorIzq.Ejecutar(ts)) != ((Double)operadorDer.Ejecutar(ts));
            }
            else if (tipo == Tipo.AND)
            {
                return ((bool)operadorIzq.Ejecutar(ts)) && ((bool)operadorDer.Ejecutar(ts));
            }
            else if (tipo == Tipo.OR)
            {
                return ((bool)operadorIzq.Ejecutar(ts)) || ((bool)operadorDer.Ejecutar(ts));
            }
            else if (tipo == Tipo.XOR)
            {
                return ((bool)operadorIzq.Ejecutar(ts)) ^ ((bool)operadorDer.Ejecutar(ts));
            }
            else if (tipo == Tipo.NOT)
            {
                return !(bool)operadorIzq.Ejecutar(ts);
            }
            else if (tipo == Tipo.CONCATENACION)
            {
                return operadorIzq.Ejecutar(ts).ToString() + operadorDer.Ejecutar(ts).ToString();
            }
            else if (tipo == Tipo.BOOLEANO) {
                return Convert.ToBoolean(valor);
            }
            else
            {
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
    }
}
