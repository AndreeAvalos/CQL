using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Servidor.Models
{
    public class Drop_DataBase : Instruccion
    {
        private string id;

        public Drop_DataBase(string id)
        {
            this.id = id;
        }

        public object Recolectar(TablaDeSimbolos ts) { return null; }
        public object Ejecutar(TablaDeSimbolos ts) {
            Program.sistema.deleteDataBase(id);
            return null;
        }
    }
}
