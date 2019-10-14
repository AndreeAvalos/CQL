using Servidor.Models.FCL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Servidor.Models
{
    public class Tipo_Switch
    {
        public Tipo_Switch(Operacion expresion, List<Tipo_Case> casos, LinkedList<Instruccion> instrucciones_default)
        {
            this.Expresion = expresion;
            this.Casos = casos;
            this.Instrucciones_default = instrucciones_default;
        }

        public Operacion Expresion { get; set; }
        public List<Tipo_Case> Casos { get; set; }
        public LinkedList<Instruccion> Instrucciones_default { get; set; }
    }
}
