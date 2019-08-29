using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Irony;
using Irony.Parsing;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Servidor.Analizador.CHISON;
using Servidor.Models;
using Servidor.NOSQL.Estructuras;
using Servidor.NOSQL.Modelos;

namespace Servidor
{
    public class Program
    {
        public static Manejo sistema, backup_sistema;
        public static List<string> log_errores = new List<string>();
        public static List<Error> errors = new List<Error>();
        private static DateTime date;
        public static bool bloqueada = false;//para pruebas funcionara sin logearse
        public static string user_activo = "Andree";//para pruebas 
        public static void Main(string[] args)
        {
            crearDB();
            CreateWebHostBuilder(args).Build().Run();

        }

        private static void crearDB()
        {
            readErrors();
            String text = File.ReadAllText("./NOSQL/Generados/Principal.chison");
            sistema = new Manejo();
            Sintactico_CHISHON sintactico = new Sintactico_CHISHON();


            sintactico.Analizar(text, new Gramatica_CHISON());
            sistema = sintactico.db_nosql;
            backup_sistema = sistema;

        }

        public static Tipo getTipo(string name) {
            Tipo real_type = Tipo.OBJETO;
            switch (name)
            {
                case "int":
                    real_type = Tipo.ENTERO;
                    break;
                case "double":
                    real_type = Tipo.DECIMAL;
                    break;
                case "string":
                    real_type = Tipo.CADENA;
                    break;
                case "boolean":
                    real_type = Tipo.BOOLEANO;
                    break;
                case "time":
                    real_type = Tipo.TIME;
                    break;
                case "date":
                    real_type = Tipo.DATE;
                    break;
                case "map":
                    real_type = Tipo.MAP;
                    break;
                case "set":
                    real_type = Tipo.SET;
                    break;
                case "list":
                    real_type = Tipo.LIST;
                    break;
                case "":
                    real_type = Tipo.OBJETO;
                    break;
            }
            return real_type;
        }

        public static Tipo getTipo2(string name)
        {
            Tipo real_type = Tipo.OBJETO;
            switch (name)
            {
                case "numero":
                    real_type = Tipo.ENTERO;
                    break;
                case "cadena":
                    real_type = Tipo.CADENA;
                    break;
                case "boolean":
                    real_type = Tipo.BOOLEANO;
                    break;
                case "time":
                    real_type = Tipo.TIME;
                    break;
                case "date":
                    real_type = Tipo.DATE;
                    break;
                case "map":
                    real_type = Tipo.MAP;
                    break;
                case "set":
                    real_type = Tipo.SET;
                    break;
                case "list":
                    real_type = Tipo.LIST;
                    break;
                case "":
                    real_type = Tipo.OBJETO;
                    break;
            }
            return real_type;
        }

        public static bool comprobarPrimitivo(string name)
        {

            switch (name.ToLower())
            {
                case "string":
                    return true;
                case "int":
                    return true;
                case "double":
                    return true;
                case "time":
                    return true;
                case "date":
                    return true;
                case "boolean":
                    return true;
                case "counter":
                    return true;
                case "map":
                    return true;
                case "set":
                    return true;
                case "list":
                    return true;
            }
            return false;
        }

        //metodo que crea un mensaje de tipo LUP
        public static string buildMessage(string message)
        {
            string salida = "[+MESSAGE]\n";
            salida += "\"" + message + "\"";
            salida += "\n[-MESSAGE]";
            return salida;
        }
        //metodo que crea un mensaje de tipo LUP
        public static string buildData(string data)
        {
            string salida = "[+DATA]\n";
            salida += "\"" + data + "\"";
            salida += "\n[-DATA]";
            return salida;
        }
        //metodo que crea un mensaje de tipo LUP
        public static string buildError(int linea, int columna, string type, string descripcion)
        {
            string salida = "[+ERROR]\n";
            salida += "[+LINE]\n";
            salida += linea;
            salida += "\n[-LINE]\n";
            salida += "[+COLUMN]\n";
            salida += columna;
            salida += "\n[-COLUMN]\n";
            salida += "[+TYPE]\n";
            salida += type;
            salida += "\n[-TYPE]\n";
            salida += "[+DESC]\n";
            salida += "\"" + descripcion + "\"";
            salida += "\n[-DESC]\n";
            salida += "[-ERROR]";

            log_errores.Add(salida);
            return salida;
        }
        //metodo que crea un mensaje de tipo LUP
        public static string buildDataBase()
        {
            string salida = "";
            return salida;
        }

        public static void execRollbak()
        {
            sistema = backup_sistema;
        }

        public static bool execCommit()
        {
            string path = "./NOSQL/Generados/Principal.chison";
            string text = "$<\n";
            text += sistema.execComit(1) + "\n";
            text += ">$";
            File.WriteAllText(path, text);
            return true;
        }

        public static string getTabulaciones(int num_tabs)
        {

            string tabulaciones = "";
            for (int i = 0; i < num_tabs; i++)
            {
                tabulaciones += "\t";

            }
            return tabulaciones;
        }

        public static List<string> lst_Errors(ParseTree arbol)
        {
            List<string> salida = new List<string>();
            foreach (LogMessage item in arbol.ParserMessages)
            {
                if (item.Message.ToString().Contains("Invalid character"))
                {
                    salida.Add(buildError(item.Location.Line, item.Location.Column, "Lexico", item.Message.ToString()));
                }
                else salida.Add(buildError(item.Location.Line, item.Location.Column, "Sintactico", item.Message.ToString()));
            }
            return salida;
        }
        public static void addError(ParseTree arbol)
        {

            foreach (LogMessage item in arbol.ParserMessages)
            {
                if (item.Message.ToString().Contains("Invalid character"))
                {
                    date = DateTime.Now;
                    errors.Add(new Error("Lexico", item.Message, item.Location.Line, item.Location.Column, date.Date.Day + "/" + date.Month + "/" + date.Year, date.Hour + ":" + date.Minute + ":" + date.Second));

                }
                else errors.Add(new Error("Sintactico", item.Message, item.Location.Line, item.Location.Column, date.Date.Day + "/" + date.Month + "/" + date.Year, date.Hour + ":" + date.Minute + ":" + date.Second));

            }

        }
        public static void writeErrors()
        {
            string salida = JsonConvert.SerializeObject(Program.errors);
            File.WriteAllText("./NOSQL/Generados/Log_Errors.json", salida);
        }
        public static void readErrors()
        {
            string entrada = File.ReadAllText("./NOSQL/Generados/Log_Errors.json");
            if (JsonConvert.DeserializeObject<List<Error>>(entrada) != null)
                errors = JsonConvert.DeserializeObject<List<Error>>(entrada);
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }
}
