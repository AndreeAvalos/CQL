using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Servidor.Models
{
    public class Tipo_Case
    {
        int line, column;
        string expresion;
        LinkedList<Instruccion> instrucciones;

        public Tipo_Case(string expresion, LinkedList<Instruccion> instrucciones, int line, int column)
        {
            this.line = line;
            this.column = column;
            this.expresion = expresion;
            this.Instrucciones = instrucciones;
        }

        public string Expresion { get => expresion; set => expresion = value; }
        public int Line { get => line; set => line = value; }
        public int Column { get => column; set => column = value; }
        public LinkedList<Instruccion> Instrucciones { get => instrucciones; set => instrucciones = value; }
    }
}
