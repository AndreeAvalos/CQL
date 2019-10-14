using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Servidor.Models
{
    public class Tipo_Collection
    {
        Tipo real_type;
        Object valor;
        string as_type;

        public Tipo_Collection(Tipo real_type, object valor, string as_type)
        {
            this.Real_type = real_type;
            this.Valor = valor;
            this.as_type = as_type;
        }

        public Tipo Real_type { get => real_type; set => real_type = value; }
        public object Valor { get => valor; set => valor = value; }
        public string As_type { get => as_type; set => as_type = value; }
    }
}
