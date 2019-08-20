﻿using System;
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
        public static void Main(string[] args)
        {
            crearDB();
            CreateWebHostBuilder(args).Build().Run();
            
        }

        private static void crearDB()
        {
            String text = System.IO.File.ReadAllText("Principal.chison");
            Sintactico_CHISHON sintactico = new Sintactico_CHISHON();
            
            if (sintactico.Validar(text, new Gramatica_CHISON())) {
                sintactico.Analizar(text, new Gramatica_CHISON());
                sistema = sintactico.db_nosql;
                backup_sistema = sistema;
            }

        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }
}
