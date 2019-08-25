using Irony.Parsing;
using Servidor.Analizador.CQL;
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
            if (Program.sistema.Buscar_Usuario(user, pass)) {
                salida.Add("[+LOGIN]\n\t[SUCCESS]\n[-LOGIN]");
            }else salida.Add("[+LOGIN]\n\t[FAIL]\n[-LOGIN]");
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
            string data = nodo.ChildNodes[5].ChildNodes[0].Token.Text.Replace("[+DATA]","");
            data = data.Replace("[-DATA]", "");
            data = data.Replace("\\\"", "\"");




            EjecutarQuery(data, user);

        }

        private void EjecutarQuery(string data, string user)
        {
            Sintactico_CQL sintactico = new Sintactico_CQL();
            sintactico.Analizar(data, new Gramatica_CQL(),user);
            //Ejecutar las sentencias query que traiga el string 
            salida = sintactico.salida;
        }

        private void STRUC(ParseTreeNode nodo)
        {
            string user = Valor(nodo.ChildNodes[4].ChildNodes[4]);

            getEstructura(user);
        }

        private void getEstructura(string user)
        {
            //metodo para traer toda la estructura 
            salida.Add(Program.sistema.Crear_Estructura(user));

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
