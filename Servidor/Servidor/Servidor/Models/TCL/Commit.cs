﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Servidor.Models.TCL
{
    public class Commit: Instruccion
    {
        List<string> salida = new List<string>();
        public object Recolectar(TablaDeSimbolos ts) { return null; }
        public object Ejecutar(TablaDeSimbolos ts) {

            return null;

        }
        public List<string> getSalida() { return salida; }

        public int getLine()
        {
            throw new NotImplementedException();
        }

        public int getColumn()
        {
            throw new NotImplementedException();
        }
    }
}