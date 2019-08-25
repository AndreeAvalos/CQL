using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Servidor.Models
{
    public class Create_DataBase : Instruccion
    {
        private bool existe;
        private string id;
        private string user;
        public List<string> salida = new List<string>();
        public Create_DataBase(bool existe, string id,string user)
        {
            this.existe = existe;
            this.id = id;
            this.user = user;
        }
        public List<string> getSalida()
        {

            return salida;
        }

        public object Recolectar(TablaDeSimbolos ts) {
            return null;
        }
        public Object Ejecutar(TablaDeSimbolos ts) {
            if (Program.sistema.existDataBase(id) == true)
            {
                if (existe == true)
                {
                    //no hacer nada
                }
                else
                {
                    //Imprimir error ya que la tabla 
                }
            }
            else {
                Program.sistema.addDataBase(id);
                Program.sistema.addPermissions(user, id);
                Program.sistema.addPermissions("admin", id);

            }
            return null;
        }

    }
}
