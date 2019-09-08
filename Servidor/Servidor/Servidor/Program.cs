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
using Servidor.Models.CASTEOS;
using Servidor.Models.FCL;
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
        public static Casteo casteos = new Casteo();
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

        public static Tipo getTipo(string name)
        {
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
                default:
                    real_type = Tipo.USER_TYPES;
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
                default:
                    real_type = Tipo.USER_TYPES;
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

        public static object getValor(Tipo tipo, string id, object valor, TablaDeSimbolos ts)
        {
            switch (tipo)
            {
                case Tipo.MAP:
                    Map map_actual = (Map)ts.getValor(id);
                    Variable_Metodo aux = (Variable_Metodo)valor;
                    string clave;
                    if (aux.Metodo.ToLower().Equals("get"))
                    {
                        clave = aux.Clave.Ejecutar(ts).ToString();
                        aux.Clave.Ejecutar(ts).ToString();
                        if (map_actual.containsKey(clave))
                        {
                            Tipo_Collection val = (Tipo_Collection)map_actual.Get(clave);

                            if (val.Real_type == Tipo.OPERACION)
                            {
                                Operacion op = (Operacion)val.Valor;
                                return op.Ejecutar(ts);
                            }

                        }
                    }
                    else if (aux.Metodo.ToLower().Equals("insert"))
                    {
                        clave = aux.Clave.Ejecutar(ts).ToString();
                        bool salida = map_actual.insert(clave, aux.Valor);
                        ts.setValor(id, map_actual.Mapita);
                        return salida;
                    }
                    else if (aux.Metodo.ToLower().Equals("set"))
                    {
                        clave = aux.Clave.Ejecutar(ts).ToString();
                        bool salida = map_actual.Set(clave, aux.Valor);
                        ts.setValor(id, map_actual.Mapita);
                        return salida;
                    }
                    else if (aux.Metodo.ToLower().Equals("remove"))
                    {
                        clave = aux.Clave.Ejecutar(ts).ToString();
                        bool salida = map_actual.Remove(clave);
                        ts.setValor(id, map_actual.Mapita);
                        return salida;
                    }
                    else if (aux.Metodo.ToLower().Equals("clear"))
                    {
                        map_actual.clear();
                        ts.setValor(id, map_actual.Mapita);
                        return true;
                    }
                    else if (aux.Metodo.ToLower().Equals("size"))
                    {
                        return Convert.ToDouble(map_actual.Size());

                    }
                    else if (aux.Metodo.ToLower().Equals("contains"))
                    {
                        clave = aux.Clave.Ejecutar(ts).ToString();
                        return map_actual.containsKey(clave);
                    }
                    break;

                default:
                    return null;
            }
            return null;
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
