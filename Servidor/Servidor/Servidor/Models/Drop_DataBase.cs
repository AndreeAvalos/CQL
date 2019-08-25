using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Servidor.Models
{
    public class Drop_DataBase : Instruccion
    {
        private string id;
        public List<string> salida = new List<string>();
        public Drop_DataBase(string id)
        {
            this.id = id;
        }

        public object Recolectar(TablaDeSimbolos ts) { return null; }
        public object Ejecutar(TablaDeSimbolos ts) {
            if (Program.sistema.existDataBase(id.ToLower())) Program.sistema.deleteDataBase(id);
            else {
                //informar que no existe database

            }
            return null;
        }
        public List<string> getSalida()
        {

            return salida;
        }

    }
}
