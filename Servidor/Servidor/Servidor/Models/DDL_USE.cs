using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Servidor.Models
{
    public class DDL_USE :Instruccion
    {
        private string id;

        public DDL_USE(string id)
        {
            this.id = id;
        }

        public object Recolectar(TablaDeSimbolos ts) {
            return null;
        }
        public object Ejecutar(TablaDeSimbolos ts) {
            if (Program.sistema.existDataBase(id.ToLower())) Program.sistema.asignUse(id);
            else
            {
                //informar que no existe database

            }
            return null;
        }
    }
}
