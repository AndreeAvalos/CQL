using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Servidor.NOSQL.Modelos
{
    public class Parametro
    {
        string name;
        string type;
        string out__;
        bool out_ = false;

        public string Name { get => name; set => name = value; }
        public string Type { get => type; set => type = value; }
        public bool Out_ { get => out_; set => out_ = value; }
        public string Out__ { get => out__; set => out__ = value; }

        public string ArmarRespuesta() {
            return "<td>"+name+"</td>"+ "<td>" + type + "</td>"+ "<td>" + out__ + "</td>";
        }
    }
}
