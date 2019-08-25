using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Servidor.Models
{
    public interface Instruccion
    {
        Object Ejecutar(TablaDeSimbolos ts);
        Object Recolectar(TablaDeSimbolos ts);
        int getLine();
        int getColumn();
        List<string> getSalida();
    }
}


