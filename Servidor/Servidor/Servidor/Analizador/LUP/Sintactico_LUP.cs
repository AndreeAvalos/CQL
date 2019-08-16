using Irony.Parsing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Servidor.Analizador.LUP
{
    public class Sintactico_LUP
    {
        public List<string> salida = new List<string>();
        public bool Validar(String entrada, Grammar gramatica) {
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
            ParseTreeNode raiz = arbol.Root;

            Instrucciones(raiz.ChildNodes.ElementAt(0));
            return arbol.Root.ChildNodes.ElementAt(0);
        }

        private void Instrucciones(ParseTreeNode nodo) {
            if (nodo.ChildNodes.Count == 2) {
                Instrucciones(nodo.ChildNodes.ElementAt(0));
                Instruccion(nodo.ChildNodes.ElementAt(1));
            }
            else Instruccion(nodo.ChildNodes.ElementAt(0));

        }

        private void Instruccion(ParseTreeNode nodo)
        {
            if (nodo.ChildNodes.ElementAt(0).ToString() == "LOGIN") LOGIN(nodo.ChildNodes.ElementAt(0));
            else if (nodo.ChildNodes.ElementAt(0).ToString() == "LOGOUT") LOGOUT(nodo.ChildNodes.ElementAt(0));
            else if (nodo.ChildNodes.ElementAt(0).ToString() == "QUERY") QUERY(nodo.ChildNodes.ElementAt(0));
            else if (nodo.ChildNodes.ElementAt(0).ToString() == "STRUC") STRUC(nodo.ChildNodes.ElementAt(0));
        }

        public void LOGIN(ParseTreeNode nodo) {
            //obtener el user y pass
            string user = Valor(nodo.ChildNodes[4].ChildNodes[4]);
            string pass = Valor(nodo.ChildNodes[5].ChildNodes[4]);

            //Metodo que verifica el password y genera un resultado

            LOGEAR(user,pass);

        }

        private void LOGEAR(string user, string pass)
        {
         
            salida.Add("[+LOGIN][SUCCESS][-LOGIN]");
        }

        public void LOGOUT(ParseTreeNode nodo)
        {
            //obtener el user
            string user = Valor(nodo.ChildNodes[4].ChildNodes[4]);

            DESLOGEAR(user);

        }
        private void DESLOGEAR(string user)
        {
            //metodo para desloquerse
            salida.Add("[+LOGOUT][SUCCESS][-LOGOUT]");
        }

        private void QUERY(ParseTreeNode nodo)
        {
            string user = Valor(nodo.ChildNodes[4].ChildNodes[4]);
            string data = Valor(nodo.ChildNodes[5].ChildNodes[4]);

            data = data.Replace("\\\"","\"");

            EjecutarQuery(data);

        }

        private void EjecutarQuery(string data)
        {
            //Ejecutar las sentencias query que traiga el string 
            salida.Add("[+DATA]" + data + "[-DATA]");
        }

        private void STRUC(ParseTreeNode nodo)
        {
            string user = Valor(nodo.ChildNodes[4].ChildNodes[4]);

            getEstructura(user);
        }

        private void getEstructura(string data)
        {
            //metodo para traer toda la estructura 
            salida.Add("[+DATABASES][+DATABASE][+NAME] Prueba1[-NAME][+TABLES][+TABLE][+NAME] Prueba1[-NAME][+COLUMNS] Column1[-COLUMNS][-TABLE][+TABLE] MAESTRO[-TABLE][-TABLES][+TYPES][+TYPE][+NAME] Persona[-NAME][+ATTRIBUTES] nombre[-ATTRIBUETS][-TYPE][+TYPE] DOMICILIO[-TYPE][-TYPES][+PROCEDURES] CreateStudent[-PROCEDURES][-DATABASE][-DATABASES]");

        }

        public string Valor(ParseTreeNode node) {

            String evaluar = node.ChildNodes[0].Term.Name;

            switch (evaluar) {
                case "Cadena":
                    return node.ChildNodes[0].ToString().Replace(" (Cadena)", "");
                case "Identificador":
                    return node.ChildNodes[0].ToString().Replace(" (Identificador)", "");
                case "Numero":
                    return node.ChildNodes[0].ToString().Replace(" (Numero)", "");
                default:
                    return "";
            }
         

        }
    }
}
