using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Servidor.Analizador.CHISON;
using Servidor.NOSQL.Estructuras;

namespace Servidor
{
    public class Program
    {
        public static Manejo sistema, backup_sistema;
        public static List<string> log_errores = new List<string>();


        public static bool bloqueada = false;//para pruebas funcionara sin logearse
        public static string user_activo = "Andree";//para pruebas 
        public static void Main(string[] args)
        {
            crearDB();
            CreateWebHostBuilder(args).Build().Run();

        }

        private static void crearDB()
        {
            String text = File.ReadAllText("./NOSQL/Generados/Principal.chison");
            sistema = new Manejo();
            Sintactico_CHISHON sintactico = new Sintactico_CHISHON();

            if (sintactico.Validar(text, new Gramatica_CHISON()))
            {
                sintactico.Analizar(text, new Gramatica_CHISON());
                sistema = sintactico.db_nosql;
                backup_sistema = sistema;
            }
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

        public static bool execCommit() {
            string path = "./NOSQL/Generados/Principal.chison";
            string text = "$<\n";
            text += sistema.execComit(1) +"\n";
            text += ">$";
            File.WriteAllText(path, text);
            return true;
        }

        public static string getTabulaciones(int num_tabs) {

            string tabulaciones = "";
            for (int i = 0; i < num_tabs; i++)
            {
                tabulaciones += "\t";

            }
            return tabulaciones;
        }
        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }
}
