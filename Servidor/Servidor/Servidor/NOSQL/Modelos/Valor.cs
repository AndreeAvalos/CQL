using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Servidor.NOSQL.Modelos
{
    public class Valor
    {
        string id;
        object val;

        public Valor(string id, object val)
        {
            this.id = id;
            this.Val = val;
        }

        public string Id { get => id; set => id = value; }
        public object Val { get => val; set => val = value; }

        public string ArmarRespuesta() {

           return "<td>" + this.val + "</td>";

        }
    }
}
