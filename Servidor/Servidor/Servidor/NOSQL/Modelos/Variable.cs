using Servidor.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Servidor.NOSQL.Modelos
{
    public class Variable
    {
        string id;
        object valor;
        Tipo real_type;
        string tipo;
        bool instanciada;
        string attr1, attr2;
        bool is_var = false;
        List<string> lst_variables = new List<string>();
        public string Id { get => id; set => id = value; }
        public object Valor { get => valor; set => valor = value; }
        public Tipo Real_type { get => real_type; set => real_type = value; }
        public string Tipo { get => tipo; set => tipo = value; }
        public bool Instanciada { get => instanciada; set => instanciada = value; }
        public string Attr1 { get => attr1; set => attr1 = value; }
        public string Attr2 { get => attr2; set => attr2 = value; }
        public bool Is_var { get => is_var; set => is_var = value; }
        public List<string> Lst_variables { get => lst_variables; set => lst_variables = value; }
    }
}
