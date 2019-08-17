using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Servidor.NOSQL.Modelos
{
    public class Tabla
    {
        string name;
        List<Columna> columnas;
        List<Fila> filas;

        string anexo;
        bool exportada = false;
    }
}
