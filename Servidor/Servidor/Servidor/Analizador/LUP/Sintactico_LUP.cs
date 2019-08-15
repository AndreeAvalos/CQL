using Irony.Parsing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Servidor.Analizador.LUP
{
    public class Sintactico_LUP
    {
        public bool validar(String entrada, Grammar gramatica) {
            LanguageData lenguaje = new LanguageData(gramatica);
            Parser parser = new Parser(lenguaje);
            ParseTree arbol = parser.Parse(entrada);
            if (arbol.Root!=null) return true;
            else return false;
        }
        public ParseTreeNode Analizar(String entrada, Grammar gramatica) {
            LanguageData lenguaje = new LanguageData(gramatica);
            Parser parser = new Parser(lenguaje);
            ParseTree arbol = parser.Parse(entrada);

            return arbol.Root.ChildNodes.ElementAt(0);
        }


    }
}
