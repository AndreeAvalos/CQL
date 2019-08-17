using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Servidor.NOSQL.Modelos
{
    public class Database
    {
        string Name;
        List<Tabla> tablas;
        List<Objeto> objetos;
        List<Procedure> procedures;

        string anexo;
        bool exportada = false;



    }
}
