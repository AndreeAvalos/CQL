using Servidor.Models.FCL;

namespace Servidor.Models
{
    public class Variable_Metodo
    {
        public Variable_Metodo(string metodo)
        {
            this.Metodo = metodo;
        }

        public string Metodo { get; set; }
        public Operacion Clave { get; set; }
        public Tipo_Collection Valor { get; set; }
    }
}
